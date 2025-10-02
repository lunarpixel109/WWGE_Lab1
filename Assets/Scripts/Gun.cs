using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour {

    public Camera camera;

    public float rateOfFire;

    private bool allowedFire = true;

    private bool tryFire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)) {
            Fire();
        }

        // if (Input.GetKeyUp(KeyCode.Mouse0)) {
        //     tryFire = false;
        // }
    }

    void Fire() {
        if (allowedFire) {
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log("HIT");
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
}
