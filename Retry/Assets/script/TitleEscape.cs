using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleEscape : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "Title"; // �^�C�g���V�[����
    [SerializeField] private GameObject confirmUI;            // �m�F�E�B���h�E�iUI�j

    private bool isConfirming = false;

    void Update()
    {
        // Esc�L�[�������ꂽ��m�FUI���g�O���\��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isConfirming)
            {
                ShowConfirmUI(true);
            }
            else
            {
                ShowConfirmUI(false);
            }
        }
    }

    public void OnConfirmYes()
    {
        // �u�͂��v�{�^���Ń^�C�g����
        SceneManager.LoadScene(titleSceneName);
    }

    public void OnConfirmNo()
    {
        // �u�������v�{�^���ŕ���
        ShowConfirmUI(false);
    }

    private void ShowConfirmUI(bool show)
    {
        if (confirmUI != null)
        {
            confirmUI.SetActive(show);
            isConfirming = show;

            // �ꎞ��~
            Time.timeScale = show ? 0f : 1f;
        }
    }
}
