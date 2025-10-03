using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;    // �ړ����x
    public float jumpForce = 7f;    // �W�����v��

    private Rigidbody2D rb;
    private bool isGrounded = false; // �n�ʂɂ��邩�ǂ���
    public float fallLimit = -10f; // ���̍�����艺�ɗ�������Q�[���I�[�o�[
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���ړ�
        float moveX = Input.GetAxis("Horizontal"); // A/D�L�[ or ���L�[
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // �W�����v
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (transform.position.y < fallLimit)
        {
            GameOver();
        }
    }

    // �n�ʂɐG�ꂽ��W�����v�\��
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
    void GameOver()
    {
        // �V�[���������[�h���ă��X�^�[�g
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}