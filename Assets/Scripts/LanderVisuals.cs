using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticalSystem;
    [SerializeField] private ParticleSystem middleThrusterParticalSystem;
    [SerializeField] private ParticleSystem rightThrusterParticalSystem;
    [SerializeField] private GameObject landerExplosionVfx;

    private Lander lander;

    private void Awake()
    {
       lander = GetComponent<Lander>();

        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        SetEnabledThrusterParticleSystem(rightThrusterParticalSystem, false);
        SetEnabledThrusterParticleSystem(middleThrusterParticalSystem, false);
        SetEnabledThrusterParticleSystem(leftThrusterParticalSystem, false);
    }

    private void Start()
    {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.TooFastLanding:
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.WrongLandingArea:
                Instantiate(landerExplosionVfx, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;

        }
    }


    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(rightThrusterParticalSystem, false);
        SetEnabledThrusterParticleSystem(middleThrusterParticalSystem, false);
        SetEnabledThrusterParticleSystem(leftThrusterParticalSystem, false);
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(rightThrusterParticalSystem, true);
        SetEnabledThrusterParticleSystem(middleThrusterParticalSystem, false);
        SetEnabledThrusterParticleSystem(leftThrusterParticalSystem, false);
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(leftThrusterParticalSystem, true);
        SetEnabledThrusterParticleSystem(middleThrusterParticalSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticalSystem, false);
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(rightThrusterParticalSystem, true);
        SetEnabledThrusterParticleSystem(middleThrusterParticalSystem, true);
        SetEnabledThrusterParticleSystem(leftThrusterParticalSystem, true);
    }

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}