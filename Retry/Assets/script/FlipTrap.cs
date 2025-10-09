using UnityEngine;
using System.Collections;

public class FlipTrap : MonoBehaviour
{
    [Header("�ݒ荀��")]
    [SerializeField] private float flipDelay = 0.5f;      // �v���C���[������Ă��甽�]����܂ł̎���
    [SerializeField] private float flipSpeed = 5f;        // ��]�X�s�[�h
    [SerializeField] private float resetDelay = 3f;       // ���ɖ߂�܂ł̎���
    [SerializeField] private bool resetAfterFlip = true;  // ���ɖ߂邩�ǂ���

    private bool isFlipping = false;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    void Start()
    {
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0f, 180f); // 180�x���]
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFlipping && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FlipAfterDelay());
        }
    }

    IEnumerator FlipAfterDelay()
    {
        isFlipping = true;
        yield return new WaitForSeconds(flipDelay);

        // ������]������
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * flipSpeed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        // ���̓����蔻���OFF
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        if (resetAfterFlip)
        {
            yield return new WaitForSeconds(resetDelay);
            // ���̏�Ԃɖ߂�
            transform.rotation = startRotation;
            if (col) col.enabled = true;
        }

        isFlipping = false;
    }
}
