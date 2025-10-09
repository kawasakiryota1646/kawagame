using UnityEngine;
using System.Collections;

public class FlipTrap : MonoBehaviour
{
    [Header("設定項目")]
    [SerializeField] private float flipDelay = 0.5f;      // プレイヤーが乗ってから反転するまでの時間
    [SerializeField] private float flipSpeed = 5f;        // 回転スピード
    [SerializeField] private float resetDelay = 3f;       // 元に戻るまでの時間
    [SerializeField] private bool resetAfterFlip = true;  // 元に戻るかどうか

    private bool isFlipping = false;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    void Start()
    {
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0f, 180f); // 180度反転
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFlipping && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FlipAfterDelay());
        }
    }

    IEnumerator FlipAfterDelay()
    {
        isFlipping = true;
        yield return new WaitForSeconds(flipDelay);

        // 床を回転させる
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * flipSpeed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        // 床の当たり判定をOFF
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        if (resetAfterFlip)
        {
            yield return new WaitForSeconds(resetDelay);
            // 元の状態に戻す
            transform.rotation = startRotation;
            if (col) col.enabled = true;
        }

        isFlipping = false;
    }
}
