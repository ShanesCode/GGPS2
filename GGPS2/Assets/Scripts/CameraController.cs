using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector2 offset;
    public float x_offset;
    public float y_offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(x_offset, y_offset);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, transform.position.z);
        transform.position = newPos;
    }
}
