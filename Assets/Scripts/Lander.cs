using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    private const float GRAVITY_NORMAL = 0.7f;

    public static Lander Instance { get; private set; }

    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType
    {
        Success, 
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    public enum State
    {
        WaitingToStart,
        Normal,
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private State state;



    private void Awake() {
        Instance = this;
        fuelAmount = fuelAmountMax;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
        state = State.WaitingToStart;
    }


    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            case State.WaitingToStart:
                if (Keyboard.current.upArrowKey.isPressed ||
                   Keyboard.current.leftArrowKey.isPressed ||
                   Keyboard.current.rightArrowKey.isPressed)
                {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    state = State.Normal;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                }
                break;
            case State.Normal:
                if (fuelAmount <= 0f)
                {
                    return;
                }
                if (Keyboard.current.upArrowKey.isPressed ||
                   Keyboard.current.leftArrowKey.isPressed ||
                   Keyboard.current.rightArrowKey.isPressed)
                {
                    ConsumeFuel();
                }
                if (Keyboard.current.upArrowKey.isPressed)
                {
                    float force = 700f;
                    landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }
                if (Keyboard.current.leftArrowKey.isPressed)
                {
                    float turnspeed = +100f;
                    landerRigidbody2D.AddTorque(turnspeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }
                if (Keyboard.current.rightArrowKey.isPressed)
                {
                    float turnspeed = -100f;
                    landerRigidbody2D.AddTorque(turnspeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingpad)) {
            Debug.Log("Crashed on the Terrain");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                score = 0,
            });
            return;
        }
            float softlandingvelocitymagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
            if (relativeVelocityMagnitude > softlandingvelocitymagnitude)
            {
                Debug.Log("Landed too Hard!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            return;
        }

            float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.9f;
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed on a too steep angle!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
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

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingpad.GetScoreMultiplier()) ;

        Debug.Log("score: " + score);
        OnLanded?.Invoke(this, new OnLandedEventArgs { 
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingpad.GetScoreMultiplier(),
            score = score,
        });
    }

   
       private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }

    public float GetFuelAmount()
    {
        return fuelAmount;
    }

    public float GetSpeedX()
    {
        return landerRigidbody2D.linearVelocity.x;
    }

    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocity.x;
    }
}

