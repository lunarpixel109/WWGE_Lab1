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
    
    private float fadeOutTimer = 0f;
    private float fadeInTimer = 0f;
    
    private bool isTransitioningOut = false;
    private bool isTransitioningIn  = true;
    private bool isTransitioning    = true;

    private AudioClip newClip;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTransitioning) {
            if (isTransitioningOut) {
                
            }
        }
    }

    public void SwitchCombatStates() {
        isInCombat = !isInCombat;
    }
    
    
    IEnumerator
}
