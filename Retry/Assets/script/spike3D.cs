using UnityEngine;
using System.Collections;
public class spike3D : MonoBehaviour
{
    [Header("�����ݒ�")]
    [SerializeField] private float slideDistance = 2f;   // ��яo������
    [SerializeField] private float slideSpeed = 5f;      // ��яo������
    [SerializeField] private float retractDelay = 1.5f;  // �������ނ܂ł̑ҋ@����
    [SerializeField] private bool repeat = true;         // �J��Ԃ���

    [Header("���m�ݒ�")]
    [SerializeField] private float detectRange = 3f;     // ���m�͈�
    [SerializeField] private Transform player;           // �v���C���[�Q�Ɓi�����ł�OK�j
    [SerializeField] private LayerMask playerLayer;      // �v���C���[�̃��C���[�i�C�Ӂj

    [Header("�����ݒ�")]
    [SerializeField] private Vector2 attackDirection = Vector2.up; // �ǂ̕����ɔ�яo����

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isAttacking = false;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + (Vector3)(attackDirection.normalized * slideDistance);

        // �V�[������Player�������ŒT��
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (isAttacking || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectRange)
        {
            StartCoroutine(SpikeAttack());
        }
    }

    IEnumerator SpikeAttack()
    {
        isAttacking = true;

        // ��яo������
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(retractDelay);

        // ���ɖ߂�
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            transform.position = Vector3.Lerp(endPos, startPos, t);
            yield return null;
        }

        isAttacking = false;

        if (!repeat)
            enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(attackDirection.normalized * slideDistance));
    }
}
