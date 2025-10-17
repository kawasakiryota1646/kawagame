using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("UI�Q��")]
    [SerializeField] private Text deathCountText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;

    [Header("�ݒ�")]
    [SerializeField] private string gameSceneName = "GameScene"; // �Q�[���{�҂̃V�[����

    void Start()
    {
        // UI�Q�Ƃ��ݒ肳��Ă��Ȃ���Ύ����擾
        if (deathCountText == null)
            deathCountText = GameObject.Find("DeathCountText")?.GetComponent<Text>();
        if (startButton == null)
            startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
        if (resetButton == null)
            resetButton = GameObject.Find("ResetButton")?.GetComponent<Button>();

        // �{�^���ݒ�
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButton);

        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButton);

        // �����\��
        UpdateDeathCountUI();
    }

    void UpdateDeathCountUI()
    {
        int count = PlayerPrefs.GetInt("DeathCount", 0);
        if (deathCountText != null)
            deathCountText.text = "Death Count: " + count;
    }

    void OnStartButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void OnResetButton()
    {
        PlayerPrefs.DeleteKey("DeathCount");
        PlayerPrefs.Save();
        UpdateDeathCountUI();
        Debug.Log("Death count reset!");
    }
}
