using UnityEngine;
using System.Collections;

public class walltrap : MonoBehaviour
{
    [Header("倒れる設定")]
    [SerializeField] private float rotationAngle = 90f; // 倒れる角度
    [SerializeField] private float fallSpeed = 50f;     // 回転スピード
    [SerializeField] private float delayBeforeFall = 1f; // 反応までの遅延
    [SerializeField] private float triggerRange = 3f;    // プレイヤーが近づく距離

    [Header("潰された時の設定")]
    [SerializeField] private GameObject gameOverUI;      // GameOverのUI
    [SerializeField] private float delayBeforeRestart = 2f; // シーンリセットまでの時間

    private bool isFalling = false;
    private Quaternion startRot;
    private Quaternion targetRot;
    private AudioSource audioSource;

    void Start()
    {
        startRot = transform.rotation;
        targetRot = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotationAngle));
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // プレイヤーが近づいたら倒れる
        if (!isFalling && distance < triggerRange)
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    IEnumerator FallAfterDelay()
    {
        isFalling = true;
        yield return new WaitForSeconds(delayBeforeFall);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * (fallSpeed / 100f);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }
    }


}
