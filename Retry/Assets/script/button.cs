using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���؂�ւ��p
using UnityEngine.UI;
public class button : MonoBehaviour
{

    public string sceneName = "GameScene"; // �J�ڐ�̃V�[����

    void Start()
    {
        // �{�^���̃R���|�[�l���g�擾
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickStart);
    }

    void OnClickStart()
    {
        // �V�[����ǂݍ���
        SceneManager.LoadScene(sceneName);
    }
}
