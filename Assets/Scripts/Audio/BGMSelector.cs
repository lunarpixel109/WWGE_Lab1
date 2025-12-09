using System.Collections.Generic;
using UnityEngine;

public class BGMSelector : MonoBehaviour {

    public bool isInCombat = false;
    
    [Header("Source Settings")]
    public AudioSource audioSource;
    [Range(0, 1)] public float initialVolume;
    
    
    [Header("Audio Clips")]
    public List<AudioClip> battleClips;
    public List<AudioClip> calmClips;

    
    [Header("Fading Settings")]
    public float fadeInTime = 0.2f;
    public float fadeOutTime = 0.2f;


    [Header("Combat Settings")] 
    public float combatTimeout = 3f;
    
    private float fadeOutTimer = 0f;
    private float fadeInTimer = 0f;
    private float combatTimer = 0f;
    
    private bool isTransitioningOut = false;
    private bool isTransitioningIn  = false;
    private bool isTransitioning    = false;
    private bool isStopping         = false;

    private AudioClip newClip;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = initialVolume;
        audioSource.clip = calmClips[Random.Range(0, calmClips.Count)];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update() {
        if (isTransitioning) { // If we marked as transitioning, handle fading
            if (isTransitioningOut) { // Fadinsg out
                audioSource.volume = Mathf.Lerp(0, initialVolume, fadeOutTimer / fadeOutTime); // Lerps from the initial volume to 0 (as fadeOutTimer goes from fadeOutTime to 0, which means the lerp is backwards)
                if (fadeOutTimer <= 0) {
                    // Once we have finished fading out, set the appropiate flags, change the clips then set the fade in timer
                    isTransitioningOut = false;
                    isTransitioningIn = true;
                    audioSource.Stop();
                    audioSource.clip = isInCombat ? battleClips[Random.Range(0, battleClips.Count)] : calmClips[Random.Range(0, calmClips.Count)];
                    audioSource.Play();
                    fadeInTimer = fadeInTime;
                }

                if (isStopping && fadeOutTimer <= 0) {
                    // If we are stopping the music, once we finish fading out, stop the music and set volume to 0
                    audioSource.Stop();
                    audioSource.volume = 0;
                    isTransitioning = false;
                    isStopping = false;
                }
                
                fadeOutTimer -= Time.deltaTime; // Decrease the fade out timer
            }

            if (isTransitioningIn && !isStopping) { // Fading in 
                audioSource.volume = Mathf.Lerp(initialVolume, 0, fadeInTimer / fadeInTime); // Lerps from 0 to the initial volume (as fadeInTimer goes from fadeInTime to 0, which means the lerp is backwards)
                if (fadeInTimer <= 0) {
                    // We are now done transitioning
                    isTransitioningIn = false;
                    isTransitioning = false;
                }

                fadeInTimer -= Time.deltaTime; // Decrease the fade in timer
            }
        }

        if (isInCombat) {
            if (combatTimer <= 0) {
                // If the combat timer has run out, we exit combat
                isInCombat = false;
                isTransitioningOut = true;
                isTransitioning = true;
                fadeOutTimer = fadeOutTime;
            } else {
                combatTimer -= Time.deltaTime; // Decrease the combat timer
            }
        }
    }

    /// <summary>
    /// Used to trigger the combat music, if the music is already in combat mode, it just resets the combat timer.
    /// </summary>
    public void TriggerCombat() {
        if (isInCombat) {
            combatTimer = combatTimeout;
        } else { 
            isInCombat = true;
            isTransitioningOut = true;
            isTransitioning = true;
            fadeOutTimer = fadeOutTime;
            combatTimer = combatTimeout;
        }
    }
    
    /// <summary>
    /// Fades the music out and stops it
    /// </summary>
    public void StopMusic() {
        isStopping = true;
        isTransitioningOut = true;
        isTransitioning = true;
        fadeOutTimer = fadeOutTime;
    }
}
