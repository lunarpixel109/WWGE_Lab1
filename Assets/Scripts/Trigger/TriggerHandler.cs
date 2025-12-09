using System;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // Kill player if death tag :)
        if (other.CompareTag("Death")) {
            GetComponent<Health>().Kill();
        }
    }
}
