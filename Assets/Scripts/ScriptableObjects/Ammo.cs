using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "Ammo", menuName = "Gun/Ammo", order = 0)]
    public class Ammo : ScriptableObject {
        public string AmmoTitle; // Name of the ammo type
        public float ReloadTime; // Time taken to reload this ammo type
        public bool SpawnsGameObject; // Does this ammo spawn a game object (like a bullet) when fired
        public GameObject BulletPrefab; // The prefab to spawn when fired (if SpawnsGameObject is true)
        public float Power; // The power/force applied to the spawned bullet
        public AudioClip fireSound; // Sound played when the ammo is fired
    }
}