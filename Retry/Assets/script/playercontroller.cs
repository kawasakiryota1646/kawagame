using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("ジャンプ調整")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("ゲームオーバー設定")]
    public float fallLimit = -10f;
    public float fallLimitup = 8f;

    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private float delayTime = 3f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isDead = false; // ← これ追加！

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 死亡中は操作不能にする
        if (isDead) return;

        // 横移動
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // ジャンプ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 落下補正（マリオ風）
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // 落下死
        if (transform.position.y < fallLimit)
        {
            StartCoroutine(Gameover());
        }

        if (transform.position.y > fallLimitup)
        {
            StartCoroutine(Gameover());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // 敵に当たったら死ぬ
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Gameover());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    IEnumerator Gameover()
    {
        if (isDead) yield break; // ← 二重実行防止
        isDead = true;

        // ゴールUIを表示
        if (GameOverUI != null)
            GameOverUI.SetActive(true);

        Debug.Log("Game Over!");

        // 動きを止める
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // ← 物理挙動も停止

        yield return new WaitForSeconds(delayTime);

        // シーンをリロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}