using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class CollisionGrenade : MonoBehaviour {
    
    public float radius;
    public float power;
    public float upwardsModifier;
    // TODO: Implement Custom Explosions
    
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        var grenadeHit = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in grenadeHit) {
            Health health = hit.GetComponent<Health>();
            Rigidbody hitRb = hit.GetComponent<Rigidbody>();
            // if (health != null) {
            //     health.Damage(power * Vector3.Distance(transform.position, hit.transform.position));
            // }

            if (hitRb != null) {
                hitRb.AddExplosionForce(power, transform.position, radius, upwardsModifier);
            }
        }
        Destroy(gameObject);
    }
}
