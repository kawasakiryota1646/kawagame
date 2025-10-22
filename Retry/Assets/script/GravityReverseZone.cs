using UnityEngine;

public class GravityReverseZone : MonoBehaviour
{
    [SerializeField] private float reverseDuration = 2f; // �t���܂ɂ��鎞��
    [SerializeField] private AudioClip reverseSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            StartCoroutine(ReverseGravity(collision.GetComponent<Rigidbody2D>()));
    }

    private System.Collections.IEnumerator ReverseGravity(Rigidbody2D rb)
    {
        if (rb == null) yield break;

        rb.gravityScale *= -5; // �d�͔��]
        rb.transform.localScale = new Vector3(
            rb.transform.localScale.x,
            rb.transform.localScale.y * -1,
            1
        );

        AudioSource.PlayClipAtPoint(reverseSound, rb.transform.position);
        yield return new WaitForSeconds(reverseDuration);

        rb.gravityScale *= -1; // ���ɖ߂�
        rb.transform.localScale = new Vector3(
            rb.transform.localScale.x,
            Mathf.Abs(rb.transform.localScale.y),
            1
        );
    }
}
