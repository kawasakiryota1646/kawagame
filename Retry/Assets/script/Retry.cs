using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Retry : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private GameObject retryUI;     // リトライUI
    [SerializeField] private Text retryCountText;    // リトライ回数表示用
    [SerializeField] private string nextSceneName = "NextScene";
    [SerializeField] private float delayTime = 3f;
    [SerializeField] private AudioClip RetryBGM; // ゴール時に流すBGM
    private AudioSource audioSource;

    private bool isTriggered = false;
    private int retryCount;

    void Start()
    {
        // AudioSourceを追加
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = RetryBGM;
        audioSource.loop = false;


        // 保存済みのリトライ回数を取得
        retryCount = PlayerPrefs.GetInt("RetryCount", 0);

        // シーン内にテキストがあるなら自動取得
        if (retryCountText == null)
        {
            GameObject textObj = GameObject.Find("RetryCountText");
            if (textObj != null)
                retryCountText = textObj.GetComponent<Text>();
        }

        UpdateRetryUI();
    }

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

        // ゴールBGMを再生
        if (RetryBGM != null)
            audioSource.Play();


        Debug.Log("Retry Triggered");

        // 🔢 回数をカウント & 保存
        retryCount++;
        PlayerPrefs.SetInt("RetryCount", retryCount);
        PlayerPrefs.Save();

        UpdateRetryUI();

        // 待機してシーン移動
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(nextSceneName);
    }

    private void UpdateRetryUI()
    {
        if (retryCountText != null)
            retryCountText.text = "Retry: " + retryCount;
    }
}
