using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour
{
    //accept THIS lantern's key
    public GameObject key;

    //got bumped
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) //it was the player
        {
            //does the player have the torch
            if (other.gameObject.GetComponent<TopDownPlayerMovement>().GetTorch())
            {
                key.SetActive(true); //activate our key
            }
            else //player doesn't have the torch
            {
                Debug.Log("get torch dingus"); //humiliate them
            } 
        }
    }
}
