using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleEscape : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "Title"; // タイトルシーン名
    [SerializeField] private GameObject confirmUI;            // 確認ウィンドウ（UI）

    private bool isConfirming = false;

    void Update()
    {
        // Escキーが押されたら確認UIをトグル表示
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
        // 「はい」ボタンでタイトルへ
        SceneManager.LoadScene(titleSceneName);
    }

    public void OnConfirmNo()
    {
        // 「いいえ」ボタンで閉じる
        ShowConfirmUI(false);
    }

    private void ShowConfirmUI(bool show)
    {
        if (confirmUI != null)
        {
            confirmUI.SetActive(show);
            isConfirming = show;

            // 一時停止
            Time.timeScale = show ? 0f : 1f;
        }
    }
}
