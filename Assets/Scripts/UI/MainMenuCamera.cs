using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuCamera : MonoBehaviour
{
    public float strength = 0.3f;
    public float smoothing = 5f;
    
    private Vector3 startPos;

    void OnEnable()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        var mouse = new Vector2(
            (mousePos.x / Screen.width  - 0.5f) * 2f,
            (mousePos.y / Screen.height - 0.5f) * 2f
        );

        Vector3 targetPos = startPos 
           + transform.right * (mouse.x * strength) 
           + transform.up    * (mouse.y * strength);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothing);

        Debug.Log(transform.position);
    }   
}