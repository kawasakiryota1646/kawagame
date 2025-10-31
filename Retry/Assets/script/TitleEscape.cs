using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleEscape : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "stageselect";
    [SerializeField] private GameObject retryUI;   // リトライUI
    [SerializeField] private string nextSceneName = "NextScene"; // 次のシーン名
    [SerializeField] private float delayTime = 3f; // シーン移行までの待機時間

    private bool isRetrying = false; // 連打防止用

    void Update()
    {
        // ESCでタイトルへ戻る
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.CanStreamedLevelBeLoaded(titleSceneName))
            {
                SceneManager.LoadScene(titleSceneName);
            }
            else
            {
                Debug.LogError($"シーン '{titleSceneName}' が Build Settings に登録されていません！");
            }
        }

        // Backspaceキーでリトライ開始
        if (Input.GetKeyDown(KeyCode.Backspace) && !isRetrying)
        {
            StartCoroutine(RetrySequence());
        }
    }

    IEnumerator RetrySequence()
    {
        isRetrying = true;

        // 💾 リトライ回数カウント
        int retryCount = PlayerPrefs.GetInt("RetryCount", 0);
        retryCount++;
        PlayerPrefs.SetInt("RetryCount", retryCount);
        PlayerPrefs.Save();

        Debug.Log($"Retry Triggered! Retry Count: {retryCount}");

        // UIを表示
        if (retryUI != null)
        {
            retryUI.SetActive(true);
        }

        // 指定時間待機
        yield return new WaitForSeconds(delayTime);

        // 🔁 シーンをリロード（または指定のシーンへ）
        SceneManager.LoadScene(nextSceneName);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        isRetrying = false;
    }
}
