using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;    // 移動速度
    public float jumpForce = 7f;    // ジャンプ力

    private Rigidbody2D rb;
    private bool isGrounded = false; // 地面にいるかどうか
    public float fallLimit = -10f; // この高さより下に落ちたらゲームオーバー
    [SerializeField] private GameObject GameOverUI;  // ゴールUIをInspectorで指定
    [SerializeField] private float delayTime = 3f; // シーン移行までの待機時間



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 横移動
        float moveX = Input.GetAxis("Horizontal"); // A/Dキー or 矢印キー
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // ジャンプ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (transform.position.y < fallLimit)
        {
            StartCoroutine(Gameover());
        }
    }

    // 地面に触れたらジャンプ可能に
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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
        // ゴールUIを表示
        if (GameOverUI != null)
            GameOverUI.SetActive(true);

        Debug.Log("Goal Reached!");

        // 数秒待機
        yield return new WaitForSeconds(delayTime);

        // シーンをリロードしてリスタート
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}