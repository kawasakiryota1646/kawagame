using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleEscape : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "Title"; // �^�C�g���V�[������ݒ�

    void Update()
    {
        // Esc�L�[�Ń^�C�g���֖߂�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }
}
