using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;
    private bool open;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && open == false)
        {
            open = true;
            StartCoroutine(DoorOpen());
            Debug.Log("?");
        }
    }
    private IEnumerator DoorOpen()
    {
        float timer = 0;
        Vector3 vector = doorObject.transform.position;
        while (timer < 1)
        {
            doorObject.transform.position = doorObject.transform.position + Vector3.right * Time.deltaTime * 3;
            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
