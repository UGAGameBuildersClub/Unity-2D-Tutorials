using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;

    void FixedUpdate() 
    {
        transform.position = new Vector3 (player.position.x, 1, -10); // (x,y,z)
    }
}
