using UnityEngine;

public class BGMVolumeAdjuster : MonoBehaviour
{
    [Range(0f, 2f)] // ← 1以上にして「ブースト」も可能
    public float bgmVolume = 1f;

    void Start()
    {
        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.volume = bgmVolume;
            Debug.Log($"BGM音量を {bgmVolume} に設定しました");
        }
    }
}
