using UnityEngine;

public class HiddenBlock : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private SpriteRenderer spriteRenderer; // 見た目
    [SerializeField] private Collider2D blockCollider;      // 当たり判定

    [Header("設定")]
    [SerializeField] private bool revealOnPlayerHit = true; // プレイヤーが当たったら出現

    void Start()
    {
        // 最初は非表示＆当たり判定なし
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        // Playerタグのオブジェクトと衝突したら
        if (collision.collider.CompareTag("Player"))
        {
            spriteRenderer.enabled = true;
        }
    }

}