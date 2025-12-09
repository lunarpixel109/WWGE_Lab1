using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ScoreSystem {
    public class DeathScore : MonoBehaviour {

        public float score;

        public void TriggerScore() {
            FindAnyObjectByType<ScoreManager>().AddScore(score); // Get the score manager and add score to the player
        }
    }
}