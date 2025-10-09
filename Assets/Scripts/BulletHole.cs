using System;
using UnityEngine;

public class BulletHole : MonoBehaviour {

    public float bulletTimer = 4;

    private float currentTimer;

    private void Start() {
        currentTimer = bulletTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimer > 0) {
            currentTimer -= Time.deltaTime;
        } else {
            Destroy(gameObject);
        }
    }
}
