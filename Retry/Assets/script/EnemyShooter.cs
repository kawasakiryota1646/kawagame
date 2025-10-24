using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("弾設定")]
    [SerializeField] private GameObject bulletPrefab;  // 弾のプレハブ
    [SerializeField] private Transform firePoint;      // 発射位置
    [SerializeField] private float bulletSpeed = 5f;   // 弾のスピード
    [SerializeField] private float fireInterval = 2f;  // 発射間隔
    [SerializeField] private float detectRange = 8f;   // プレイヤー検知距離

    public enum ShootDirection { Right, Left, Up, Down }
    [SerializeField] private ShootDirection shootDirection = ShootDirection.Right;

    [Header("効果音設定")]
    [SerializeField] private AudioClip shootSE;        // 発射音
    [SerializeField] private AudioSource audioSource;  // 再生用AudioSource

    private Transform player;
    private float timer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // AudioSource未指定なら自動取得
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
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

        // 発射方向をenumから決定
        Vector2 direction = Vector2.zero;
        switch (shootDirection)
        {
            case ShootDirection.Right: direction = Vector2.right; break;
            case ShootDirection.Left: direction = Vector2.left; break;
            case ShootDirection.Up: direction = Vector2.up; break;
            case ShootDirection.Down: direction = Vector2.down; break;
        }

        rb.linearVelocity = direction * bulletSpeed;

        // 発射音再生
        if (audioSource && shootSE)
            audioSource.PlayOneShot(shootSE);

        Destroy(bullet, 5f);
    }

    // エディタ上で発射方向を可視化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 dir = Vector3.zero;

        switch (shootDirection)
        {
            case ShootDirection.Right: dir = Vector3.right; break;
            case ShootDirection.Left: dir = Vector3.left; break;
            case ShootDirection.Up: dir = Vector3.up; break;
            case ShootDirection.Down: dir = Vector3.down; break;
        }

        Gizmos.DrawLine(transform.position, transform.position + dir * 2);
    }
}
