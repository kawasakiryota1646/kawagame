using UnityEngine;
using System.Collections;

public class CloneSwitch : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField] private GameObject fakePlayerPrefab; // �U���v���C���[Prefab
    [SerializeField] private int cloneCount = 2;          // �o����
    [SerializeField] private float spawnRadius = 1.5f;    // �o���ʒu�̂΂炯�
    [SerializeField] private AudioClip spawnSound;        // ���ʉ�
    [SerializeField] private float cloneLifetime = 6f;    // ��������

    private bool activated = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        if (other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(SpawnFakePlayers(other.transform));
        }
    }

    IEnumerator SpawnFakePlayers(Transform player)
    {
        if (spawnSound && audioSource)
            audioSource.PlayOneShot(spawnSound);

        for (int i = 0; i < cloneCount; i++)
        {
            Vector3 spawnPos = player.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0.5f,
                0f
            );

            GameObject fake = Instantiate(fakePlayerPrefab, spawnPos, Quaternion.identity);
            Destroy(fake, cloneLifetime); // ��莞�Ԍ�ɏ���
        }

        yield return null;
    }
}
