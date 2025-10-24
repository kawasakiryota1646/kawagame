using UnityEngine;
using System.Collections;

public class walltrap : MonoBehaviour
{
    [Header("�|���ݒ�")]
    [SerializeField] private float rotationAngle = 90f; // �|���p�x
    [SerializeField] private float fallSpeed = 50f;     // ��]�X�s�[�h
    [SerializeField] private float delayBeforeFall = 1f; // �����܂ł̒x��
    [SerializeField] private float triggerRange = 3f;    // �v���C���[���߂Â�����

    [Header("�ׂ��ꂽ���̐ݒ�")]
    [SerializeField] private GameObject gameOverUI;      // GameOver��UI
    [SerializeField] private float delayBeforeRestart = 2f; // �V�[�����Z�b�g�܂ł̎���

    private bool isFalling = false;
    private Quaternion startRot;
    private Quaternion targetRot;
    private AudioSource audioSource;

    void Start()
    {
        startRot = transform.rotation;
        targetRot = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotationAngle));
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // �v���C���[���߂Â�����|���
        if (!isFalling && distance < triggerRange)
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    IEnumerator FallAfterDelay()
    {
        isFalling = true;
        yield return new WaitForSeconds(delayBeforeFall);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * (fallSpeed / 100f);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }
    }


}
