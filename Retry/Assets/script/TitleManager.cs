using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private Text deathCountText;
    [SerializeField] private Text retryCountText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;

    [Header("設定")]
    [SerializeField] private string gameSceneName = "GameScene"; // ゲーム本編のシーン名

    void Start()
    {
        // UI参照が設定されていなければ自動取得
        if (deathCountText == null)
            deathCountText = GameObject.Find("DeathCountText")?.GetComponent<Text>();
        if (retryCountText == null)
            retryCountText = GameObject.Find("RetryCountText")?.GetComponent<Text>();
        if (startButton == null)
            startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
        if (resetButton == null)
            resetButton = GameObject.Find("ResetButton")?.GetComponent<Button>();

        // ボタン設定
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButton);

        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButton);

        // 初期表示
        UpdateCountUI();
    }

    void UpdateCountUI()
    {
        int deathCount = PlayerPrefs.GetInt("DeathCount", 0);
        int retryCount = PlayerPrefs.GetInt("RetryCount", 0);

        if (deathCountText != null)
            deathCountText.text = "Death Count: " + deathCount;

        if (retryCountText != null)
            retryCountText.text = "Retry Count: " + retryCount;
    }

    void OnStartButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void OnResetButton()
    {
        // 🔁 どちらもリセット！
        PlayerPrefs.DeleteKey("DeathCount");
        PlayerPrefs.DeleteKey("RetryCount");
        PlayerPrefs.Save();

        UpdateCountUI();
        Debug.Log("DeathCount & RetryCount reset!");
    }
}
