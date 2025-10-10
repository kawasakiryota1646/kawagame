using UnityEngine;

public class mark2 : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] private float moveDistance = 2f; // �㉺�̈ړ�����
    [SerializeField] private float moveSpeed = 2f;    // �ړ����x
    [SerializeField] private bool startUp = true;     // �ŏ��ɏ�֓�����

    private Vector3 startPos;
    private Vector3 topPos;
    private Vector3 bottomPos;
    private bool movingUp;

    void Start()
    {
        startPos = transform.position;
        topPos = startPos + Vector3.right * moveDistance;
        bottomPos = startPos - Vector3.right * moveDistance;
        movingUp = startUp;
    }

    void Update()
    {
        if (movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, topPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, topPos) < 0.01f)
                movingUp = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, bottomPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, bottomPos) < 0.01f)
                movingUp = true;
        }
    }

    // �V�[����ňړ��͈͂�����
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.right * moveDistance, transform.position - Vector3.right * moveDistance);
    }
}