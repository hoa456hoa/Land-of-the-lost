using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private List<Collider2D> alreadyHit = new List<Collider2D>();

    void OnEnable()
    {
        alreadyHit.Clear();
    }
    public float damage = 10f;

    void OnTriggerStay2D(Collider2D collision)
    {
        DameByPlayer enemy = collision.GetComponent<DameByPlayer>();
        if (collision.CompareTag("Enemy") && !alreadyHit.Contains(collision))
        {
            enemy.TakeDamage(damage);
            alreadyHit.Add(collision);
        }
    }
}