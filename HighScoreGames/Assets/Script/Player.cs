using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    static public Player instance;
    [SerializeField] private GameObject playerObeject;
    [SerializeField] private Animator animator;
    [SerializeField] private LineRenderer lineRenderer;

    private Vector3 mousePos;
    private Vector3 beforePos;
    Vector3 playerPos;
    Vector3 endPos;

    private Coroutine coroutine;
    private bool isMove;
    private bool isHit;
    private bool isRotate;

    private LayerMask mask;

    [Header("Mark")]
    [SerializeField]
    private GameObject endPointer;
    [SerializeField]
    private MeshRenderer endPointerRenderer;
    [SerializeField]
    private Material[] endPointerColor;
    [SerializeField]
    private GameObject markObject;
    [SerializeField]
    private int markCount;
    [SerializeField]
    private float markDis;
    private List<GameObject> markGroup = new List<GameObject>();
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < markCount; i++)
        {
            GameObject mark = Instantiate(markObject);
            mark.transform.parent = transform.GetChild(0);
            markGroup.Add(mark);
        }
        mask = LayerMask.GetMask("map");
        endPointerRenderer = endPointer.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isMove == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerPos = playerObeject.transform.position;
                endPointer.SetActive(true);
            }
            else if (Input.GetMouseButton(0))
            {
                mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

                Vector3 vec = mousePos - playerPos;

                float deg = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                float dis = 0;
                RaycastHit hit;
                if (Physics.Raycast(playerPos, vec, out hit, markCount * markDis, mask))
                {
                    dis = Vector2.Distance(hit.point, playerPos);
                    isHit = true;
                    endPos = hit.point;
                    endPointerRenderer.material = endPointerColor[0];
                }
                else
                {
                    isHit = false;
                    dis = markDis * markCount;
                    endPointerRenderer.material = endPointerColor[1];
                }

                int count = (int)(dis / markDis);
                for (int i = 0; i < markCount; i++)
                {
                    if (i <= count)
                    {
                        MarkRoute(i);
                        markGroup[i].SetActive(true);
                    }
                    else
                        markGroup[i].SetActive(false);
                }
                endPointer.transform.position = markGroup[count].transform.position;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (isHit == true)
                    coroutine = StartCoroutine(Move());
                for (int i = 0; i < markCount; i++)
                {
                    markGroup[i].SetActive(false);
                }
                endPointer.SetActive(false);
            }
        }
    }
    private void MarkRoute(float count)
    {
        Vector3 vec = new Vector3(mousePos.x, mousePos.y) - new Vector3(playerObeject.transform.position.x, playerObeject.transform.position.y);

        markGroup[(int)count].transform.position = new Vector3(playerObeject.transform.position.x, playerObeject.transform.position.y)
            + vec.normalized * markDis * count;
    }
    private IEnumerator Move()
    {
        isMove = true;

        animator.SetBool("wall", false);
        animator.SetBool("grab", true);
        beforePos = transform.position;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        float timer = 0;
        bool rotate = false;
        lineRenderer.SetPosition(1, endPos);
        while (timer < 1)
        {
            lineRenderer.SetPosition(0, playerObeject.transform.position);
            timer += Time.deltaTime * 4;
            transform.position = Vector3.Lerp(beforePos, endPos, timer);
            if (timer > 0.5f && rotate == false)
            {
                isRotate = true;
                rotate = true;
            }
            yield return null;
        }
        lineRenderer.SetPosition(0, playerObeject.transform.position);
        lineRenderer.SetPosition(1, playerObeject.transform.position);
        animator.SetBool("grab", false);
        isMove = false;
    }
    public void WallHit(int idx)
    {
        if (isRotate == true)
        {
            switch (idx)
            {
                case 0:
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    }
                case 1:
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 90);
                        break;
                    }
                case 2:
                    {
                        transform.rotation = Quaternion.Euler(0, 180, -180);
                        break;
                    }
                case 3:
                    {
                        transform.rotation = Quaternion.Euler(0, 180, -90);
                        break;
                    }

            }
            animator.SetBool("wall", idx > 0);
            isRotate = false;
        }
    }
    public void EnemyKill(GameObject enemyObject)
    {
        StopCoroutine(coroutine);
        StartCoroutine(EnemyKillCoroutine(enemyObject));
        animator.SetBool("grab", false);
    }
    private IEnumerator EnemyKillCoroutine(GameObject enemyObject)
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 3;
            transform.position = Vector3.Lerp(transform.position, enemyObject.transform.position, timer);
        }
        isMove = false;
        lineRenderer.SetPosition(1, playerObeject.transform.position);
        lineRenderer.SetPosition(0, playerObeject.transform.position);
        yield return null;
    }
    public void PlayerDead()
    {
        StopCoroutine(coroutine);
        StartCoroutine(Dead());
    }
    private IEnumerator Dead()
    {
        animator.SetBool("Dead", true);
        lineRenderer.SetPosition(1, playerObeject.transform.position);
        lineRenderer.SetPosition(0, playerObeject.transform.position);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
        yield return null;
    }
}
