using UnityEngine;
using System.Collections;

public class SlidingFloorTrap : MonoBehaviour
{
    [Header("設定項目")]
    [SerializeField] private float slideDistance = 3f;      // 左方向に動く距離
    [SerializeField] private float slideSpeed = 2f;         // 動く速さ
    [SerializeField] private float delayBeforeSlide = 0.5f; // 動くまでの遅延
    [SerializeField] private float resetDelay = 3f;         // 元に戻るまでの時間
    [SerializeField] private bool resetAfterSlide = true;   // 戻すかどうか

    [Header("検知範囲設定")]
    [SerializeField] private float triggerRange = 3f;       // プレイヤーを検知する距離
    [SerializeField] private LayerMask playerLayer;         // Playerレイヤーを指定（Inspectorで）

    private bool isSliding = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.left * slideDistance;
    }

    void Update()
    {
        if (isSliding) return;

        // プレイヤー検知
        Collider2D hit = Physics2D.OverlapCircle(transform.position, triggerRange, playerLayer);
        if (hit != null)
        {
            StartCoroutine(SlideTrap());
        }
    }

    IEnumerator SlideTrap()
    {
        isSliding = true;
        yield return new WaitForSeconds(delayBeforeSlide);

        // 左へスライド
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        if (resetAfterSlide)
        {
            yield return new WaitForSeconds(resetDelay);

            // 元の位置に戻す
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * slideSpeed;
                transform.position = Vector3.Lerp(targetPosition, startPosition, t);
                yield return null;
            }
        }

        isSliding = false;
    }

    // Scene上で検知範囲を可視化（確認用）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
