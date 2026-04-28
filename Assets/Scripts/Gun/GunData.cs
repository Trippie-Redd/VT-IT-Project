using UnityEngine;
namespace Gun
{
    public enum FireMode
    {
        Single,
        SemiAuto,
        Burst,
        FullAuto
    }
    
    [CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
    public class GunData : ScriptableObject
    {
        public FireMode fireMode;
        public float fireRate; // bullets / second

        public float damage;
        public float range;

        public int maxMagAmmo;
        public int maxBackupAmmo;

        public AudioClip fireSound;
        public AudioClip reloadSound;
    }
}