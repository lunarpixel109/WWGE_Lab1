using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Scripts.Audio {
    public class AudioManager : MonoBehaviour {

        [Header("Intial Volumes")]
        [Range(-80, 20)] public float initialMasterVolume;
        [Range(-80, 20)] public float initialMusicVolume;
        [Range(-80, 20)] public float initialSfxVolume;

        [Header("Volume Mixer Settings")]
        public AudioMixer mixer;

        [Header("UI Settings")]
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;

        private void Start() {
            mixer.SetFloat("MasterVolume", initialMasterVolume);
            mixer.SetFloat("MusicVolume", initialMusicVolume);
            mixer.SetFloat("SFXVolume", initialSfxVolume);
        }

        private void Update() {
            // If the game is paused, update the volume levels
            if (Time.timeScale == 0) {
                mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
                mixer.SetFloat("MusicVolume", musicVolumeSlider.value);
                mixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
            }
        }


    }
}