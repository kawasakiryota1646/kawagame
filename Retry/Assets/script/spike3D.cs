using UnityEngine;
using System.Collections;
public class spike3D : MonoBehaviour
{
    [Header("動き設定")]
    [SerializeField] private float slideDistance = 2f;   // 飛び出す距離
    [SerializeField] private float slideSpeed = 5f;      // 飛び出す速さ
    [SerializeField] private float retractDelay = 1.5f;  // 引っ込むまでの待機時間
    [SerializeField] private bool repeat = true;         // 繰り返すか

    [Header("検知設定")]
    [SerializeField] private float detectRange = 3f;     // 検知範囲
    [SerializeField] private Transform player;           // プレイヤー参照（自動でもOK）
    [SerializeField] private LayerMask playerLayer;      // プレイヤーのレイヤー（任意）

    [Header("方向設定")]
    [SerializeField] private Vector2 attackDirection = Vector2.up; // どの方向に飛び出すか

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isAttacking = false;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + (Vector3)(attackDirection.normalized * slideDistance);

        // シーン内のPlayerを自動で探す
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (isAttacking || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectRange)
        {
            StartCoroutine(SpikeAttack());
        }
    }

    IEnumerator SpikeAttack()
    {
        isAttacking = true;

        // 飛び出す動作
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(retractDelay);

        // 元に戻る
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            transform.position = Vector3.Lerp(endPos, startPos, t);
            yield return null;
        }

        isAttacking = false;

        if (!repeat)
            enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(attackDirection.normalized * slideDistance));
    }
}
