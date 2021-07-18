using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Transform p_transform;
    public float y_offset;
    public float smoothingStep;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        p_transform = player.transform;

        InitialisePosition();

        smoothingStep = 5.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float smooth_increment = Mathf.MoveTowards(transform.position.y, player.transform.position.y + y_offset, smoothingStep * Time.deltaTime);
        transform.position = new Vector3(player.transform.position.x, smooth_increment, transform.position.z);
    }

    public void InitialisePosition()
    {
        transform.position = new Vector3(p_transform.position.x, p_transform.position.y + y_offset, transform.position.z);
    }
}
