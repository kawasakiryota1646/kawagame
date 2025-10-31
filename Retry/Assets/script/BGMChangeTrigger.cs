using UnityEngine;

public class BGMChangeTrigger : MonoBehaviour
{
    [Header("新しいBGMクリップ")]
    [SerializeField] private AudioClip newBGM;

    [Header("音量設定")]
    [SerializeField, Range(0f, 1f)] private float fadeOutSpeed = 1f;
    [SerializeField, Range(0f, 1f)] private float fadeInSpeed = 1f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            AudioSource mainAudio = Camera.main.GetComponent<AudioSource>();
            if (mainAudio != null && newBGM != null)
            {
                StartCoroutine(FadeBGM(mainAudio, newBGM));
            }
        }
    }

    private System.Collections.IEnumerator FadeBGM(AudioSource audioSource, AudioClip nextClip)
    {
        float originalVolume = audioSource.volume;

        // フェードアウト
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= fadeOutSpeed * Time.deltaTime;
            yield return null;
        }

        // 新しい曲に切り替え
        audioSource.clip = nextClip;
        audioSource.Play();

        // フェードイン
        while (audioSource.volume < originalVolume)
        {
            audioSource.volume += fadeInSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
