using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject goalUI;  // ゴールUIをInspectorで指定
    [SerializeField] private string nextSceneName = "NextScene"; // 次のシーン名
    [SerializeField] private float delayTime = 3f; // シーン移行までの待機時間
    [SerializeField] private AudioClip goalBGM; // ゴール時に流すBGM

    private bool isCleared = false;
    private AudioSource audioSource;

    void Start()
    {
        // AudioSourceを追加
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = goalBGM;
        audioSource.loop = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared) return; // 二重反応防止
        if (other.CompareTag("Player"))
        {
            isCleared = true;

            // プレイヤー操作を無効化
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
                playerController.enabled = false;

            StartCoroutine(GoalSequence());
        }
    }

    IEnumerator GoalSequence()
    {
        // ゴールUIを表示
        if (goalUI != null)
            goalUI.SetActive(true);

        // ゴールBGMを再生
        if (goalBGM != null)
            audioSource.Play();

        Debug.Log("Goal Reached!");

        // 数秒待機
        yield return new WaitForSeconds(delayTime);

        // 次のシーンへ移行
        SceneManager.LoadScene(nextSceneName);
    }
}
