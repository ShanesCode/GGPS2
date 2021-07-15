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
        goals[0] = (Vector2)transform.localPosition - new Vector2(range,0);
        goals[1] = (Vector2)transform.localPosition + new Vector2(range,0);

        activeGoal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckProximity(goals[activeGoal]);
        transform.Translate(new Vector2(speed * Time.deltaTime * Mathf.Sign(goals[activeGoal].x - transform.localPosition.x), 0));
    }

    void CheckProximity(Vector2 goal)
    {
        if (Mathf.Abs(transform.localPosition.x - goal.x) < 0.5f)
        {
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
