using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Retry : MonoBehaviour
{
    [SerializeField] private GameObject retryUI;   // ���g���CUI
    [SerializeField] private string nextSceneName = "NextScene"; // ���̃V�[����
    [SerializeField] private float delayTime = 3f; // �V�[���ڍs�܂ł̑ҋ@����

    private bool isTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return; // ��d�����h�~

        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(RetrySequence(other.gameObject));
        }
    }

    IEnumerator RetrySequence(GameObject player)
    {
        // �v���C���[������~
        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // UI��\��
        if (retryUI != null)
            retryUI.SetActive(true);

        Debug.Log("Retry Triggered");

        // �w�莞�ԑҋ@
        yield return new WaitForSeconds(delayTime);

        // �V�[���������[�h or �ڍs
        SceneManager.LoadScene(nextSceneName);
    }
}
