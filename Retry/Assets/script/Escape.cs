using UnityEngine;

public class Escape : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField] private Transform player;       // �v���C���[��Transform
    [SerializeField] private Transform escapePoint;  // ������̒n�_
    [SerializeField] private float detectRange = 5f; // �����o������
    [SerializeField] private float moveSpeed = 3f;   // �����鑬��
    [SerializeField] private float stopDistance = 0.1f; // ������Ŏ~�܂鋗��

    private bool isEscaping = false;
    private bool hasEscaped = false;

    void Update()
    {
        if (player == null || escapePoint == null) return;

        // ���łɓ����؂��Ă��牽�����Ȃ�
        if (hasEscaped) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // �v���C���[���߂Â����瓦����
        if (!isEscaping && distance < detectRange)
        {
            isEscaping = true;
        }

        // ������
        if (isEscaping)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                escapePoint.position,
                moveSpeed * Time.deltaTime
            );

            // �����؂�����I��
            if (Vector2.Distance(transform.position, escapePoint.position) <= stopDistance)
            {
                isEscaping = false;
                hasEscaped = true;
            }
        }
    }

    // �G�f�B�^�Ŕ͈͊m�F
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (escapePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, escapePoint.position);
        }
    }
}