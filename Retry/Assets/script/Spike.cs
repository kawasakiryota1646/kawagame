
using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    [SerializeField] private float extendHeight = 1f;
    [SerializeField] private float extendSpeed = 3f;
    [SerializeField] private float retractDelay = 2f;
    [SerializeField] private float triggerRangeX = 3f; // ‰¡•ûŒü‚Ì”ÍˆÍ
    [SerializeField] private float triggerRangeY = 2f; // c•ûŒü‚Ì”ÍˆÍ

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

        Vector3 playerPos = player.transform.position;
        float dx = Mathf.Abs(playerPos.x - transform.position.x);
        float dy = Mathf.Abs(playerPos.y - transform.position.y);

        // c‰¡‚Ì”ÍˆÍ‚Å”»’è
        if (!isExtended && dx < triggerRangeX && dy < triggerRangeY)
        {
            StartCoroutine(Extend());
        }
    }

    IEnumerator Extend()
    {
        isExtended = true;
        if (spikeCollider) spikeCollider.enabled = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * extendSpeed;
            transform.position = Vector3.Lerp(startPos, extendedPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(retractDelay);

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
