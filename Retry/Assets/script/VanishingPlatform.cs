using UnityEngine;
using System.Collections;

public class VanishingPlatform : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float vanishDelay = 0.5f;   // 乗ってから消えるまでの時間
    [SerializeField] private float reappearDelay = 2f;   // 再出現までの時間
    [SerializeField] private bool loop = true;           // 何度も使えるか

    [Header("演出（任意）")]
    [SerializeField] private AudioClip vanishSound;
    [SerializeField] private ParticleSystem vanishEffect;

    private SpriteRenderer sr;
    private Collider2D col;
    private AudioSource audioSource;
    private bool isActive = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(VanishRoutine());
        }
    }

    IEnumerator VanishRoutine()
    {
        isActive = false;

        yield return new WaitForSeconds(vanishDelay);

        // 消える処理
        if (sr) sr.enabled = false;
        if (col) col.enabled = false;
        if (vanishSound && audioSource) audioSource.PlayOneShot(vanishSound);
        if (vanishEffect) vanishEffect.Play();

        // 再出現まで待つ
        if (loop)
        {
            yield return new WaitForSeconds(reappearDelay);

            if (sr) sr.enabled = true;
            if (col) col.enabled = true;
            isActive = true;
        }
        else
        {
            Destroy(gameObject, 1f); // 一度きりタイプなら削除
        }
    }
}
