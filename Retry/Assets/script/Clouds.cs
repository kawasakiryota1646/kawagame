using UnityEngine;
using System.Collections;

public class CloudTrap_UnderTrigger : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    private Vector3 startPosition;  // 元の位置を保存
    private SpriteRenderer sr;      // 雲の見た目を消す用

    [Header("落下設定")]
    [SerializeField] private float delayBeforeFall = 0.5f; // 反応してから落ちるまで
    [SerializeField] private float gravityAfterFall = 3f;  // 落下スピード
    [SerializeField] private float triggerRangeX = 1.5f;   // 横方向の反応範囲
    [SerializeField] private float triggerRangeY = 3f;     // 下方向の反応範囲

    [Header("リスポーン設定")]
    [SerializeField] private float disappearDelay = 1f;    // 当たってから消えるまでの時間
    [SerializeField] private float respawnDelay = 3f;      // 消えてから戻るまでの時間

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        startPosition = transform.position;
    }

    void Update()
    {
        if (hasFallen) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 playerPos = player.transform.position;
        Vector2 cloudPos = transform.position;

        bool isUnder = playerPos.y < cloudPos.y &&
                       Mathf.Abs(playerPos.x - cloudPos.x) < triggerRangeX &&
                       (cloudPos.y - playerPos.y) < triggerRangeY;

        if (isUnder)
        {
            hasFallen = true;
            Invoke(nameof(FallDown), delayBeforeFall);
        }
    }

    void FallDown()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityAfterFall;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            // 一定時間後に消えて、リスポーン処理を開始
            StartCoroutine(DisappearThenRespawn());
        }
    }

    IEnumerator DisappearThenRespawn()
    {
        // 一定時間待ってから雲を非表示にする
        yield return new WaitForSeconds(disappearDelay);

        sr.enabled = false;           // 雲を消す
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // 消えたあと、さらに一定時間待ってから元の位置に戻す
        yield return new WaitForSeconds(respawnDelay);

        transform.position = startPosition;
        sr.enabled = true;            // 再表示
        GetComponent<Collider2D>().enabled = true;

        hasFallen = false;
    }
}
