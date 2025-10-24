using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("�e�ݒ�")]
    [SerializeField] private GameObject bulletPrefab;  // �e�̃v���n�u
    [SerializeField] private Transform firePoint;      // ���ˈʒu
    [SerializeField] private float bulletSpeed = 5f;   // �e�̃X�s�[�h
    [SerializeField] private float fireInterval = 2f;  // ���ˊԊu
    [SerializeField] private float detectRange = 8f;   // �v���C���[���m����

    public enum ShootDirection { Right, Left, Up, Down }
    [SerializeField] private ShootDirection shootDirection = ShootDirection.Right;

    [Header("���ʉ��ݒ�")]
    [SerializeField] private AudioClip shootSE;        // ���ˉ�
    [SerializeField] private AudioSource audioSource;  // �Đ��pAudioSource

    private Transform player;
    private float timer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // AudioSource���w��Ȃ玩���擾
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
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

        // ���˕�����enum���猈��
        Vector2 direction = Vector2.zero;
        switch (shootDirection)
        {
            case ShootDirection.Right: direction = Vector2.right; break;
            case ShootDirection.Left: direction = Vector2.left; break;
            case ShootDirection.Up: direction = Vector2.up; break;
            case ShootDirection.Down: direction = Vector2.down; break;
        }

        rb.linearVelocity = direction * bulletSpeed;

        // ���ˉ��Đ�
        if (audioSource && shootSE)
            audioSource.PlayOneShot(shootSE);

        Destroy(bullet, 5f);
    }

    // �G�f�B�^��Ŕ��˕���������
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
