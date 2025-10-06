using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;    // �ړ����x
    public float jumpForce = 7f;    // �W�����v��

    private Rigidbody2D rb;
    private bool isGrounded = false; // �n�ʂɂ��邩�ǂ���
    public float fallLimit = -10f; // ���̍�����艺�ɗ�������Q�[���I�[�o�[
    [SerializeField] private GameObject GameOverUI;  // �S�[��UI��Inspector�Ŏw��
    [SerializeField] private float delayTime = 3f; // �V�[���ڍs�܂ł̑ҋ@����



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
            StartCoroutine(Gameover());
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


    IEnumerator Gameover()
    {
        // �S�[��UI��\��
        if (GameOverUI != null)
            GameOverUI.SetActive(true);

        Debug.Log("Goal Reached!");

        // ���b�ҋ@
        yield return new WaitForSeconds(delayTime);

        // �V�[���������[�h���ă��X�^�[�g
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}