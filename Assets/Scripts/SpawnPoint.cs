using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    bool isRightSpawn;

    private void Start()
    {
        if (playerTransform.position.x < transform.position.x) isRightSpawn = true;
    }

    void Update()
    {
        if (isRightSpawn)
        {
            if (Vector3.Distance(playerTransform.position, transform.position) < 10) transform.position += new Vector3(0.5f, 0, 0);
            if (Vector3.Distance(playerTransform.position, transform.position) > 10) transform.position -= new Vector3(0.5f, 0, 0);
        }
        else
        {
            if (Vector3.Distance(playerTransform.position, transform.position) > 10) transform.position += new Vector3(0.5f, 0, 0);
            if (Vector3.Distance(playerTransform.position, transform.position) < 10) transform.position -= new Vector3(0.5f, 0, 0);
        }
    }
}
