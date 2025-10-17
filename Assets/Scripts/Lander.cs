using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            Debug.Log("Up");
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            Debug.Log("Down");
        }
    }
}
