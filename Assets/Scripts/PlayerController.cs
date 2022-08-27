using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerController : MonoBehaviour
{
    private Touch touch;
    public float speed = 0;
    public float touchSpeed = 0;
    public float forceMagnitude = 0;
    public bool stopFlag;
    private int dir = 0;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

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


        if (Touch.activeFingers.Count == 1)
        {
            MovePlayer(Touch.activeTouches[0]);
        }

        /*if (Input.touchCount > 0)
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

            /*if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.x > 0)
                {
                    Debug.Log("rigjt");
                    if (transform.position.x <= 5.5)
                    {
                        dir = 1;
                        transform.position += Vector3.right * Time.deltaTime * touchSpeed;
                    }
                }
                else if (touch.deltaPosition.x < 0)
                {
                    Debug.Log("left");
                    if (transform.position.x >= -8)
                    {
                        dir = -1;
                        transform.position += Vector3.left * Time.deltaTime * touchSpeed;
                    }
                }
            }
            if (touch.phase == TouchPhase.Stationary)
            {
                if (dir == 1)
                {
                    Debug.Log("rigghhhhtttttttttttt");
                    if (transform.position.x <= 5.5)
                        transform.position += Vector3.right * Time.deltaTime * touchSpeed;
                }
                else if (dir == -1)
                {
                    Debug.Log("leeeeeeeeeeeeft");
                    if (transform.position.x >= -8)
                        transform.position += Vector3.left * Time.deltaTime * touchSpeed;
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                dir = 0;
            }
        }*/

    }
    private void MovePlayer(Touch touch)
    {

        if (touch.phase != TouchPhase.Moved)
        {
            return;
        }
        if (touch.delta.x > 0 && transform.position.x >= 5.5)
        {
            return;
        }
        else if (touch.delta.x < 0 && transform.position.x <= -8)
        {
            return;
        }

        if(touch.delta.x < 1 && touch.delta.x > -1)
            return;

        Vector3 newPosition = new Vector3(touch.delta.normalized.x, 0, 0) * Time.deltaTime * touchSpeed;


        transform.position += newPosition;
        //CameraController.Instance?.Move(newPosition);
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

