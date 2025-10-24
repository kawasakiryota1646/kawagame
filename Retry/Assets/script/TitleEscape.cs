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

        // Rキーでリトライ開始
        if (Input.GetKeyDown(KeyCode.Backspace) && !isRetrying)
        {
            StartCoroutine(RetrySequence());
        }
    }

    IEnumerator RetrySequence()
    {
        isRetrying = true;

        // UIを表示
        if (retryUI != null)
        {
            retryUI.SetActive(true);
        }

        Debug.Log("Retry Triggered");

        // 指定時間待機
        yield return new WaitForSeconds(delayTime);

        // 次のシーンに移行（今のシーンをリロードしたい場合はコメントを切り替えてね）
        SceneManager.LoadScene(nextSceneName);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        isRetrying = false;
    }
}
