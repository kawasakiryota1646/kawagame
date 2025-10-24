using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // ボタンから呼び出すメソッド
    public void QuitGame()
    {
        Debug.Log("ゲーム終了"); // エディタ確認用

        // 実際にアプリケーションを終了
        Application.Quit();

#if UNITY_EDITOR
        // エディタ上では終了しないので止めたいときはこれ
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}