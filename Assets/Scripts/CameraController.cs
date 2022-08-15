using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // LateUpdate runs every frame but after all other updates are done
    void LateUpdate()
    {
        //updating camera position while player is moving
        transform.position = new Vector3(transform.position.x, transform.position.y, (player.transform.position.z + offset.z));
    }
}
