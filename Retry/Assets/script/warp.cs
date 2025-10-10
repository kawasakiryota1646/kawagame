using UnityEngine;
using System.Collections;

public class warp : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField] private Transform exitPipe;       // �o���̓y��
    [SerializeField] private float warpDelay = 0.5f;   // �y�ǂ̒��ɒ���ł鎞��
    [SerializeField] private float sinkDistance = 1f;  // �y�ǂɒ��ދ���
    [SerializeField] private float sinkSpeed = 2f;     // ���ރX�s�[�h
    [SerializeField] private float appearSpeed = 2f;   // �o��X�s�[�h
    [SerializeField] private float controlLockTime = 1.0f; // �o�����Ƒ���s����
    [SerializeField] private KeyCode warpKey = KeyCode.S;

    private bool isPlayerInside = false;
    private bool isWarping = false;
    private Transform player;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerRenderer;
    private PlayerController playerController; // �� �v���C���[����X�N���v�g�ւ̎Q��

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            player = other.transform;
            playerRb = player.GetComponent<Rigidbody2D>();
            playerRenderer = player.GetComponent<SpriteRenderer>();
            playerController = player.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void Update()
    {
        if (isPlayerInside && !isWarping && Input.GetKeyDown(warpKey))
        {
            StartCoroutine(WarpAnimation());
        }
    }

    private IEnumerator WarpAnimation()
    {
        if (exitPipe == null || player == null) yield break;
        isWarping = true;

        // �v���C���[�̓������~
        if (playerController) playerController.enabled = false; // �� ���͂��~�߂�

        Vector3 startPos = player.position;
        Vector3 targetPos = startPos + Vector3.down * sinkDistance;

        // �y�ǂɒ���
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * sinkSpeed;
            player.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // �y�Ǔ��ŏ�����
        if (playerRenderer) playerRenderer.enabled = false;

        yield return new WaitForSeconds(warpDelay);

        // �o���փ��[�v
        player.position = exitPipe.position;

        // �o���������֏o�鉉�o
        Vector3 appearStart = exitPipe.position + Vector3.down * sinkDistance;
        Vector3 appearEnd = exitPipe.position;
        player.position = appearStart;

        if (playerRenderer) playerRenderer.enabled = true;

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * appearSpeed;
            player.position = Vector3.Lerp(appearStart, appearEnd, t);
            yield return null;
        }

        // �o�������������s�\
        yield return new WaitForSeconds(controlLockTime);

        // �������ĊJ
        if (playerRb) playerRb.simulated = true;
        if (playerController) playerController.enabled = true;

        isWarping = false;
    }
}
