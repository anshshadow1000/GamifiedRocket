using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClip coinpickupAudioClip;
    [SerializeField] private AudioClip fuelpickupAudioClip;
    [SerializeField] private AudioClip crashAudioClip;
    [SerializeField] private AudioClip landingSuccessAudioClip;

    private void Start()
    {
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType) {
            case Lander.LandingType.Success:
                AudioSource.PlayClipAtPoint(landingSuccessAudioClip, Camera.main.transform.position);
        break;
                default:
                AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position);
                break;
        }
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinpickupAudioClip, Camera.main.transform.position);
    }

    private void Lander_OnFuelPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelpickupAudioClip, Camera.main.transform.position);
    }


}
