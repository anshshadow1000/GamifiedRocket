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

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingpad)) {
            Debug.Log("Crashed on the Terrain");
            return;
        }
            float softlandingvelocitymagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
            if (relativeVelocityMagnitude > softlandingvelocitymagnitude)
            {
                Debug.Log("Landed too Hard!");
            return;
            }

            float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.9f;
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed on a too steep angle!");
            return;
        }
        Debug.Log("Successful landing!");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;
        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softlandingvelocitymagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("landingAngleScore: " + landingAngleScore);
        Debug.Log("landingSpeedScore: " + landingSpeedScore);
    }
}

