using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    [SerializeField] private float extendHeight = 1f;
    [SerializeField] private float extendSpeed = 3f;
    [SerializeField] private float retractDelay = 2f;
    [SerializeField] private float triggerRange = 3f;

    private Vector3 startPos;
    private Vector3 extendedPos;
    private bool isExtended = false;
    private Collider2D spikeCollider;

    void Start()
    {
        startPos = transform.position;
        extendedPos = startPos + Vector3.up * extendHeight;
        spikeCollider = GetComponent<Collider2D>();
        if (spikeCollider) spikeCollider.enabled = false;
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (!isExtended && distance < triggerRange)
        {
            StartCoroutine(Extend());
        }
    }

    IEnumerator Extend()
    {
        isExtended = true;
        if (spikeCollider) spikeCollider.enabled = true;

        // 出るアニメーション
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * extendSpeed;
            transform.position = Vector3.Lerp(startPos, extendedPos, t);
            yield return null;
        }

        // 一定時間経ったら戻る
        yield return new WaitForSeconds(retractDelay);

        // 引っ込む
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * extendSpeed;
            transform.position = Vector3.Lerp(extendedPos, startPos, t);
            yield return null;
        }

        if (spikeCollider) spikeCollider.enabled = false;
        isExtended = false;
    }
}
