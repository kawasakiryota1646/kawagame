using UnityEngine;
using System.Collections;

public class VanishingPlatform : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField] private float vanishDelay = 0.5f;   // ����Ă��������܂ł̎���
    [SerializeField] private float reappearDelay = 2f;   // �ďo���܂ł̎���
    [SerializeField] private bool loop = true;           // ���x���g���邩

    [Header("���o�i�C�Ӂj")]
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

        // �����鏈��
        if (sr) sr.enabled = false;
        if (col) col.enabled = false;
        if (vanishSound && audioSource) audioSource.PlayOneShot(vanishSound);
        if (vanishEffect) vanishEffect.Play();

        // �ďo���܂ő҂�
        if (loop)
        {
            yield return new WaitForSeconds(reappearDelay);

            if (sr) sr.enabled = true;
            if (col) col.enabled = true;
            isActive = true;
        }
        else
        {
            Destroy(gameObject, 1f); // ��x����^�C�v�Ȃ�폜
        }
    }
}
