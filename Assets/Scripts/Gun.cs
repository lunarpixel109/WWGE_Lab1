using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour {

    public Camera camera;

    [Header("Ammo Settings")]
    [Tooltip("The rate of fire in rps")] public float rateOfFire;
    [Tooltip("The size of the mag")] public int maxAmmo = 10;
    [Tooltip("The time it takes to reload")] public float reloadTime;
    [Tooltip("The force added to the weapon")]  public float force;
    
    [Header("General Gun Settings")]
    public TextMeshProUGUI text;

    public GameObject bulletHole;
    
    private bool allowedFire = true;

    private bool tryFire;
    
    private int ammo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammo = maxAmmo;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)) {
            Fire();
        }

        if (Input.GetKey(KeyCode.Mouse1)) {
            AltFire();
        }
        
        if (Input.GetKey(KeyCode.R)) {
            StartCoroutine(Reload());
        }
        
        text.text = $"{ammo}/{maxAmmo}";

        // if (Input.GetKeyUp(KeyCode.Mouse0)) {
        //     tryFire = false;
        // }
    }
    
    void Fire() {
        if (allowedFire && ammo > 0) {
            ammo--;
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && Time.timeScale == 1f) {
                if (hit.rigidbody) {
                    var direction = new Vector3(
                        hit.transform.position.x - transform.position.x, 
                        hit.transform.position.y - transform.position.y, 
                        hit.transform.position.z - transform.position.z
                        );
                    
                    hit.rigidbody.AddForceAtPosition(Vector3.Normalize(direction) * force, hit.point);
                    
                    var tempBullet = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                    tempBullet.transform.parent = hit.transform;
                    tempBullet.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));

                } else {
                    var tempBullet = Instantiate(bulletHole,  hit.point, Quaternion.LookRotation(hit.normal));
                    tempBullet.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
                }
                
                
            } else {
                Debug.Log("MISS");
            }
            StartCoroutine(FireTimer());
        }
    }
    
    void AltFire() {
        if (allowedFire && ammo > 0) {
            ammo--;
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && Time.timeScale == 1f) {
                if (hit.rigidbody) {
                    var direction = new Vector3(
                        hit.transform.position.x - transform.position.x, 
                        hit.transform.position.y - transform.position.y, 
                        hit.transform.position.z - transform.position.z
                    );
                    
                    hit.rigidbody.AddExplosionForce(force, hit.point, 3f);
                    
                    var tempBullet = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                    tempBullet.transform.parent = hit.transform;
                    tempBullet.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));

                } else {
                    var tempBullet = Instantiate(bulletHole,  hit.point, Quaternion.LookRotation(hit.normal));
                    tempBullet.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
                }
                
                
            } else {
                Debug.Log("MISS");
            }
            StartCoroutine(FireTimer());
        }
    }

    IEnumerator FireTimer() {
        allowedFire = false;
        yield return new WaitForSeconds(1/rateOfFire);
        allowedFire = true;
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
    }
}
