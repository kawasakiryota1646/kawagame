using UnityEngine;
using System.Collections;

public class CloneSwitch : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject fakePlayerPrefab; // 偽物プレイヤーPrefab
    [SerializeField] private int cloneCount = 2;          // 出現数
    [SerializeField] private float spawnRadius = 1.5f;    // 出現位置のばらけ具合
    [SerializeField] private AudioClip spawnSound;        // 効果音
    [SerializeField] private float cloneLifetime = 6f;    // 生存時間

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
            Destroy(fake, cloneLifetime); // 一定時間後に消滅
        }

        yield return null;
    }
}
