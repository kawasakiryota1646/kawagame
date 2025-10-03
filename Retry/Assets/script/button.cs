using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替え用
using UnityEngine.UI;
public class button : MonoBehaviour
{

    public string sceneName = "GameScene"; // 遷移先のシーン名

    void Start()
    {
        // ボタンのコンポーネント取得
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickStart);
    }

    void OnClickStart()
    {
        // シーンを読み込む
        SceneManager.LoadScene(sceneName);
    }
}
