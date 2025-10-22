using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleEscape : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "stageselect";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �V�[�������݂��邩�m�F
            if (Application.CanStreamedLevelBeLoaded(titleSceneName))
            {
                SceneManager.LoadScene(titleSceneName);
            }
            else
            {
                Debug.LogError($"�V�[�� '{titleSceneName}' �� Build Settings �ɓo�^����Ă��܂���I");
            }
        }
    }
}
