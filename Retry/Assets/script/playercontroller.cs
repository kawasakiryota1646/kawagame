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
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("ゲームオーバー設定")]
    public float fallLimit = -10f;
    public float fallLimitup = 8f;

    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private float delayTime = 3f;

    [Header("UI設定")]
    [SerializeField] private Text deathCountText;

    [Header("サウンド設定🎵")]
    [SerializeField] private AudioClip deathSound;  // ← 死亡効果音
    private AudioSource audioSource;                // ← 再生用

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isDead = false;
    private int deathCount;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // ← AudioSource取得！

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

        isGrounded = CheckGrounded();
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        if (transform.position.y < fallLimit || transform.position.y > fallLimitup)
            StartCoroutine(Gameover());
    }

    bool CheckGrounded()
    {
        bool hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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

        // 🎵 効果音を鳴らす
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogWarning("死亡音が設定されていません！");
        }

        // 🎧 カメラのBGMを止める
        AudioSource cameraAudio = Camera.main.GetComponent<AudioSource>();
        if (cameraAudio != null)
            cameraAudio.Stop();

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
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
