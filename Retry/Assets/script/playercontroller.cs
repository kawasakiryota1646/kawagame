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

    [Header("ゲームオーバー設定")]
    public float fallLimit = -10f;
    public float fallLimitup = 8f;

    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private float delayTime = 3f;

    [Header("UI設定（任意）")]
    [SerializeField] private Text deathCountText; // 死亡回数を表示するText

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isDead = false;

    private int deathCount; // 死亡回数

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 保存されている死亡回数を読み込む
        deathCount = PlayerPrefs.GetInt("DeathCount", 0);

        // シーン内のUIを自動取得（設定し忘れてもOK）
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

        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (transform.position.y < fallLimit || transform.position.y > fallLimitup)
        {
            StartCoroutine(Gameover());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Enemy"))
            StartCoroutine(Gameover());
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    IEnumerator Gameover()
    {
        if (isDead) yield break;
        isDead = true;

        // 死亡回数をカウント & 保存
        deathCount++;
        PlayerPrefs.SetInt("DeathCount", deathCount);
        PlayerPrefs.Save();
        UpdateDeathCountUI();

        if (GameOverUI != null)
            GameOverUI.SetActive(true);

        Debug.Log("Game Over! Death Count: " + deathCount);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateDeathCountUI()
    {
        if (deathCountText != null)
            deathCountText.text = "Death: " + deathCount.ToString();
    }
}
