using UnityEngine;

public class BGMChangeTrigger : MonoBehaviour
{
    [Header("�V����BGM�N���b�v")]
    [SerializeField] private AudioClip newBGM;

    [Header("���ʐݒ�")]
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

        // �t�F�[�h�A�E�g
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= fadeOutSpeed * Time.deltaTime;
            yield return null;
        }

        // �V�����Ȃɐ؂�ւ�
        audioSource.clip = nextClip;
        audioSource.Play();

        // �t�F�[�h�C��
        while (audioSource.volume < originalVolume)
        {
            audioSource.volume += fadeInSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
