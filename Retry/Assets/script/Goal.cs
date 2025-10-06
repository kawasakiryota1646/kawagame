using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject goalUI;  // �S�[��UI��Inspector�Ŏw��
    [SerializeField] private string nextSceneName = "NextScene"; // ���̃V�[����
    [SerializeField] private float delayTime = 3f; // �V�[���ڍs�܂ł̑ҋ@����

    private bool isCleared = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared) return; // ��d�����h�~
        if (other.CompareTag("Player"))
        {
            isCleared = true;
            StartCoroutine(GoalSequence());
        }
    }

    IEnumerator GoalSequence()
    {
        // �S�[��UI��\��
        if (goalUI != null)
            goalUI.SetActive(true);

        Debug.Log("Goal Reached!");

        // ���b�ҋ@
        yield return new WaitForSeconds(delayTime);

        // ���̃V�[���ֈڍs
        SceneManager.LoadScene(nextSceneName);
    }
}