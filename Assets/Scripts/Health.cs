using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Health : MonoBehaviour {
        
    [Header("Health Settings")]
    public float maxHealth;
    public Vector3 respawnPosition;

    [Header("HUD Settings")] 
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    
    float _currentHealth;
    CharacterController _controller;
    
    private void Start() {
        _controller = GetComponent<CharacterController>();
        
        _currentHealth = maxHealth;
        
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = maxHealth;
        healthText.text = $"{_currentHealth}/{maxHealth}";
    }

    public void Damage(float damage) {
        _currentHealth -= damage;
        healthText.text = $"{_currentHealth}/{maxHealth}";

        if (_currentHealth <= 0) {
            Kill();
        }
    }

    public void Kill() {
        _controller.enabled = false;
        transform.position = respawnPosition;
        _controller.enabled = true;
    }
}