using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuaiCay : MonoBehaviour, DameByPlayer
{
    [SerializeField] protected Image hpBar;
    [SerializeField] protected float maxHp = 100f;
    [SerializeField] protected float detectRadius = 4f;
    [SerializeField] protected GameObject posCircle;
    [SerializeField] protected LayerMask checkLayer;
    public float timeToAttack = 3f;
    public bool isFlipped = false;
    protected Animator animator;
    protected float currentHp;
    protected Vector2 startPoint;
    protected Rigidbody2D rb;
    public GameObject bulletPrefabs;
    public Transform firePos;
    public float speedDanThuong = 5;
    PlayerMovement player;
    bool isdie;
    public float moveSpeed = 3;
    bool movingRight = false;
    public float patrolDistance = 2f;
    float speed = 0;
    public GameObject woodPrefabs;
    protected virtual void Start()
    {
        speed = moveSpeed;
        isdie = false;
        player = FindAnyObjectByType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHp = maxHp;
        startPoint = transform.position;
        UpdateHpBar();
    }

    protected virtual void Update()
    {
        Attack();
    }
    private bool canAttack = true;

    private void Attack()
    {
        if (canAttack)
        {
            StartCoroutine(CheckPlayer());
        }
    }
    IEnumerator CheckPlayer()
    {
        Collider2D[] playerCollider = Physics2D.OverlapCircleAll(posCircle.transform.position, detectRadius, checkLayer);

        if (playerCollider.Length > 0)
        {
            animator.SetBool("isrun", false);
            moveSpeed = 0;
            rb.velocity = Vector2.zero;
            canAttack = false;
            animator.SetTrigger("isattack");
            yield return new WaitForSeconds(timeToAttack);
            canAttack = true;
        }
        else if(playerCollider.Length <= 0)
        {
            animator.SetBool("isrun", true);
            moveSpeed = speed;
            Patrol();
        }

    }
    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        StartCoroutine(DamageFlash());
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0 && isdie == false)
        {
            Die();
        }
    }

    private void Die()
    {
        isdie = true;
        animator.SetTrigger("isdie");
        Destroy(gameObject, 1f);
        Instantiate(woodPrefabs, transform.position, Quaternion.identity);
    }

    public void OnDrawGizmosSelected()
    {
        if (posCircle != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(posCircle.transform.position, detectRadius);
        }
    }
    IEnumerator DamageFlash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f); // thời gian nháy
        sr.color = originalColor;
    }
    public void BanDanThuong()
    {
        if (player != null)
        {
            Vector3 directisonToPlayer = player.transform.position - firePos.position;
            directisonToPlayer.Normalize();
            GameObject bullet = Instantiate(bulletPrefabs, firePos.position, Quaternion.identity);
            BulletEnemy enemyBullet = bullet.GetComponent<BulletEnemy>();
            if (enemyBullet != null)
            {
                enemyBullet.SetMoveDirection(directisonToPlayer * speedDanThuong);
            }
        }
    }
    void Patrol()
    {
        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        float distanceMoved = transform.position.x - startPoint.x;

        if (movingRight && distanceMoved >= patrolDistance)
        {
            Flip();
        }
        else if (!movingRight && distanceMoved <= -patrolDistance)
        {
            Flip();
        }
    }
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

