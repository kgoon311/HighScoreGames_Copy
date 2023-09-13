using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ClampPos
{
    public GameObject left;
    public GameObject right;
    public GameObject top;
    public GameObject bottom;
}

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;
    private Camera cam;
    private Player player;
    
    [SerializeField] private ClampPos clampPosArray;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = Player.instance;
        cam = Camera.main;
    }
    void Update()
    {
        CameraMove();
    }
    private void CameraMove()
    {
        Vector3 min = new Vector3(clampPosArray.left.transform.position.x,
            clampPosArray.bottom.transform.position.y);
        Vector3 max = new Vector3(clampPosArray.right.transform.position.x, 
            clampPosArray.top.transform.position.y);

        float x = Mathf.Clamp(player.transform.position.x, min.x, max.x);
        float y = Mathf.Clamp(player.transform.position.y, min.y, max.y);
        Vector3 vec = new Vector3(x, y,cam.transform.position.z);

        cam.transform.position = vec;
    }    

}
