using UnityEngine;

public class SpringJump : MonoBehaviour
{
    [Header("バネ設定")]
    [SerializeField] private float jumpPower = 20f; // 吹っ飛ぶ力
    [SerializeField] private float compressScale = 0.6f; // 縮む量
    [SerializeField] private float compressSpeed = 10f; // 縮む速さ
    [SerializeField] private float restoreSpeed = 5f; // 元に戻る速さ

    [Header("演出オプション")]
    [SerializeField] private AudioClip springSound; // 効果音
    [SerializeField] private ParticleSystem springEffect; // エフェクト

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
                // 上方向に大ジャンプ！
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
                playerRb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }

            // 演出処理
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

        // バネが縮む
        while (transform.localScale.y > originalScale.y * compressScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
                new Vector3(originalScale.x, originalScale.y * compressScale, originalScale.z),
                Time.deltaTime * compressSpeed);
            yield return null;
        }

        // 戻る
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
