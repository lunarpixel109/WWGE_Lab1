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
        public Button quitButton;
        [Space]
        public GameObject pauseMenu;
        
        private void Start() {
            isPaused = true;
            
            resumeButton.onClick.AddListener(TogglePause);
            quitButton.onClick.AddListener(Quit);

            TogglePause();
        }


        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) { // if escape key pressed, pause the app
                TogglePause();
            }

            if (isPaused) { // If we are paused, update the fov based on the slider
                camera.fieldOfView = fovSlider.value;
            }
        }


        /// <summary>
        /// Toggles the pause state, triggering the menue and handling the mouse lock
        /// </summary>
        void TogglePause() {
            
            if (isPaused) {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                pauseMenu.SetActive(false);
            } else {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                fovSlider.value = camera.fieldOfView;
                pauseMenu.SetActive(true);
            }
            
            isPaused = !isPaused;
        }

        void Quit() {
            Application.Quit();


#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}