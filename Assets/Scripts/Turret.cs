using System;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    
    Vector3 targetDirection;
    GameObject player;
    public float speed;
    public float maxFollowDotAngle;
    public float maxFollowDistance;

    public Ammo turretAmmo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            throw new NullReferenceException("Player not found");
        }
    }

    // Update is called once per frame
    void Update() {
        if (IsPlayerVisible()) {
           // Debug.Log("Player is visible");
            targetDirection = player.transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    private bool IsPlayerVisible() {
        float dot = Vector3.Dot(player.transform.position, (player.transform.position - transform.position).normalized);

        if (dot > maxFollowDotAngle && Vector3.Distance(player.transform.position, transform.position) > maxFollowDistance)
            return true;
        else
            return false;
    }

    
    
    
    private void OnDrawGizmos() {
        //Gizmos.color = Color.red;
        //Gizmos.DrawFrustum(transform.position , maxFollowAngle, 0, maxFollowDistance, 1);
    }
}
