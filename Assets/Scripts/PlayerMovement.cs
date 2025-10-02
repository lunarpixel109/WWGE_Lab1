using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
class PlayerMovement : MonoBehaviour {

    CharacterController controller;
    public float speed = 5f;

    private Vector3 velocity;
    public float gravity = -9.8f;
    
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    private void Start() {
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        
        float x =  Input.GetAxis("Horizontal");
        float z =  Input.GetAxis("Vertical");
        Vector3 movementInput = transform.right * x + transform.forward * z;
        controller.Move(movementInput * speed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = 5;
        }
    }
    
    
}