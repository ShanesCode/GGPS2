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
    GameObject sticky;
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

        tag = "Ground";

        /*sticky = new GameObject();
        sticky.transform.SetParent(gameObject.transform);
        sticky.transform.localPosition = Vector2.zero;
        sticky.AddComponent<Rigidbody2D>();
        sticky.GetComponent<Rigidbody2D>().isKinematic = true;
        sticky.AddComponent<BoxCollider2D>().isTrigger = true;
        sticky.GetComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.1f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.1f);
        sticky.AddComponent<StickyPlatform>();*/
    }

    private void Update()
    {
        CheckProximity(goals[activeGoal]);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = speed * direction.normalized;
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
