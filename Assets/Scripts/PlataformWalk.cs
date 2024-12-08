using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformWalk : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rayDist;
    [SerializeField] bool isMovingForward;
    [SerializeField] Transform groundPivot;

    public void Walk()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundCheck = Physics2D.Raycast(groundPivot.position, Vector2.down, rayDist);

        if (groundCheck.collider == false)
        {
            Debug.Log("No coll");
            if (isMovingForward)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                isMovingForward = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isMovingForward = true;
            }
        }
    }

    void OnDrawGizmos()
    { 
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(groundPivot.position, Vector2.down);
    }
}
