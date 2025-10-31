using UnityEngine;

public class BGMVolumeAdjuster : MonoBehaviour
{
    [Range(0f, 2f)] // �� 1�ȏ�ɂ��āu�u�[�X�g�v���\
    public float bgmVolume = 1f;

    void Start()
    {
        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.volume = bgmVolume;
            Debug.Log($"BGM���ʂ� {bgmVolume} �ɐݒ肵�܂���");
        }
    }
}
