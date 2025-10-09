using UnityEngine;
using System.Collections;

public class CloudTrap_UnderTrigger : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    private Vector3 startPosition;  // ���̈ʒu��ۑ�
    private SpriteRenderer sr;      // �_�̌����ڂ������p

    [Header("�����ݒ�")]
    [SerializeField] private float delayBeforeFall = 0.5f; // �������Ă��痎����܂�
    [SerializeField] private float gravityAfterFall = 3f;  // �����X�s�[�h
    [SerializeField] private float triggerRangeX = 1.5f;   // �������̔����͈�
    [SerializeField] private float triggerRangeY = 3f;     // �������̔����͈�

    [Header("���X�|�[���ݒ�")]
    [SerializeField] private float disappearDelay = 1f;    // �������Ă��������܂ł̎���
    [SerializeField] private float respawnDelay = 3f;      // �����Ă���߂�܂ł̎���

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        startPosition = transform.position;
    }

    void Update()
    {
        if (hasFallen) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 playerPos = player.transform.position;
        Vector2 cloudPos = transform.position;

        bool isUnder = playerPos.y < cloudPos.y &&
                       Mathf.Abs(playerPos.x - cloudPos.x) < triggerRangeX &&
                       (cloudPos.y - playerPos.y) < triggerRangeY;

        if (isUnder)
        {
            hasFallen = true;
            Invoke(nameof(FallDown), delayBeforeFall);
        }
    }

    void FallDown()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityAfterFall;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            // ��莞�Ԍ�ɏ����āA���X�|�[���������J�n
            StartCoroutine(DisappearThenRespawn());
        }
    }

    IEnumerator DisappearThenRespawn()
    {
        // ��莞�ԑ҂��Ă���_���\���ɂ���
        yield return new WaitForSeconds(disappearDelay);

        sr.enabled = false;           // �_������
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // ���������ƁA����Ɉ�莞�ԑ҂��Ă��猳�̈ʒu�ɖ߂�
        yield return new WaitForSeconds(respawnDelay);

        transform.position = startPosition;
        sr.enabled = true;            // �ĕ\��
        GetComponent<Collider2D>().enabled = true;

        hasFallen = false;
    }
}
