using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Camera camera;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camera.transform.position + offset;
    }
}
