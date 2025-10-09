using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class GameManager : MonoBehaviour {
        
        private bool isPaused = false;

        public Camera camera;
        
        [Header("UI Elements")] 
        public Slider fovSlider;
        public Button resumeButton;
        public GameObject pauseMenu;
        
        private void Start() {
            isPaused = false;
            
            resumeButton.onClick.AddListener(TogglePause);
        }


        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                TogglePause();
            }

            if (isPaused) {
                camera.fieldOfView = fovSlider.value;
            }
        }

        void TogglePause() {
            
            if (isPaused) {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                pauseMenu.SetActive(false);
            } else {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                fovSlider.value = camera.fieldOfView;
                pauseMenu.SetActive(true);
            }
            
            isPaused = !isPaused;
        }
    }
}