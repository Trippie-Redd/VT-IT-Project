using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public enum FireMode
    {
        Single,
        SemiAuto,
        Burst,
        FullAuto
    }

    public FireMode fireMode;
    public float fireRate; // bullets / second

    public float damage;
    public float range;
}
