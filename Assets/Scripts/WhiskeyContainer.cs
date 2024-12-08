using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskeyContainer : MonoBehaviour
{
    [SerializeField] private GameObject whiskey; //PowerUp Reference
    private int maxPowerUps;

    void Start()
    {
        maxPowerUps = 4;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddPowerUp();
            Debug.Log("sdasdasdasdasd");
        }
        if (Input.GetKeyDown(KeyCode.Y)) RemovePowerUp();
    }

    public void AddPowerUp()
    {
        if (transform.childCount < maxPowerUps)
        {
            var whiskeyBottle = Instantiate(whiskey); 
            whiskeyBottle.transform.parent = gameObject.transform; //Make bottle a child for the Layout    
        }
    }

    public void RemovePowerUp()
    {
        Destroy(GetComponent<Transform>().GetChild(0).gameObject); //Destroy first child.
    }
}
