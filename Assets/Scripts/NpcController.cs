using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NpcController : MonoBehaviour
{
    public NPC npc;
    public bool friendy;

    public float health;
    public float maxHealth;

    public GameObject healthBarUI;
    public Slider slider;

    internal Transform thisTransform;

    // The movement speed of the object
    public float moveSpeed;

    // A minimum and maximum time delay for taking a decision, choosing a direction to move in
    public Vector2 decisionTime = new Vector2(1, 2);
    internal float decisionTimeCount = 0;

    // The possible directions that the object can move int, right, left, up, down, and zero for staying in place.
    // I added zero multiple times to give a bigger chance if it happening than other directions
    internal Vector2[] moveDirections = new Vector2[]
    {
        Vector2.right, Vector2.left, Vector2.up, Vector2.down,
        Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero,
        Vector2.zero, Vector2.zero
    };

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

    public SpriteRenderer spriterenderer;

    public Sprite sprite;

    private Animator _animator;
    private readonly int _isMoving = Animator.StringToHash("IsWalking");
    private readonly int _attack = Animator.StringToHash("Attack");
    private readonly int _die = Animator.StringToHash("Death");

    public bool left;
    public bool right;


    public bool isDying;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

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

        gameObject.GetComponent<SpriteRenderer>().sprite = npc.sprite;

        spriterenderer = GetComponent<SpriteRenderer>();

        if (GameObject.FindGameObjectsWithTag("spawnArea") != null)
            connectedSpawnArea = GetComponentInParent<NPCSpawn>().gameObject;

        _rigidbody = this.GetComponent<Rigidbody2D>();

        gameObject.AddComponent<PathFinding>();

        // Cache the transform for quicker access
        thisTransform = this.transform;

        // Set a random time delay for taking a decision ( changing direction, or standing in place for a while )
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);

        // Choose a movement direction, or stay in place
        ChooseMoveDirection();

        _animator = GetComponent<Animator>();


    }



    public IEnumerator DyingCooldown()
    {
        isDying = true;
        _animator.SetTrigger(_die);
        yield return new WaitForSeconds(1.5f);
        isDying = false;
        target.GetComponent<PlayerScript>().isPsychoCam = false;
        Die();
        
        
    }
    public void Update()
    {
       
        slider.value = CalculateHealth();

        if (health < maxHealth)
            healthBarUI.SetActive(true);
        else
            healthBarUI.SetActive(false);

        if (health > maxHealth)
            health = maxHealth;


        if (health <= 0 && !isDying)
        {    
            moveSpeed = 0;

            if (target != null)
                target.GetComponent<PlayerScript>().highscore += GetScoreValue();

            if (connectedSpawnArea != null && connectedSpawnArea.GetComponent<NPCSpawn>().npcPrefab != null && !connectedSpawnArea.GetComponent<NPCSpawn>().bossAlive)
            {
                connectedSpawnArea.GetComponent<NPCSpawn>().npcsAlive--;
                Die();

            }
            else if(connectedSpawnArea != null && connectedSpawnArea.GetComponent<NPCSpawn>().bossPrefab != null && connectedSpawnArea.GetComponent<NPCSpawn>().bossAlive)
            {
                Debug.Log("jo");
                connectedSpawnArea.GetComponent<NPCSpawn>().bossKilled = true;
                connectedSpawnArea.GetComponent<NPCSpawn>().bossesAlive--;
                connectedSpawnArea.GetComponent<NPCSpawn>().bossAlive = false;
                
                StartCoroutine(DyingCooldown());
            }

           

           
        }




        if (!isChasing)
        {
            Movement();
        }

        if (target != null)
        {
            if (!target.GetComponent<PlayerScript>().isPsychoCam && !friendy)
            {
                Destroy(gameObject);
            }

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

            if (distToTarget < attackRange && !isHitCD && !isDying)
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
        _animator.SetTrigger(_attack);
        target.GetComponent<PlayerScript>().Damage(Random.Range(0, damage));
        StartCoroutine(StartCooldown());
    }

    public IEnumerator StartCooldown()
    {
        isHitCD = true;
        yield return new WaitForSeconds(hitCD);
        isHitCD = false;
    }

    public IEnumerator FlashDamage()
    {
        spriterenderer.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        spriterenderer.color = Color.white;
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
        _rigidbody.MovePosition(_rigidbody.position +
                                Time.deltaTime * moveSpeed * moveDirections[currentMoveDirection].normalized);

        //thisTransform.position += moveDirections[currentMoveDirection] * Time.deltaTime * moveSpeed;

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

        if (currentMoveDirection == 0)
        {
            spriterenderer.flipX = true;
        }

        if (currentMoveDirection == 1)
        {
            spriterenderer.flipX = false;
        }
    }

    void ChooseMoveDirection()
    {
        // Choose whether to move sideways or up/down
        currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
    }

    public void Chase()
    {
        _animator.SetBool(_isMoving, true);
        if (target != null)
        {
            var path = GetComponent<PathFinding>().path;
            if (path != null && path.Count != 0)
            {
                var target = new Vector2(path[0].worldPosition.x, path[0].worldPosition.y);
                _rigidbody.MovePosition(_rigidbody.position +
                                        moveSpeed * Time.deltaTime * (target - _rigidbody.position).normalized);

                if (target.x < 0)
                {
                    spriterenderer.flipX = true;
                }

                if (target.x > 0)
                {
                    spriterenderer.flipX = false;
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.position.x > gameObject.transform.position.x)
            left = true;
        else
            right = true;

        if (collision.CompareTag("Player"))
        {
            target = collision.gameObject.GetComponent<Rigidbody2D>();
            StartCoroutine(FlashDamage());
        }
    }

    public void StopChasingPlayer()
    {
        _rigidbody.velocity = new Vector2(0, 0);
        //target = null;
        isChasing = false;
    }

    public void Damage(int dmg)
    {
        if (dmg > 0)
        {
            GameObject damage = Instantiate(damagePopUp, transform.position, Quaternion.identity) as GameObject;
            damage.transform.GetChild(0).GetComponent<TextMeshPro>().text = dmg.ToString();

            if (right == true)
                _rigidbody.AddForce(Vector2.right * gameObject.transform.localScale * 300); //knockback
            if (left == true)
                _rigidbody.AddForce(Vector2.left * gameObject.transform.localScale * 300);
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