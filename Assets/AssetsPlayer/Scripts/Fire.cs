using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    PlayerMovement player;
    void Start()
    {
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        player = FindAnyObjectByType<PlayerMovement>();
        if (player != null)
        {
            if (collision.CompareTag("Player"))
            {
                player.TakeDamage(1f);
            }
        }
    }
}
