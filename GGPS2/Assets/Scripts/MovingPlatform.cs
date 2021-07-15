using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool autoMode = true;
    public float speed;
    public float range;
    private float direction;
    public Vector2[] goals;
    public int activeGoal;

    public bool flips;
    public bool left;

    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = gameObject.AddComponent<Rigidbody2D>();
        }

        speed = 2.0f;
        range = 7.0f;

        goals = new Vector2[2];
        goals[0] = (Vector2)transform.localPosition - new Vector2(range,0);
        goals[1] = (Vector2)transform.localPosition + new Vector2(range,0);

        activeGoal = 0;
}

    private void Update()
    {
        rb2d.velocity = new Vector2(speed * direction, rb2d.velocity.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckProximity(goals[activeGoal]);
    }

    void CheckProximity(Vector2 goal)
    {
        float distance = Mathf.Abs(transform.localPosition.x - goal.x);
        if (distance < 0.5f)
        {
            activeGoal += 1;
            if (activeGoal >= goals.Length) activeGoal = 0;
            if (flips) { Flip(); };
        }
        direction = Mathf.Sign(goal.x - transform.localPosition.x);
    }

    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - new Vector3(range, 0, 0), transform.position + new Vector3(range, 0, 0));
    }

    void Flip()
    {
        transform.localScale *= new Vector2(-1, 1);
        left = !left;
    }
}
