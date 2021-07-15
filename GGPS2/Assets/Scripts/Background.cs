using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Camera camera;
    private Vector2 offset;
    public float x_offset;
    public float y_offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(x_offset, y_offset);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(camera.transform.position.x + offset.x, camera.transform.position.y + offset.y, transform.position.z);
        transform.position = newPos;
    }
}
