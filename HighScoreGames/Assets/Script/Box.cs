using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            rigidbody.AddForce(Vector3.right * 2);
        }
        if(collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Dead();
        }
    }
}
