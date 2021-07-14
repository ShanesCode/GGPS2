using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Camera camera;
    private Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(camera.transform.position.x - transform.position.x, camera.transform.position.y - transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(camera.transform.position.x - offset.x, camera.transform.position.y - offset.y, transform.position.z);
        transform.position = newPos;
    }
}
