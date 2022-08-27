using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        //updating camera position while player is moving
        transform.position = new Vector3(transform.position.x, transform.position.y, (player.transform.position.z + offset.z));
    }
}
