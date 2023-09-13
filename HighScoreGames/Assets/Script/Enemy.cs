using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider collider;
    public void Dead()
    {
        animator.SetBool("dead", true);
        collider.isTrigger = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Player.instance.EnemyKill(gameObject);
            Dead();
        }

        if (collision.transform.CompareTag("Box"))
        {
            Dead();
        }
    }
}
