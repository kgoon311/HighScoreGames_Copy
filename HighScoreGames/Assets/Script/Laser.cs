using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private int move;
    private float timer;
    private Vector3[] vec = new Vector3[2];

    private void Start()
    {
        vec[0] = new Vector3(-0.3f, 6.977716f, 0);
        vec[1] = new Vector3(5.1f, 6.977716f, 0);
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000 , LayerMask.GetMask("map")))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
            if (hit.transform.CompareTag("Player"))
            {
                Player.instance.PlayerDead();
            }
        }
        
        timer += Time.deltaTime / 3;
        if (move == 0)
         transform.position = Vector3.Lerp(vec[0], vec[1], timer);
        else
         transform.position = Vector3.Lerp(vec[1], vec[0], timer);
        if(timer >= 1)
        {
            timer = 0;
            if (move == 0)
                move = 1;
            else
                move = 0;
        }
    }
}
