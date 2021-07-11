using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private float speed;
    public Vector2 direction;

    void Awake()
    {
        damage = 1;
        speed = 3.0f;
    }

    public void Init(int dmg, float spd, Quaternion rot)
    {
        speed = spd;
        damage = dmg;

        Vector2 rotEuler = rot.eulerAngles;
        float dirX = Mathf.Cos(rotEuler.x);
        float dirY = Mathf.Sin(rotEuler.y);
        direction = new Vector2(dirX,dirY);
    }

    void Update()
    {
        Vector2 step = speed * direction * Time.deltaTime;
        transform.Translate(step);
    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        Entity e = other.GetComponent<Entity>();

        if (e)
        {
            e.Damage(this.damage);
        }
        
    }
        */
}
