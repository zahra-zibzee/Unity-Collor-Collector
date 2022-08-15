using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Touch touch;
    public float speed = 0;
    public float touchSpeed = 0;
    public float forceMagnitude = 0;
    public bool stopFlag;
    private int dir = 0;

    void Update()
    {
        if (stopFlag)
            return;
            
        transform.position += Vector3.forward * Time.deltaTime * speed;

        if (Input.GetKey("right"))
        {
            if (!(transform.position.x >= 5.5))
                transform.position += Vector3.right * Time.deltaTime * speed;

        }
        else if (Input.GetKey("left"))
        {
            if (!(transform.position.x <= -8))
                transform.position += Vector3.left * Time.deltaTime * speed;
        }

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            /*if (touch.position.x < Screen.width / 2)
            {
                transform.position += Vector3.left * Time.deltaTime * speed;
            }
            else if (touch.position.x >= Screen.width / 2)
            {
                transform.position += Vector3.right * Time.deltaTime * speed;
            }*/


            /*if (touch.phase == TouchPhase.Moved)
            {

                if(transform.position.x + touch.deltaPosition.x * touchSpeed <= 5.5 && transform.position.x + touch.deltaPosition.x * touchSpeed >= -8)
                {
                    Debug.Log(touch.deltaPosition);
                    if (touch.deltaPosition.x > 0) {
                        if (touch.deltaPosition.x <= 10)
                            transform.position += new Vector3(touch.deltaPosition.x * touchSpeed, 0, 0);
                        else
                            transform.position += new Vector3(10 * touchSpeed, 0, 0);
                    }
                    else
                    {
                        if (touch.deltaPosition.x >= -10)
                            transform.position += new Vector3(touch.deltaPosition.x * touchSpeed, 0, 0);
                        else
                            transform.position += new Vector3(-10 * touchSpeed, 0, 0);
                    }
                }
            }*/

            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.x > 10 && transform.position.x <= 5.5)
                {
                    dir = 1;
                    transform.position += Vector3.right * Time.deltaTime * touchSpeed;
                }
                else if (touch.deltaPosition.x < 10 && transform.position.x >= -8)
                {
                    dir = -1;
                    transform.position += Vector3.left * Time.deltaTime * touchSpeed;
                }
            }
            if(touch.phase == TouchPhase.Stationary)
            {
                if (dir == 1 && transform.position.x <= 5.5)
                {
                    transform.position += Vector3.right * Time.deltaTime * touchSpeed;
                }
                else if (dir == -1 && transform.position.x >= -8)
                {
                    transform.position += Vector3.left * Time.deltaTime * touchSpeed;
                }
            }
            if(touch.phase == TouchPhase.Ended)
            {
                dir = 0;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();
            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }

    public void ResetPlayer()
    {
        transform.position = new Vector3(-0.5f, 0.5f, -65);
        stopFlag = false;
    }

}

