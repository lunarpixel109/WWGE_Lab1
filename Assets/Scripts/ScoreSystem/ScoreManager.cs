using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    [Header("Combo Settings")] 
    [Tooltip("The amount of actions in a row needed to combo")] public int actionsForCombo;
    [Tooltip("The initial modifier when a combo starts")] public float scoreComboInitialModifier;
    [Tooltip("The amount the modifier increases by after each action in the combo")] public float scoreComboModifierIncrement;
    [Tooltip("The time without action it takes for the combo to run out (also the time between actions for the start of the combo)")]
    public float comboTime;

    [Header("Score Settings")]
    [Tooltip("The minimum amount of score that an action can give")] public int minScore;
    [Tooltip("The maximum amount of score that an action can give")] public int maxScore;
    
    private int   score;
    private float modifier;

    private void Start() {
        score = 0;
    }
}
