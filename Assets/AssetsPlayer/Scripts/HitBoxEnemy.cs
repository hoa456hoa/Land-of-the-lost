using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxEnemy : MonoBehaviour
{
    private List<Collider2D> alreadyHit = new List<Collider2D>();

    void OnEnable()
    {
        alreadyHit.Clear();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null && !alreadyHit.Contains(collision))
        {
            player.TakeDamage(1);
            alreadyHit.Add(collision);
        }
    }
}
