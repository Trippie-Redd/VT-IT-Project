using Enemy;
using Player;
using TinyGoose.Tremble;
using UnityEngine;
using UnityEngine.SceneManagement;

[BrushEntity("exit", type:BrushType.Trigger)]
[RequireComponent(typeof(Collider))]
public class Exit : MonoBehaviour
{
    TargetsTracker _targetsTracker;
    bool _exitActivated;

    void Start()
    {
        _targetsTracker = FindFirstObjectByType<TargetsTracker>();
    }

    void Update()
    {
        if (_targetsTracker.AllTargetsDead && !_exitActivated)
        {
            // Display some ui/billboard thing
            _exitActivated = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!_exitActivated) return;

        SceneManager.LoadScene((int)Utils.SceneEnum.SuccessScreen);
    }
}