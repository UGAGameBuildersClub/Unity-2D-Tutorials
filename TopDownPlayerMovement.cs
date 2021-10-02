using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(5f, 2f);
    Vector2 targetPosition, relativePosition, movement;
    int coinsCollected = 0;
    bool canInput = true;

    // Update is called once per frame
    void Update()
    {
        if (canInput)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetAxis("Horizontal") < 0) //left
            {
                targetPosition.x = gameObject.transform.position.x - (60 * speed.x * Time.deltaTime);
            }
            if (Input.GetAxis("Horizontal") > 0) //right
            {
                targetPosition.x = gameObject.transform.position.x + (60 * speed.x * Time.deltaTime);
            }
            if (Input.GetAxis("Vertical") < 0) //down
            {
                targetPosition.y = gameObject.transform.position.y - (60 * speed.y * Time.deltaTime);
            }
            if (Input.GetAxis("Vertical") > 0) //up
            {
                targetPosition.y = gameObject.transform.position.y + (60 * speed.y * Time.deltaTime);
            }
        }
        relativePosition = new Vector2
            (targetPosition.x - gameObject.transform.position.x,
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

    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("hit something");
        if (other.gameObject.CompareTag("Coin")) 
        {
            coinsCollected += 1;
            Debug.Log("coinsCollected" + coinsCollected);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Left Edge")) 
        {
            canInput = false;

            Camera.main.transform.Translate(-19.5f,0f,0f);

            canInput = true;
        }
        if (other.gameObject.CompareTag("Right Edge"))
        {
            canInput = false;

            Camera.main.transform.Translate(19.5f, 0f, 0f);

            canInput = true;
        }
        if (other.gameObject.CompareTag("Down Edge"))
        {
            canInput = false;

            Camera.main.transform.Translate(0f, -8.5f, 0f);

            canInput = true;
        }
        if (other.gameObject.CompareTag("Up Edge"))
        {
            canInput = false;

            Camera.main.transform.Translate(0f, 8.5f, 0f);

            canInput = true;
        }
    }
}
