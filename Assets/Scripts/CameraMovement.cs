using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
   [SerializeField] public GameObject player;
   
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraMovement = player.transform.position;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
    }
}
