using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "Ammo", menuName = "Gun/Ammo", order = 0)]
    public class Ammo : ScriptableObject {
        public string AmmoTitle;
        public float ReloadTime;
        public bool SpawnsGameObject;
        public GameObject BulletPrefab;
        public float Power;
    }
}