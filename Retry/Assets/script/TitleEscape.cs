using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleEscape : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "stageselect";
    [SerializeField] private GameObject retryUI;   // ���g���CUI
    [SerializeField] private string nextSceneName = "NextScene"; // ���̃V�[����
    [SerializeField] private float delayTime = 3f; // �V�[���ڍs�܂ł̑ҋ@����

    private bool isRetrying = false; // �A�Ŗh�~�p

    void Update()
    {
        // ESC�Ń^�C�g���֖߂�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.CanStreamedLevelBeLoaded(titleSceneName))
            {
                SceneManager.LoadScene(titleSceneName);
            }
            else
            {
                Debug.LogError($"�V�[�� '{titleSceneName}' �� Build Settings �ɓo�^����Ă��܂���I");
            }
        }

        // R�L�[�Ń��g���C�J�n
        if (Input.GetKeyDown(KeyCode.Backspace) && !isRetrying)
        {
            StartCoroutine(RetrySequence());
        }
    }

    IEnumerator RetrySequence()
    {
        isRetrying = true;

        // UI��\��
        if (retryUI != null)
        {
            retryUI.SetActive(true);
        }

        Debug.Log("Retry Triggered");

        // �w�莞�ԑҋ@
        yield return new WaitForSeconds(delayTime);

        // ���̃V�[���Ɉڍs�i���̃V�[���������[�h�������ꍇ�̓R�����g��؂�ւ��Ăˁj
        SceneManager.LoadScene(nextSceneName);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        isRetrying = false;
    }
}
