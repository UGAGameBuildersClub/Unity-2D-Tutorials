using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(5f,2f);
    Vector2 targetPosition, relativePosition, movement;
    int coinsCollected = 0;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            targetPosition.x = gameObject.transform.position.x - (60 * speed.x * Time.deltaTime);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            targetPosition.x = gameObject.transform.position.x + (60 * speed.x * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            targetPosition.y = gameObject.transform.position.y - (60 * speed.y * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            targetPosition.y = gameObject.transform.position.y + (60 * speed.y * Time.deltaTime);
        }

        relativePosition = new Vector2(
            targetPosition.x - gameObject.transform.position.x,
            targetPosition.y - gameObject.transform.position.y);
    }

    void FixedUpdate()
    {
        if (speed.x * Time.deltaTime >= Mathf.Abs(relativePosition.x))
        {
            movement.x = relativePosition.x;
        }
        else 
        {
            movement.x = speed.x * Mathf.Sign(relativePosition.x);
        }
        if (speed.y * Time.deltaTime >= Mathf.Abs(relativePosition.y))
        {
            movement.y = relativePosition.y;
        }
        else
        {
            movement.y = speed.y * Mathf.Sign(relativePosition.y);
        }

        GetComponent<Rigidbody2D>().velocity = movement;
    }
    
}
