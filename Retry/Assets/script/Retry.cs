using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Retry : MonoBehaviour
{
    [SerializeField] private GameObject retryUI;   // リトライUI
    [SerializeField] private string nextSceneName = "NextScene"; // 次のシーン名
    [SerializeField] private float delayTime = 3f; // シーン移行までの待機時間

    private bool isTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return; // 二重反応防止

        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(RetrySequence(other.gameObject));
        }
    }

    IEnumerator RetrySequence(GameObject player)
    {
        // プレイヤー操作を停止
        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // UIを表示
        if (retryUI != null)
            retryUI.SetActive(true);

        Debug.Log("Retry Triggered");

        // 指定時間待機
        yield return new WaitForSeconds(delayTime);

        // シーンをリロード or 移行
        SceneManager.LoadScene(nextSceneName);
    }
}
