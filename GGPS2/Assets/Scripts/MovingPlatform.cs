using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool autoMode = true;
    public float speed;
    //public float range;
    private Vector2 direction;
    public Vector2[] goals;
    [SerializeField] private int activeGoal;

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
        rb2d.isKinematic = true;

        activeGoal = 0;
    }

    private void Start()
    {
        for (int i = 0; i < goals.Length; i++)
        {
            goals[i] += (Vector2)transform.localPosition;
        }
    }

    private void Update()
    {
        rb2d.velocity = speed * direction.normalized;
        //transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckProximity(goals[activeGoal]);
    }

    void CheckProximity(Vector2 goal)
    {
        Vector2 distance = (Vector2)transform.localPosition - goal;
        if (distance.magnitude < 0.5f)
        {
            activeGoal++;
            if (activeGoal >= goals.Length) activeGoal = 0;
        }

        if (direction != Vector2.zero)
        {
            // If x direction of next goal is different, flip
            if (flips && Mathf.Sign(goals[activeGoal].x - transform.localPosition.x) != Mathf.Sign(direction.x)) { Flip(); };
        }
        direction = goals[activeGoal] - (Vector2)transform.localPosition;
    }

    void Flip()
    {
        transform.localScale *= new Vector2(-1, 1);
        left = !left;
    }
}
