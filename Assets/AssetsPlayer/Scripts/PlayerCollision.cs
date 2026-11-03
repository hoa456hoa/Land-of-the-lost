using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameObject hitbox;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player;
        player = FindAnyObjectByType<PlayerMovement>();
        if (collision.CompareTag("Hp")) 
        {
            player.GetHp();
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Wood"))
        {
            player.GetWood();
            Destroy(collision.gameObject);
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
