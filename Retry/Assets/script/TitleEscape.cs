using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleEscape : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "stageselect";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // シーンが存在するか確認
            if (Application.CanStreamedLevelBeLoaded(titleSceneName))
            {
                SceneManager.LoadScene(titleSceneName);
            }
            else
            {
                Debug.LogError($"シーン '{titleSceneName}' が Build Settings に登録されていません！");
            }
        }
    }
}
