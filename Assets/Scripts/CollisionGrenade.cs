using System;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif



[RequireComponent(typeof(Rigidbody))]
public class CollisionGrenade : MonoBehaviour {
    
    public float radius;
    public float power;
    public float upwardsModifier;
    public float falloff;
    public AudioClip explosionSound;
    Rigidbody rb;
    AudioSource audioSource;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    private void Update() {
    }


    private void OnCollisionEnter(Collision other) {
        // Play explosion sound
        audioSource.PlayOneShot(explosionSound);
        // Disable the grenade's collider to prevent further collisions
        GetComponent<Collider>().enabled = false;
        // Destroy the grenade after the explosion sound has finished playing
        Destroy(gameObject, explosionSound.length);

        // Apply explosion effects
        var grenadeHit = Physics.OverlapSphere(transform.position, radius); // Get all colliders in the explosion radius
        foreach (var hit in grenadeHit) {
            Health health = hit.GetComponent<Health>(); // Get the Health component of the object we hit
            Rigidbody hitRb = hit.GetComponent<Rigidbody>(); // Get the Rigidbody component of the object we hit
            if (health != null) { // If the object has a Health component, apply damage
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                float damage = Mathf.Log(power, distance); // Calculate damage based on distance (logarithmic falloff)
                health.Damage(damage);
            }

            if (hitRb != null) {
                hitRb.AddForceAtPosition(-(transform.position - hit.transform.position) * power, transform.position); // Apply explosion force to the object we hit
            }

        }
    }

        
}
