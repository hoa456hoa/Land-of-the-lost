using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OcSen : MonoBehaviour, DameByPlayer
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float patrolDistance = 2f;
    bool movingRight = false;
    Vector2 startPoint;
    public float damage = 1;
    public float currentHp = 100f;
    public float maxHp = 100f;
    public Image hpbar;
    Animator animator;
    
    void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        startPoint = transform.position;
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Patrol();
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0) { Die(); }
    }
    public void UpdateHpBar()
    {
        if (hpbar != null)
        {
            hpbar.fillAmount = currentHp / maxHp;
        }
    }
    public void Die()
    {
        animator.SetTrigger("isdie");
        Destroy(gameObject, 1f);
    }
}
