using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        if (scoreTextMesh == null)
            Debug.LogError("scoreTextMesh is not assigned!");

        if (GameManager.Instance == null)
            Debug.LogError("GameManager.Instance is null!");

        if (scoreTextMesh != null && GameManager.Instance != null)
            scoreTextMesh.text = "FINAL SCORE: " + GameManager.Instance.GetTotalScore().ToString();

    scoreTextMesh.text = "FINAL SCORE: " + GameManager.Instance.GetTotalScore().ToString();
    }

}
