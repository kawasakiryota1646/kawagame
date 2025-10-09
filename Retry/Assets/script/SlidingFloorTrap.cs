using UnityEngine;
using System.Collections;

public class SlidingFloorTrap : MonoBehaviour
{
    [Header("İ’è€–Ú")]
    [SerializeField] private float slideDistance = 3f;    // ‚Ç‚ê‚¾‚¯¶‚É“®‚­‚©
    [SerializeField] private float slideSpeed = 2f;       // ‚Ç‚ê‚­‚ç‚¢‚Ì‘¬‚³‚Å“®‚­‚©
    [SerializeField] private float delayBeforeSlide = 0.5f; // æ‚Á‚Ä‚©‚ç“®‚­‚Ü‚Å‚ÌŠÔ
    [SerializeField] private float resetDelay = 3f;       // Œ³‚É–ß‚é‚Ü‚Å‚ÌŠÔ
    [SerializeField] private bool resetAfterSlide = true; // –ß‚·‚©‚Ç‚¤‚©

    private bool isSliding = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.left * slideDistance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isSliding && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SlideTrap());
        }
    }

    IEnumerator SlideTrap()
    {
        isSliding = true;
        yield return new WaitForSeconds(delayBeforeSlide);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        if (resetAfterSlide)
        {
            yield return new WaitForSeconds(resetDelay);

            // Œ³‚ÌˆÊ’u‚É–ß‚·
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * slideSpeed;
                transform.position = Vector3.Lerp(targetPosition, startPosition, t);
                yield return null;
            }
        }

        isSliding = false;
    }
}
