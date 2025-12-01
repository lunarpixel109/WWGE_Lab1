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
                if (allowedPrimaryFire && _currentPrimaryAmmo > 0 && Time.timeScale > 0) {
                    TryFire();
                }
            }

            if (Input.GetKey(KeyCode.Mouse1) && Time.timeScale > 0) {
                TryAltFire();
            }

            if (Input.GetKey(KeyCode.R) && Time.timeScale > 0) {
                isReloading = true;
                _primaryReloadTimer = primaryAmmo.ReloadTime;
                _alternateReloadTimer = secondaryAmmo.ReloadTime;
            }

            text.text
                = $"{primaryAmmo.AmmoTitle}: {_currentPrimaryAmmo}/{primaryMagSize}\n{secondaryAmmo.AmmoTitle}: {_currentSecondaryAmmo}/{secondaryMagSize}";
        }
        // if (Input.GetKeyUp(KeyCode.Mouse0)) {
        //     tryFire = false;
        // }

        if (isReloading && _primaryReloadTimer > 0) {
            _primaryReloadTimer -= Time.deltaTime;
            allowedPrimaryFire = false;
            //ebug.Log("PRIM RELOAD: " + _primaryReloadTimer);
        } else if (isReloading && _primaryReloadTimer <= 0) {
            _currentPrimaryAmmo = primaryMagSize;
            allowedPrimaryFire = true;
        }
        
        if (isReloading && _alternateReloadTimer > 0) {
            _alternateReloadTimer -= Time.deltaTime;
            allowedSecondaryFire = false;
            //Debug.Log($"ALT RELOAD: {_alternateReloadTimer}");
        } else if (isReloading && _alternateReloadTimer <= 0) {
            _currentSecondaryAmmo = secondaryMagSize;
            allowedSecondaryFire = true;
        }

        if (_currentPrimaryAmmo == primaryMagSize && _currentSecondaryAmmo == secondaryMagSize) {
            isReloading = false;
        }
        
        if (allowedPrimaryFire == false && _primaryRateOfFireTimer > 0) {
            _primaryRateOfFireTimer -= Time.deltaTime;
        } else {
            allowedPrimaryFire = true;
        }
        
        if (allowedSecondaryFire == false && _secondaryRateOfFireTimer > 0) {
            _secondaryRateOfFireTimer -= Time.deltaTime;
        } else {
            allowedSecondaryFire = true;
        }
        
        //Debug.Log($"ALLOWED PRIM FIRE: {allowedPrimaryFire}");
        //Debug.Log($"ALLOWED SECOND FIRE: {allowedSecondaryFire}");
        
    }

    public void TryFire() {
        _currentPrimaryAmmo--;
        if (primaryAmmo.SpawnsGameObject) { // if it is a physical weapon
            var bullet = Instantiate(primaryAmmo.BulletPrefab, altSpawnPoint.transform);
            bullet.GetComponent<Rigidbody>().AddForce(altSpawnPoint.transform.forward * primaryAmmo.Power);
        } else { // if the weapon is hitscan
            Ray ray;
            if (isPlayer) {
                ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            } else {
                ray = new Ray(altSpawnPoint.transform.position, altSpawnPoint.transform.forward);
            }

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "MoveableObject") {
                    var direction = new Vector3(
                        hit.transform.position.x - transform.position.x,
                        hit.transform.position.y - transform.position.y,
                        hit.transform.position.z - transform.position.z
                    );
                    hit.rigidbody.AddForceAtPosition(Vector3.Normalize(direction) * primaryAmmo.Power, hit.point);
                }


                Health objHealth = hit.transform.GetComponent<Health>();
                if (objHealth) {
                    objHealth.Damage(primaryAmmo.Power);
                }

                var tempBullet = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                tempBullet.transform.parent = hit.transform;
                tempBullet.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
            }
        }

        allowedPrimaryFire = false;
        _primaryRateOfFireTimer = 1 / primaryFireRate;
        
    }
    
    public void TryAltFire() {
        if (allowedSecondaryFire && _currentSecondaryAmmo > 0) {
            _currentSecondaryAmmo--;
            if (secondaryAmmo.SpawnsGameObject) { // if it is a physical weapon
                var bullet = Instantiate(secondaryAmmo.BulletPrefab, altSpawnPoint.transform);
                bullet.GetComponent<Rigidbody>().AddForce(altSpawnPoint.transform.forward * secondaryAmmo.Power);
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

            allowedSecondaryFire = false;
            _secondaryRateOfFireTimer = 1 / secondaryFireRate;

        }
        
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(altSpawnPoint.transform.position, altSpawnPoint.transform.forward);
    }
    
    
}
