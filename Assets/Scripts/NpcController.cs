using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NpcController : MonoBehaviour
{
    public NPC npc;

    public float health;
    public float maxHealth;

    public GameObject healthBarUI;
    public Slider slider;

    internal Transform thisTransform;

    // The movement speed of the object
    public float moveSpeed;

    // A minimum and maximum time delay for taking a decision, choosing a direction to move in
    public Vector2 decisionTime = new Vector2(1, 4);
    internal float decisionTimeCount = 0;

    // The possible directions that the object can move int, right, left, up, down, and zero for staying in place. I added zero twice to give a bigger chance if it happening than other directions
    internal Vector2[] moveDirections = new Vector2[] { Vector2.right, Vector2.left, Vector2.up, Vector2.down, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
    internal int currentMoveDirection;


    public GameObject connectedSpawnArea;

    public float distToTarget;

    public bool isHitCD;
    public bool isChasing;

    public Rigidbody2D target;

    public float aggroRange;

    public Rigidbody2D _rigidbody;

    public float hitCD;

    public int damage;

    public float attackRange;

    public int scoreValue;

    public GameObject damagePopUp;




    // Use this for initialization
    public void Start()
    {
        maxHealth = npc.health;
        health = maxHealth;
        slider.value = CalculateHealth();
        moveSpeed = npc.speed;
        aggroRange = npc.aggroRange;
        attackRange = npc.attackRange;
        scoreValue = npc.scoreValue;
        damage = npc.damage;
        hitCD = npc.hitCD;



        if (GameObject.FindGameObjectsWithTag("spawnArea") != null)
            connectedSpawnArea = GetComponentInParent<NPCSpawn>().gameObject;

        _rigidbody = this.GetComponent<Rigidbody2D>();

        // Cache the transform for quicker access
        thisTransform = this.transform;

        // Set a random time delay for taking a decision ( changing direction, or standing in place for a while )
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);

        // Choose a movement direction, or stay in place
        ChooseMoveDirection();
    }

    public void Update()
    {
        slider.value = CalculateHealth();

        if(health < maxHealth)
            healthBarUI.SetActive(true);
        else
            healthBarUI.SetActive(false);

        if (health > maxHealth)
            health = maxHealth;
        

        if(health <= 0)
        {
            if(target != null)
                target.GetComponent<PlayerScript>().highscore += GetScoreValue();

            if (connectedSpawnArea != null)
                connectedSpawnArea.GetComponent<NPCSpawn>().npcsAlive--;

            Die();
        }

        if (!isChasing)
        {
            Movement();

        }

        if (target != null)
        {
            distToTarget = Vector2.Distance(transform.position, target.position);

            if (distToTarget < aggroRange)
            {
                // Direction();
                Chase();
                isChasing = true;
            }
            else
            {
                StopChasingPlayer();
            }

            if (distToTarget < attackRange && !isHitCD)
            {
                Attack();
            }
        }
    }



    private float CalculateHealth()
    {
        return health / maxHealth;
    }

    public void Attack()
    {
        target.GetComponent<PlayerScript>().Damage(Random.Range(0, damage));
        StartCoroutine(StartCooldown());
    }

    public IEnumerator StartCooldown()
    {
        isHitCD = true;
        yield return new WaitForSeconds(hitCD);
        isHitCD = false;
    }

    public void Direction()
    {
        if (target != null)
        {
            Vector3 direction = target.position - _rigidbody.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
            direction.Normalize();
            //movement = direction;
        }

    }

    public void Movement()
    {
        // Move the object in the chosen direction at the set speed
        _rigidbody.MovePosition(_rigidbody.position + Time.deltaTime * moveSpeed * moveDirections[currentMoveDirection].normalized);

        //thisTransform.position += moveDirections[currentMoveDirection] * Time.deltaTime * moveSpeed;

        if (moveDirections[currentMoveDirection] == Vector2.zero)
        {
            //animator.SetBool("attack", false);
            //animator.SetBool("run", false);
            //animator.SetBool("idle", true);
        }

        if (decisionTimeCount > 0)
        {
            decisionTimeCount -= Time.deltaTime;
        }
        else
        {
            // Choose a random time delay for taking a decision ( changing direction, or standing in place for a while )
            decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);

            // Choose a movement direction, or stay in place
            ChooseMoveDirection();
        }
    }

    void ChooseMoveDirection()
    {
        // Choose whether to move sideways or up/down
        currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
    }

    public void Chase()
    {
        if (target != null)
        {
            //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            _rigidbody.MovePosition(_rigidbody.position + (target.position - _rigidbody.position).normalized * Time.deltaTime * moveSpeed);
        }

        //rb.MovePosition((Vector2)transform.position + (direction * npc.speed * Time.deltaTime));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    public void StopChasingPlayer()
    {
        _rigidbody.velocity = new Vector2(0, 0);
        target = null;
        isChasing = false;
    }

    public void Damage(int dmg)
    {
        if(dmg > 0)
        {
            GameObject damage = Instantiate(damagePopUp, transform.position, Quaternion.identity) as GameObject;
            damage.transform.GetChild(0).GetComponent<TextMeshPro>().text = dmg.ToString();
            //_rigidbody.AddForce(Vector2.right * gameObject.transform.localScale * 300); //knockback
        }
        else
        {
            GameObject damage = Instantiate(damagePopUp, transform.position, Quaternion.identity) as GameObject;
            damage.transform.GetChild(0).GetComponent<TextMeshPro>().text = "MISSED";
        } 
        if (health > 0)
        {
            health -= dmg;
        }
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
