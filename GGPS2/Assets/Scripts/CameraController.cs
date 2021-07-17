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

        player.GetComponent<PlayerController>().OnGrounded += Cam_OnPlayerGrounded;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(p_transform.position.x, transform.position.y, transform.position.z);
        transform.position = newPos;
    }

    private void Cam_OnPlayerGrounded(object sender, PlayerController.OnGroundedEventArgs e)
    {
        float smooth_increment = Mathf.MoveTowards(transform.position.y, player.transform.position.y + y_offset, smoothingStep * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, smooth_increment, transform.position.z);
    }

    public void InitialisePosition()
    {
        transform.position = new Vector3(p_transform.position.x, p_transform.position.y + y_offset, transform.position.z);
    }
}
