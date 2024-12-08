using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskyCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            collision.gameObject.GetComponent<Player>().RefreshHealth(-30);
            Destroy(transform.parent.gameObject);
        }
    }
}
