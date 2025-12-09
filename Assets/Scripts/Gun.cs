using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Gun : MonoBehaviour {

    public Camera camera;

    [Header("Gun Settings")] 
    [Tooltip("The Size of the Primary fire magazine")] public int primaryMagSize;
    [Tooltip("The Size of the Secondary fire magazine")] public int secondaryMagSize;
    [Tooltip("The rate of fire for primary ammo")] public float primaryFireRate;
    [Tooltip("The rate of fire for secondary ammo")] public float secondaryFireRate;
    public GameObject altSpawnPoint;
    [FormerlySerializedAs("hudEnabled")] public bool isPlayer;
    
    [Header("Ammo")]
    public ScriptableObjects.Ammo primaryAmmo;
    public ScriptableObjects.Ammo secondaryAmmo;

    [Header("Audio Settings")]
    public AudioSource audioSource;


    [Header("Gun HUD Settings")]
    public TextMeshProUGUI text;
    public GameObject bulletHole;
    
    private bool allowedPrimaryFire = true;
    private bool allowedSecondaryFire = true;

    private bool tryFire;

    private bool isReloading = false;
    
    private int _currentPrimaryAmmo;
    private int _currentSecondaryAmmo;

    
    
    #region TIMERS

    private float _primaryReloadTimer;
    private float _alternateReloadTimer;
    private float _primaryRateOfFireTimer;
    private float _secondaryRateOfFireTimer;
    
    #endregion
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentPrimaryAmmo = primaryMagSize;
        _currentSecondaryAmmo = secondaryMagSize;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (isPlayer) {
            if (Input.GetKey(KeyCode.Mouse0)) {
                if (allowedPrimaryFire && _currentPrimaryAmmo > 0 && Time.timeScale > 0) { // Only try to fire if we are allowed to fire and have ammo
                    TryFire();
                }
            }

            if (Input.GetKey(KeyCode.Mouse1)) {
                if (allowedPrimaryFire && _currentSecondaryAmmo > 0 && Time.timeScale > 0) { // Only try to fire if we are allowed to fire and have ammo
                    TryAltFire();
                }
            }

            if (Input.GetKey(KeyCode.R) && Time.timeScale > 0) { // Reload if we press R and the game is not paused
                isReloading = true;
                _primaryReloadTimer = primaryAmmo.ReloadTime;
                _alternateReloadTimer = secondaryAmmo.ReloadTime;
            }

            text.text = $"{primaryAmmo.AmmoTitle}: {_currentPrimaryAmmo}/{primaryMagSize}\n{secondaryAmmo.AmmoTitle}: {_currentSecondaryAmmo}/{secondaryMagSize}";
        }

         
        if (isReloading && _primaryReloadTimer > 0) { // if we are reloading and the timer hasn't finished
            _primaryReloadTimer -= Time.deltaTime; // Decrease the timer
            allowedPrimaryFire = false; // Disallow firing
            //ebug.Log("PRIM RELOAD: " + _primaryReloadTimer);
        } else if (isReloading && _primaryReloadTimer <= 0) {  // if we are reloading and the timer has finished
            _currentPrimaryAmmo = primaryMagSize; // Refill the magazine 
            allowedPrimaryFire = true; // Allow firing again
        }
        
        if (isReloading && _alternateReloadTimer > 0) { // if we are reloading and the timer hasn't finished
            _alternateReloadTimer -= Time.deltaTime; // Decrease the timer
            allowedSecondaryFire = false; // Disallow firing
            //Debug.Log($"ALT RELOAD: {_alternateReloadTimer}");
        } else if (isReloading && _alternateReloadTimer <= 0) { // if we are reloading and the timer has finished
            _currentSecondaryAmmo = secondaryMagSize; // Refill the magazine
            allowedSecondaryFire = true; // Allow firing again
        }

        if (_currentPrimaryAmmo == primaryMagSize && _currentSecondaryAmmo == secondaryMagSize) { // if both mags are full, we are no longer reloading
            isReloading = false;
        }
        
        if (allowedPrimaryFire == false && _primaryRateOfFireTimer > 0) { // if we are not allowed to fire and the timer hasn't finished
            _primaryRateOfFireTimer -= Time.deltaTime; // Decrease the timer
        } else { // if the timer has finished
            allowedPrimaryFire = true; // Allow firing again
        }
        
        if (allowedSecondaryFire == false && _secondaryRateOfFireTimer > 0) { // if we are not allowed to fire and the timer hasn't finished, continue decreasing the timer
            _secondaryRateOfFireTimer -= Time.deltaTime;
        } else {                                                              // if the timer has finished allow firing again
            allowedSecondaryFire = true;
        }

        //Debug.Log($"ALLOWED PRIM FIRE: {allowedPrimaryFire}");
        //Debug.Log($"ALLOWED SECOND FIRE: {allowedSecondaryFire}");
    }


    /// <summary>
    /// Fires the primary weapon
    /// </summary>
    public void TryFire() {
        _currentPrimaryAmmo--;
        if (primaryAmmo.SpawnsGameObject) { // if it is a physical weapon
            var bullet = Instantiate(primaryAmmo.BulletPrefab, altSpawnPoint.transform); // Create an instance of the bullet prefab at the spawn point
            bullet.GetComponent<Rigidbody>().AddForce(altSpawnPoint.transform.forward * primaryAmmo.Power); // Add force to the bullet to propel it forward
            bullet.transform.parent = null; // Detach the bullet from the gun so it doesn't move with it
        } else { // if the weapon is hitscan
            Ray ray;
            if (isPlayer) {
                ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // if the gun is held by the player, shoot from the center of the screen
            } else {
                ray = new Ray(altSpawnPoint.transform.position, altSpawnPoint.transform.forward); // if the gun is held by an NPC, shoot from the gun's spawn point
            }

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) { // If we have hit something
                if (hit.transform.tag == "MoveableObject") {
                    var direction = new Vector3(
                        hit.transform.position.x - transform.position.x,
                        hit.transform.position.y - transform.position.y,
                        hit.transform.position.z - transform.position.z
                    );// Calculate direction from gun to hit object 
                    hit.rigidbody.AddForceAtPosition(Vector3.Normalize(direction) * primaryAmmo.Power, hit.point);
                }

                // Get the Health component of the object we hit
                Health objHealth = hit.transform.GetComponent<Health>();
                if (objHealth) { // If the object has a Health component, apply damage
                    objHealth.Damage(primaryAmmo.Power);
                }

                // Instantiate a bullet hole at the point of impact, and parent it to the object we hit, with a random rotation
                var tempBullet = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                tempBullet.transform.parent = hit.transform;
                tempBullet.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
            }
        }

        // Play the firing sound, and set the rate of fire timer and trigger combat music
        audioSource.PlayOneShot(primaryAmmo.fireSound);
        allowedPrimaryFire = false;
        _primaryRateOfFireTimer = 1 / primaryFireRate;
        FindAnyObjectByType<BGMSelector>()?.TriggerCombat();
    }

    /// <summary>
    /// Fires the secondary weapon
    /// </summary>
    public void TryAltFire() {
        // See above
        if (allowedSecondaryFire && _currentSecondaryAmmo > 0) {
            _currentSecondaryAmmo--;
            if (secondaryAmmo.SpawnsGameObject) { // if it is a physical weapon
                var bullet = Instantiate(secondaryAmmo.BulletPrefab, altSpawnPoint.transform);
                bullet.GetComponent<Rigidbody>().AddForce(altSpawnPoint.transform.forward * secondaryAmmo.Power);
                bullet.transform.parent = null;
            } else { // if the weapon is hitscan
                Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.transform.tag == "MoveableObject") {
                        var direction = new Vector3(
                            hit.transform.position.x - transform.position.x,
                            hit.transform.position.y - transform.position.y,
                            hit.transform.position.z - transform.position.z
                        );
                        hit.rigidbody.AddForceAtPosition(Vector3.Normalize(direction) * secondaryAmmo.Power, hit.point);
                    }


                    Health objHealth = hit.transform.GetComponent<Health>();
                    if (objHealth) {
                        objHealth.Damage(secondaryAmmo.Power);
                    }

                    var tempBullet = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                    tempBullet.transform.parent = hit.transform;
                    tempBullet.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
                }
            }
            audioSource.PlayOneShot(secondaryAmmo.fireSound);
            allowedSecondaryFire = false;
            _secondaryRateOfFireTimer = 1 / secondaryFireRate;
            FindAnyObjectByType<BGMSelector>()?.TriggerCombat();
        }

    }
    
    
}
