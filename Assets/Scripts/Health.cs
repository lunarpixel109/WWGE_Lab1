using Assets.Scripts.ScoreSystem;
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
    public bool isPlayer;
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    
    float _currentHealth;
    CharacterController _controller;
    
    private void Start() {
        _controller = GetComponent<CharacterController>();
        
        _currentHealth = maxHealth;
    }

    /// <summary>
    /// Reduces the health by the specified damage amount.
    /// </summary>
    /// <param name="damage">The amount of health to remove. <b>Should be a positive value</b></param>
    public void Damage(float damage) {
        _currentHealth -= damage;

        FindAnyObjectByType<BGMSelector>().TriggerCombat();

        if (_currentHealth <= 0) {
            Kill();
        }
    }

    /// <summary>
    /// Kills the object
    /// </summary>
    /// <remarks>
    /// If the object has the Player tag, it will respawn, if not it will destroy the object and add score if the object has one.
    /// </remarks>
    public void Kill() {
        if (gameObject.tag == "Player")
        {
            _controller.enabled = false;
            transform.position = respawnPosition;
            _controller.enabled = true;
            _currentHealth = maxHealth;
            GetComponent<ScoreManager>().ResetScore();
        }
        else
        {
            GetComponent<DeathScore>().TriggerScore();
            Destroy(gameObject);
        }
    }
}