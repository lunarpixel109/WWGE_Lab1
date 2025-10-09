using System;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    
    CharacterController controller;

    private void Start() {
        controller = GetComponent<CharacterController>();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Death")) {
            var health = GetComponent<Health>();
            health.Kill();
        }
    }
}
