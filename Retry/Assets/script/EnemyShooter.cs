using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("�e�ݒ�")]
    [SerializeField] private GameObject bulletPrefab; // �e�̃v���n�u
    [SerializeField] private Transform firePoint;     // ���ˈʒu
    [SerializeField] private float bulletSpeed = 5f;  // �e�̃X�s�[�h
    [SerializeField] private float fireInterval = 2f; // ���ˊԊu
    [SerializeField] private float detectRange = 8f;  // �v���C���[���m����

    [Header("�����ݒ�")]
    [SerializeField] private bool shootRight = true; // �E�����Ȃ�true�A���Ȃ�false

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

        // �v���C���[����苗���ɋ߂Â����猂��
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

        // ���˕���
        Vector2 direction = shootRight ? Vector2.right : Vector2.left;
        rb.linearVelocity = direction * bulletSpeed;

        Destroy(bullet, 5f);
    }

    // Unity�G�f�B�^�Ō��������₷�����邽�߂̃K�C�h
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 dir = shootRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(transform.position, transform.position + dir * 2);
    }
}
