using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public float score;

    [Header("Combo Settings")]
    public float maxComboMultiplier = 1f;
    public float comboDecayRate = 0.05f; // multiplier decrease per second
    public float comboIncreaseRate = 0.1f; // multiplier increase per successful action

    private float currentComboMultiplier = 1f;

    [Header("UI Settings")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;


    private void Start() {
        InvokeRepeating(nameof(DecayCombo), 1f, 1f);
    }

    private void Update() {
        // Update the score display every frame, formatted to be six digits with leading zeros
        scoreText.text = $"Score: {score.ToString("N6")}";
        comboText.text = $"{currentComboMultiplier:N2}x";


    }


    public void DecayCombo() {
        currentComboMultiplier -= comboDecayRate;
    }

    /// <summary>
    /// Adds the specified amount to the current score and updates the score display
    /// </summary>
    /// <param name="amount">The value to add to the current score</param>
    public void AddScore(float amount) {
        score += amount * currentComboMultiplier;
        currentComboMultiplier += comboIncreaseRate;
        scoreText.text = "Score: " + score;
    }


    /// <summary>
    /// Resets the score to zero
    /// </summary>
    public void ResetScore() {
        score = 0;
        currentComboMultiplier = 1f;
        scoreText.text = "Score: " + score;
    }

}
