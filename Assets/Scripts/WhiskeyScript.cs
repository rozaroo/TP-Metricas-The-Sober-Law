using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskeyScript : MonoBehaviour
{
    Rigidbody rb;
    Vector2 spawnAim;
    float diff;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        spawnAim = GameManager.Instance.player.transform.position - transform.position;
        diff = (GameManager.Instance.player.transform.position - transform.position).magnitude / 100;
        rb.AddForce(new Vector2(spawnAim.x, spawnAim.y + 6), ForceMode.Impulse);
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 180 * Time.deltaTime));
    }
   
}
