using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    PlayerMovement playerMovement;
    Rigidbody2D rb;
    public float moveSpeed = 10f;
    public float damage = 10f;
    private float xSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        xSpeed = playerMovement.transform.localScale.x * moveSpeed;
    }
    void Update()
    {
        rb.velocity = new Vector2(xSpeed, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DameByPlayer enemy = collision.GetComponent<DameByPlayer>();
        if (collision.CompareTag("Enemy"))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground")) 
        {
            Destroy(gameObject);
        }
    }
}
