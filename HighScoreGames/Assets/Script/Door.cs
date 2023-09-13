using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;
    private bool open;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player" && open == false)
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
        while(timer < 1)
        {
            doorObject.transform.position = doorObject.transform.position + Vector3.up * Time.deltaTime * 2;
            timer += Time.deltaTime;
            yield return null;
        }    
        yield return null;
    }
}
