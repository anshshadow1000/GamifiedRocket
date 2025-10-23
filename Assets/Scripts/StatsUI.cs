using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private Image fuelImage;


    private void Update()
    {
        UpdateStatsTextMesh();
    }

    private void UpdateStatsTextMesh()
    {
        speedDownArrowGameObject.SetActive(Lander.Instance.GetSpeedY() < 0);
        speedUpArrowGameObject.SetActive(Lander.Instance.GetSpeedY() >= 0);
        speedLeftArrowGameObject.SetActive(Lander.Instance.GetSpeedX() < 0);
        speedRightArrowGameObject.SetActive(Lander.Instance.GetSpeedX() >= 0);

        fuelImage.fillAmount = Lander.Instance.GetFuelAmountNormalized();

        statsTextMesh.text = GameManager.Instance.GetScore() + "\n" +
        Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
        Mathf.Round(Mathf.Abs(Lander.Instance.GetSpeedX() * 10f)) + "\n" +
        Mathf.Round(Mathf.Abs(Lander.Instance.GetSpeedY() * 10f));
    }


}
