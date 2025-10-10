using UnityEngine;
using System.Collections;

public class warp : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private Transform exitPipe;       // 出口の土管
    [SerializeField] private float warpDelay = 0.5f;   // 土管の中に沈んでる時間
    [SerializeField] private float sinkDistance = 1f;  // 土管に沈む距離
    [SerializeField] private float sinkSpeed = 2f;     // 沈むスピード
    [SerializeField] private float appearSpeed = 2f;   // 出るスピード
    [SerializeField] private float controlLockTime = 1.0f; // 出たあと操作不可時間
    [SerializeField] private KeyCode warpKey = KeyCode.S;

    private bool isPlayerInside = false;
    private bool isWarping = false;
    private Transform player;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerRenderer;
    private PlayerController playerController; // ← プレイヤー操作スクリプトへの参照

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            player = other.transform;
            playerRb = player.GetComponent<Rigidbody2D>();
            playerRenderer = player.GetComponent<SpriteRenderer>();
            playerController = player.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void Update()
    {
        if (isPlayerInside && !isWarping && Input.GetKeyDown(warpKey))
        {
            StartCoroutine(WarpAnimation());
        }
    }

    private IEnumerator WarpAnimation()
    {
        if (exitPipe == null || player == null) yield break;
        isWarping = true;

        // プレイヤーの動きを停止
        if (playerController) playerController.enabled = false; // ← 入力を止める

        Vector3 startPos = player.position;
        Vector3 targetPos = startPos + Vector3.down * sinkDistance;

        // 土管に沈む
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * sinkSpeed;
            player.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // 土管内で消える
        if (playerRenderer) playerRenderer.enabled = false;

        yield return new WaitForSeconds(warpDelay);

        // 出口へワープ
        player.position = exitPipe.position;

        // 出口下から上へ出る演出
        Vector3 appearStart = exitPipe.position + Vector3.down * sinkDistance;
        Vector3 appearEnd = exitPipe.position;
        player.position = appearStart;

        if (playerRenderer) playerRenderer.enabled = true;

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * appearSpeed;
            player.position = Vector3.Lerp(appearStart, appearEnd, t);
            yield return null;
        }

        // 出た後も少し操作不能
        yield return new WaitForSeconds(controlLockTime);

        // 動きを再開
        if (playerRb) playerRb.simulated = true;
        if (playerController) playerController.enabled = true;

        isWarping = false;
    }
}
