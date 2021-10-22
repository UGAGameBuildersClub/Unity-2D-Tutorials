using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoomScript : MonoBehaviour
{
    //prefab for enemy
    public Transform enemy;
    public int enemyNum = 1;
    public bool lockDoors;
    GameObject enemies;
    bool hasEntered;

    void Start()
    {
        //find the empty game object we want to hold our enemies
        enemies = gameObject.transform.Find("Enemies").gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if we have already entered the room
        if (hasEntered)
        {
            //delete the children
            Debug.Log("Deleting children");
            //for each loop, goes through every child of this gameobject
            foreach (Transform child in enemies.transform)
            {
                //calls their Begone function
                child.GetComponent<EnemyScript>().Begone();
            }
            //unenter the room
            hasEntered = false;
        }
        else  //hasEntered = false
        {
            //bad, should have better name
            //but im lazy. listen, youre getting this stuff for free
            DoStuff();
        }
    }

    void DoStuff()
    {
        //enter the room
        hasEntered = true;
        if (lockDoors) //if you should lock the doors
        {
            //sets every child active, even the enemies empty game object
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        //add a new enemy until we have the right number
        for (int i = 0; i < enemyNum; i++)
        {
            //finds a random point to spawn at. NOTE z=0 THAT IS CRUCIAL
            //otherwise your collisions will mess up. ask me how i know
            Vector3 v = new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f), 0f);
            //Instantiate (prefab object to use, place to spawn in at, rotation to use, game object to make this a child of)
            //Quaternions are fancy math things I dont understand .identity basically means no rotation tho
            Instantiate(enemy, gameObject.transform.position + v, Quaternion.identity, enemies.transform);
        }
    }
}
