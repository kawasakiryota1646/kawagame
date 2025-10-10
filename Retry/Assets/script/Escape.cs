using UnityEngine;

public class Escape : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private Transform player;       // プレイヤーのTransform
    [SerializeField] private Transform escapePoint;  // 逃げ先の地点
    [SerializeField] private float detectRange = 5f; // 逃げ出す距離
    [SerializeField] private float moveSpeed = 3f;   // 逃げる速さ
    [SerializeField] private float stopDistance = 0.1f; // 逃げ先で止まる距離

    private bool isEscaping = false;
    private bool hasEscaped = false;

    void Update()
    {
        if (player == null || escapePoint == null) return;

        // すでに逃げ切ってたら何もしない
        if (hasEscaped) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // プレイヤーが近づいたら逃げる
        if (!isEscaping && distance < detectRange)
        {
            isEscaping = true;
        }

        // 逃げ中
        if (isEscaping)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                escapePoint.position,
                moveSpeed * Time.deltaTime
            );

            // 逃げ切ったら終了
            if (Vector2.Distance(transform.position, escapePoint.position) <= stopDistance)
            {
                isEscaping = false;
                hasEscaped = true;
            }
        }
    }

    // エディタで範囲確認
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (escapePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, escapePoint.position);
        }
    }
}