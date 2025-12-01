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
    // TODO: Implement Custom Explosions
    public float falloff;
    
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();

    }

    private void OnCollisionEnter(Collision other) {
        var grenadeHit = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in grenadeHit) {
           Health health = hit.GetComponent<Health>();
            Rigidbody hitRb = hit.GetComponent<Rigidbody>();
            if (health != null) {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                float damage = Mathf.Log(power, distance);
                health.Damage(damage);
            }

            if (hitRb != null) {
                hitRb.AddForceAtPosition(-(transform.position - hit.transform.position) * power, transform.position);
            }
            
        }
        Destroy(gameObject);
    }
}
