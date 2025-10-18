using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    
    private Rigidbody2D landerRigidbody2D;



    private void Awake() {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnspeed = +100f;
            landerRigidbody2D.AddTorque(turnspeed * Time.deltaTime);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnspeed = -100f;
            landerRigidbody2D.AddTorque(turnspeed * Time.deltaTime);
        }
    }
}

