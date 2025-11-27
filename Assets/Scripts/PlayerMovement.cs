using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5;   
    void Update()
    {

        if (Keyboard.current.wKey.isPressed)
        {
            transform.Translate(Vector3.left * Time.deltaTime * playerSpeed, Space.World);
        }
    }
    
}
