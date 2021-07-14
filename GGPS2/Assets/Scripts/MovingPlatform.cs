using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool autoMode = true;
    public float speed;
    public float range;
    public Vector2[] goals;
    public int activeGoal;
    // Start is called before the first frame update
    void Awake()
    {
        speed = 2.0f;
        range = 7.0f;

        goals = new Vector2[2];
        goals[0] = (Vector2)transform.position - new Vector2(range,0);
        goals[1] = (Vector2)transform.position + new Vector2(range,0);

        activeGoal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckProximity(goals[activeGoal]);
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
    }

    void CheckProximity(Vector2 goal)
    {
        if (Vector2.Distance(transform.position, goal) < 1.0f)
        {
            Debug.Log("i mdae it");
            activeGoal += 1;
            if (activeGoal >= goals.Length) activeGoal = 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - new Vector3(range, 0, 0), transform.position + new Vector3(range, 0, 0));
    }
}
