using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("ジャンプ調整")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("接地判定設定")]
    [SerializeField] private Transform groundCheck;      // 足元の判定位置
    [SerializeField] private float groundCheckRadius = 0.2f; // 接地判定の範囲
    [SerializeField] private LayerMask groundLayer;      // 地面のレイヤー

    [Header("ゲームオーバー設定")]
    public float fallLimit = -10f;
    public float fallLimitup = 8f;

    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private float delayTime = 3f;

    [Header("UI設定")]
    [SerializeField] private Text deathCountText;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isDead = false;
    private int deathCount;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        deathCount = PlayerPrefs.GetInt("DeathCount", 0);

        if (deathCountText == null)
        {
            GameObject textObj = GameObject.Find("DeathCountText");
            if (textObj != null)
                deathCountText = textObj.GetComponent<Text>();
        }

        UpdateDeathCountUI();
    }

    void Update()
    {
        if (isDead) return;

        // GroundCheck + Tag両方で接地判定を更新
        isGrounded = CheckGrounded();

        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 落下補正（マリオ風）
        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        if (transform.position.y < fallLimit || transform.position.y > fallLimitup)
            StartCoroutine(Gameover());
    }

    bool CheckGrounded()
    {
        // ① OverlapCircleでレイヤーを判定
        bool hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ② 念のためタグ「Ground」にも反応（柔軟対応）
        if (!hit)
        {
            Collider2D col = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius);
            if (col != null && col.CompareTag("Ground"))
                hit = true;
        }

        return hit;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            StartCoroutine(Gameover());
    }

    IEnumerator Gameover()
    {
        if (isDead) yield break;
        isDead = true;

        deathCount++;
        PlayerPrefs.SetInt("DeathCount", deathCount);
        PlayerPrefs.Save();
        UpdateDeathCountUI();

        if (GameOverUI != null)
            GameOverUI.SetActive(true);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateDeathCountUI()
    {
        if (deathCountText != null)
            deathCountText.text = "Death: " + deathCount;
    }

    void OnDrawGizmosSelected()
    {
        // GroundCheckの範囲をScene上で見えるように
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
