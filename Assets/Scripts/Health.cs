using System;
using UnityEngine;

public class Health : MonoBehaviour {
        
    public float health;
    float currentHealth;

    public Vector3 respawnPosition;

    CharacterController controller;
    
    private void Start() {
        controller = GetComponent<CharacterController>();
    }

    public void Damage(float damage) {
        health -= damage;

        if (health <= 0) {
            Kill();
        }
    }

    public void Kill() {
        controller.enabled = false;
        transform.position = respawnPosition;
        controller.enabled = true;
    }
}