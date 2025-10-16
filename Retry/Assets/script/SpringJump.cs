using UnityEngine;

public class SpringJump : MonoBehaviour
{
    [Header("�o�l�ݒ�")]
    [SerializeField] private float jumpPower = 20f; // ������ԗ�
    [SerializeField] private float compressScale = 0.6f; // �k�ޗ�
    [SerializeField] private float compressSpeed = 10f; // �k�ޑ���
    [SerializeField] private float restoreSpeed = 5f; // ���ɖ߂鑬��

    [Header("���o�I�v�V����")]
    [SerializeField] private AudioClip springSound; // ���ʉ�
    [SerializeField] private ParticleSystem springEffect; // �G�t�F�N�g

    private Vector3 originalScale;
    private AudioSource audioSource;
    private bool isCompressing = false;

    void Start()
    {
        originalScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCompressing)
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // ������ɑ�W�����v�I
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
                playerRb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }

            // ���o����
            if (springSound && audioSource)
                audioSource.PlayOneShot(springSound);

            if (springEffect)
                springEffect.Play();

            StartCoroutine(AnimateSpring());
        }
    }

    private System.Collections.IEnumerator AnimateSpring()
    {
        isCompressing = true;

        // �o�l���k��
        while (transform.localScale.y > originalScale.y * compressScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
                new Vector3(originalScale.x, originalScale.y * compressScale, originalScale.z),
                Time.deltaTime * compressSpeed);
            yield return null;
        }

        // �߂�
        while (transform.localScale.y < originalScale.y - 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
                originalScale,
                Time.deltaTime * restoreSpeed);
            yield return null;
        }

        transform.localScale = originalScale;
        isCompressing = false;
    }
}
