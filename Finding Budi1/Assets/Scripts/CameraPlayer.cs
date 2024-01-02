using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public GameObject character;
    private Vector3 camera = new Vector3(0,4.5f,23.3f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = character.transform.position + camera;
    }
}
