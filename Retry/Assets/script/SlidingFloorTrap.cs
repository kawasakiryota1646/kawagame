using UnityEngine;
using System.Collections;

public class SlidingFloorTrap : MonoBehaviour
{
    [Header("�ݒ荀��")]
    [SerializeField] private float slideDistance = 3f;    // �ǂꂾ�����ɓ�����
    [SerializeField] private float slideSpeed = 2f;       // �ǂꂭ�炢�̑����œ�����
    [SerializeField] private float delayBeforeSlide = 0.5f; // ����Ă��瓮���܂ł̎���
    [SerializeField] private float resetDelay = 3f;       // ���ɖ߂�܂ł̎���
    [SerializeField] private bool resetAfterSlide = true; // �߂����ǂ���

    private bool isSliding = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.left * slideDistance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isSliding && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SlideTrap());
        }
    }

    IEnumerator SlideTrap()
    {
        isSliding = true;
        yield return new WaitForSeconds(delayBeforeSlide);

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
}
