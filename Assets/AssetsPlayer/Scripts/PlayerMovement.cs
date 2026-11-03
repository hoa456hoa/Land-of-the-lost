using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10;
    public float jumpFroce = 5;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public LayerMask ladderLayer;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public GameObject bulletPrefab;
    public Transform gunPos;
    bool isGroundCheck;
    bool isRunning;
    bool isJumping;
    // mien thuong
    private bool isInvincible = false;
    // thoi gian mien thuong
    public float invincibleDuration = 2f;
    public float currentHp = 5;
    public float maxHp = 5;
    public TextMeshProUGUI textHp;
    bool canMove = false;
    float gravityScaleAtStart;
    bool isAlive = true;
    bool isLadderCheck;
    Collider2D myCollider;
    public TextMeshProUGUI textWood;
    public float currentWood = 0;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        currentHp = maxHp;
        UpdateHp();
        canMove = false ;
        gravityScaleAtStart = rb.gravityScale;
    }

    void Update()
    {
        OnMove();
        Shoot();
        Chem();
        ClimbingLadder();
        UpdateAnimation();
    }
    public void OnMove()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        isGroundCheck = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isLadderCheck = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ladderLayer);
        if (moveInput > 0)
        {
            spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)*1, Mathf.Abs(transform.localScale.y)*1, 1);
        }
        else if (moveInput < 0)
        {
            spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)*(-1),Mathf.Abs(transform.localScale.y)* 1, 1);
        }
        if (isGroundCheck || isLadderCheck)
        {
            Jump();
        }
    }
    public void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpFroce);
            rb.gravityScale = gravityScaleAtStart;
            AudioManager.Instance.PlayJumpSound();
        }
    }
    public void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isRunning)
        {
            animator.SetTrigger("isshooting");
            Instantiate(bulletPrefab, gunPos.position, Quaternion.identity);
            AudioManager.Instance.PlayShootSound();
        }
    }
    public void Chem()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isRunning)
        {
            animator.SetTrigger("ischem");
        }
    }
    public void UpdateAnimation()
    {
        isRunning = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f;
        animator.SetBool("isruning", isRunning);
        isJumping = !isGroundCheck;
        animator.SetBool("isjumping", isJumping);
    }
    IEnumerator DamageFlash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f); // thời gian nháy
        sr.color = originalColor;
    }
    // Coroutine mien thuong
    IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }
    public void TakeDamage(float damage)
    {
        if (isInvincible) return; // nếu đang miễn thương thì bỏ qua

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        StartCoroutine(DamageFlash());
        StartCoroutine(InvincibilityTimer()); // bắt đầu miễn thương
        UpdateHp();

        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Die() 
    {
        animator.SetTrigger("isdie");
        Destroy(gameObject, 1f);
        canMove = true;
        isAlive = false;
    }
    public void UpdateHp() 
    {
        textHp.text = currentHp.ToString();
    }
    public void GetHp() 
    {
        currentHp++;
        currentHp = Mathf.Min(maxHp, currentHp);
        UpdateHp();
    }
    public void GetWood()
    {
        currentWood++;
        textWood.text = currentWood.ToString();
    }
    public void CanMove()
    {
        if (canMove == true)
        {
            moveSpeed = 0;
            jumpFroce = 0;
        }

    }
    void ClimbingLadder()
    {
        if (!isAlive) return;
        if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb.gravityScale = gravityScaleAtStart;
            return;
        }
        if (Input.GetButton("Jump")) return;
        float moveInputt = Input.GetAxis("Vertical");
        Vector2 climVelocity = new Vector2(rb.velocity.x, moveInputt * 4);
        rb.velocity = climVelocity;
        rb.gravityScale = 0f;
    }
}
