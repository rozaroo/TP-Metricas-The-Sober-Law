using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraScript : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        if (GameManager.Instance.player == null)
        {
            GameManager.Instance.FindPlayer();
            player = GameManager.Instance.player;
        }
        player = GameManager.Instance.player;
    }
    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (GameManager.Instance.player != null)
        {
            if (player.transform.position.x >= -2.4 && player.transform.position.x <= 121) transform.position = new Vector3(player.transform.position.x, 0, transform.position.z);
        }
        else GameManager.Instance.FindPlayer();
    }
}
