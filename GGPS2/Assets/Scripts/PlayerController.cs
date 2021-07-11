using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int hp;
    private int hpCurrent;

    private float speed;
    private float dashSpeed;
    private float dashFrames;
    private float dashTimer;
    private bool dashing;
    private Vector2 dashVector;

    private float attackFrames;
    private float attackTimer;
    private int attackDamage;
    private float attackSpeed;
    private Transform spawnPoint;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10.0f;
        dashSpeed = 20.0f;
        dashing = false;
        dashFrames = 0.2f;
        dashTimer = 0.0f;

        attackFrames = 0.2f;
        attackTimer = 0.0f;
        attackDamage = 1;
        attackSpeed = 10.0f;
        spawnPoint = transform.GetChild(0).transform;

        projectile = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile");
    }

    // Update is called once per frame
    void Update()
    {
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (dashing)
        {
            transform.Translate(dashVector * dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;
            if(dashTimer <= 0)
            {
                dashing = false;
            }
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            Fire();
        }

        HandleMovement();
    }

    void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire2"))
        {
            dashing = true;
            dashTimer = dashFrames;
            dashVector = new Vector2(Mathf.Round(inputX + Mathf.Sign(inputX) * 0.49f), Mathf.Round(inputY + Mathf.Sign(inputY) * 0.49f));
        }

        float moveX = inputX * speed * Time.deltaTime;
        float moveY = inputY * speed * Time.deltaTime;

        transform.Translate(new Vector2(moveX, moveY));
    }

    void Fire()
    {
        if (attackTimer <= 0)
        {
            Projectile p = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation).GetComponent<Projectile>();
            p.Init(attackDamage, attackSpeed, spawnPoint.rotation);

            attackTimer = attackFrames;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void Damage(int damageValue)
    {
        hpCurrent -= damageValue;
        if(hpCurrent <= 0)
        {
            this.Kill();
        }
    }

    void Kill()
    {
        //play game over
        //delete player
    }
}
