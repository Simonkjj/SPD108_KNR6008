using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{

    public Rigidbody2D rb;
    public Rigidbody2D hook;

    public float releaseTime = .15f;
    public float maxDragDistance = 2f;

    public GameObject nextBall;

    private bool isPressed = false;

    void Update()
    {
        if (isPressed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector3.Distance(mousePos, hook.position) > maxDragDistance)
                rb.position = hook.position + (mousePos - hook.position).normalized * maxDragDistance;
            else
                rb.position = mousePos;
        }


        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 t_pos = Camera.main.ScreenToWorldPoint(touch.position);

                RaycastHit2D hit = Physics2D.Raycast(t_pos, t_pos);

                if (isPressed)
                {

                    if (Vector3.Distance(t_pos, hook.position) > maxDragDistance)
                        rb.position = hook.position + (t_pos - hook.position).normalized * maxDragDistance;
                    else
                        rb.position = t_pos;
                }

                if (hit.collider.GetComponent<Ball>())
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        transform.position = t_pos;
                        isPressed = true;
                        rb.isKinematic = true;
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        isPressed = false;
                        rb.isKinematic = false;
                        StartCoroutine(Release());
                    }
                }
            }


        }
    }


    IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseTime);

        GetComponent<SpringJoint2D>().enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(2f);

        if (nextBall != null)
        {
            nextBall.SetActive(true);
        }
        else
        {
            Enemy.EnemiesAlive = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
