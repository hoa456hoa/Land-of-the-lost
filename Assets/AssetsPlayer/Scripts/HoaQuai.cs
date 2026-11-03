using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HoaQuai : MonoBehaviour, DameByPlayer
{
    public float detectRadius = 2f;
    public LayerMask checkLayer;
    public GameObject posCircle;
    public float timeToAttack = 1.5f;
    public float currentHp = 100;
    public float maxHp = 100;
    public Image hpbar;
    public GameObject hitbox;

    private Animator animator;
    private bool playerInRange = false;
    private Coroutine attackCoroutine;


    void Start()
    {
        currentHp = maxHp;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckPlayer();
    }

    void CheckPlayer()
    {
        Collider2D[] playerColliders = Physics2D.OverlapCircleAll(posCircle.transform.position, detectRadius, checkLayer);

        if (playerColliders.Length > 0)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                attackCoroutine = StartCoroutine(RepeatAttack());
            }
        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                animator.SetBool("isattack", false);
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
            }
        }
    }

    IEnumerator RepeatAttack()
    {
        while (playerInRange)
        {
            animator.SetBool("isattack", true);
            yield return new WaitForSeconds(0.8f);
            animator.SetBool("isattack", false);
            yield return new WaitForSeconds(timeToAttack);
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
        if(hpbar != null)
        {
            hpbar.fillAmount = currentHp/maxHp;
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        if (posCircle != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(posCircle.transform.position, detectRadius);
        }
    }
    public void HitBoxOn() 
    {
        hitbox.SetActive(true);
    }
    public void HitBoxOff() 
    { 
        hitbox.SetActive(false);
    }
}
