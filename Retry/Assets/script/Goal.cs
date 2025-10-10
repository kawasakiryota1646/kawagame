using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject goalUI;  // �S�[��UI��Inspector�Ŏw��
    [SerializeField] private string nextSceneName = "NextScene"; // ���̃V�[����
    [SerializeField] private float delayTime = 3f; // �V�[���ڍs�܂ł̑ҋ@����
    [SerializeField] private AudioClip goalBGM; // �S�[�����ɗ���BGM

    private bool isCleared = false;
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource��ǉ�
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = goalBGM;
        audioSource.loop = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared) return; // ��d�����h�~
        if (other.CompareTag("Player"))
        {
            isCleared = true;

            // �v���C���[����𖳌���
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
                playerController.enabled = false;

            StartCoroutine(GoalSequence());
        }
    }

    IEnumerator GoalSequence()
    {
        // �S�[��UI��\��
        if (goalUI != null)
            goalUI.SetActive(true);

        // �S�[��BGM���Đ�
        if (goalBGM != null)
            audioSource.Play();

        Debug.Log("Goal Reached!");

        // ���b�ҋ@
        yield return new WaitForSeconds(delayTime);

        // ���̃V�[���ֈڍs
        SceneManager.LoadScene(nextSceneName);
    }
}
