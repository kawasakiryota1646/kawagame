using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // �{�^������Ăяo�����\�b�h
    public void QuitGame()
    {
        Debug.Log("�Q�[���I��"); // �G�f�B�^�m�F�p

        // ���ۂɃA�v���P�[�V�������I��
        Application.Quit();

#if UNITY_EDITOR
        // �G�f�B�^��ł͏I�����Ȃ��̂Ŏ~�߂����Ƃ��͂���
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}