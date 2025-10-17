using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class FakePlayer : MonoBehaviour
{
    [Header("動き設定")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpInterval = 1.5f;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool isGrounded = true;

    [Header("ゲームオーバー設定")]
    public float fallLimit = -10f;
    public float fallLimitup = 8f;

    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private float delayTime = 3f;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(RandomBehavior());

        // 🔽 自動でシーン上のUIを探す
        if (GameOverUI == null)
        {
            GameOverUI = GameObject.Find("GameOverUI");
            if (GameOverUI == null)
            {
                // Canvas内からも探す
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    Transform found = canvas.transform.Find("GameOverUI");
                    if (found != null) GameOverUI = found.gameObject;
                }
            }
        }
    }

    private void Update()
    {
        if (transform.position.y < fallLimit || transform.position.y > fallLimitup)
        {
            StartCoroutine(Gameover());
        }
    }

    IEnumerator RandomBehavior()
    {
        while (true)
        {
            float dir = Random.value > 0.5f ? 1f : -1f;
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

            if ((dir > 0 && !facingRight) || (dir < 0 && facingRight))
            {
                facingRight = !facingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }

            if (isGrounded && Random.value > 0.3f)
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(jumpInterval);
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

        Debug.Log($"{gameObject.name} が死亡！");

        if (GameOverUI != null)
        {
            Debug.Log("GameOverUIを表示します");
            GameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameOverUIが見つかりません！");
        }

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
