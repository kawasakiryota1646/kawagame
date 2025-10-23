using UnityEngine;
using System.Collections;

public class SlidingFloorTrap : MonoBehaviour
{
    [Header("�ݒ荀��")]
    [SerializeField] private float slideDistance = 3f;      // �������ɓ�������
    [SerializeField] private float slideSpeed = 2f;         // ��������
    [SerializeField] private float delayBeforeSlide = 0.5f; // �����܂ł̒x��
    [SerializeField] private float resetDelay = 3f;         // ���ɖ߂�܂ł̎���
    [SerializeField] private bool resetAfterSlide = true;   // �߂����ǂ���

    [Header("���m�͈͐ݒ�")]
    [SerializeField] private float triggerRange = 3f;       // �v���C���[�����m���鋗��
    [SerializeField] private LayerMask playerLayer;         // Player���C���[���w��iInspector�Łj

    private bool isSliding = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.left * slideDistance;
    }

    void Update()
    {
        if (isSliding) return;

        // �v���C���[���m
        Collider2D hit = Physics2D.OverlapCircle(transform.position, triggerRange, playerLayer);
        if (hit != null)
        {
            StartCoroutine(SlideTrap());
        }
    }

    IEnumerator SlideTrap()
    {
        isSliding = true;
        yield return new WaitForSeconds(delayBeforeSlide);

        // ���փX���C�h
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        if (resetAfterSlide)
        {
            yield return new WaitForSeconds(resetDelay);

            // ���̈ʒu�ɖ߂�
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * slideSpeed;
                transform.position = Vector3.Lerp(targetPosition, startPosition, t);
                yield return null;
            }
        }

        isSliding = false;
    }

    // Scene��Ō��m�͈͂������i�m�F�p�j
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
