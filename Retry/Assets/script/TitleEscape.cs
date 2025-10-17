using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleEscape : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "Title"; // タイトルシーン名を設定

    void Update()
    {
        // Escキーでタイトルへ戻る
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }
}
