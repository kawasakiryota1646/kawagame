using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("弾設定")]
    [SerializeField] private GameObject bulletPrefab; // 弾のプレハブ
    [SerializeField] private Transform firePoint;     // 発射位置
    [SerializeField] private float bulletSpeed = 5f;  // 弾のスピード
    [SerializeField] private float fireInterval = 2f; // 発射間隔
    [SerializeField] private float detectRange = 8f;  // プレイヤー検知距離

    [Header("向き設定")]
    [SerializeField] private bool shootRight = true; // 右向きならtrue、左ならfalse

    private Transform player;
    private float timer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // プレイヤーが一定距離に近づいたら撃つ
        if (distance <= detectRange)
        {
            timer += Time.deltaTime;
            if (timer >= fireInterval)
            {
                Shoot();
                timer = 0f;
            }
        }
    }

    void Shoot()
    {
        if (!bulletPrefab || !firePoint) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // 発射方向
        Vector2 direction = shootRight ? Vector2.right : Vector2.left;
        rb.linearVelocity = direction * bulletSpeed;

        Destroy(bullet, 5f);
    }

    // Unityエディタで向きを見やすくするためのガイド
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 dir = shootRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(transform.position, transform.position + dir * 2);
    }
}
