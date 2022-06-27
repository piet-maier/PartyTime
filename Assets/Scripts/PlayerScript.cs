using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private int regenDelay;

    public float movementSpeed = 1;
    public float health;
    public float maxHealth;
    public float attackRange;
    public int damage;
    public int highscore;
    public string playerName;

    public GameObject healthBarUI;
    public Slider slider;
    
    // Movement
    private Rigidbody2D _rigidbody;
    private Vector2 _movementDirection;
    
    // Animations
    private Animator _animator;
    private readonly int _isMoving = Animator.StringToHash("isMoving");
    private readonly int _attack = Animator.StringToHash("attack");
    
    private SpriteRenderer _renderer;

    //public GameObject healtbar;

    public float distToTarget;

    public Transform target;
    public bool isHitCD;
    public float hitCD;

    public bool isHandUsed;
    public bool isAttack;

    public GameObject damagePopUp;

    public GameObject scenenmanger;

    // This method is called once at the start of the game.
    public void Start()
    {
        maxHealth = health;
        slider.value = CalculateHealth();

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();

        

       

        playerName = "Dummy";
    }

    // This method is called once per frame.
    public void Update()
    {
        slider.value = CalculateHealth();

        if (health < maxHealth)
            healthBarUI.SetActive(true);
        else
            healthBarUI.SetActive(false);

        if (health > maxHealth)
            health = maxHealth;

        Move();
        
        if (Input.GetButtonDown("Fire1") && isHandUsed && !isAttack)
        {
            if(!isAttack)
            {
                isAttack = true;
                // Toggle Attack Animation
                _animator.SetTrigger(_attack);
                StartCoroutine(StartCooldown());

            }
            if (target != null && distToTarget < attackRange && !isHitCD)
            {
                //Attack();
                //StartCoroutine(StartCooldown());

            }

        }
        
        //HealthbarUpdate();
       
        if (health <= 0)
        {
            Die();
        }

        if(health < maxHealth && regenDelay == 0){
                health += 1;
                regenDelay = 500;
            }
        regenDelay--;

        if (target != null)
            distToTarget = Vector2.Distance(transform.position, target.position);
    }

    private float CalculateHealth()
    {
        return health / maxHealth;
    }

    // This method does not depend on the frame rate, so it should be used for calculations related to physics.
    public void FixedUpdate()
    {
        // Delta Time = Time Since Last Function Call => Movement Not Affected By Function Interval
        _rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * movementSpeed * _movementDirection);
    }

    // Get Movement Input & Handle Movement Animation
    private void Move()
    {
        _movementDirection.x = Input.GetAxisRaw("Horizontal");
        _movementDirection.y = Input.GetAxisRaw("Vertical");

        // Normalized => Equal Velocity In All Directions
        _movementDirection = _movementDirection.normalized;
        
        // Toggle Movement Animation
        _animator.SetBool(_isMoving, _movementDirection != Vector2.zero);

        // Flip Sprite Depending On Direction
        _renderer.flipX = _movementDirection.x switch
        {
            < 0 => true,
            > 0 => false,
            _ => _renderer.flipX
        };
    }
    
    //public void Attack()
    //{
    //    isHitCD = true;

    //    target.GetComponent<NpcController>().Damage(Random.Range(0, damage));
    //}

    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(hitCD);
        isHitCD = false;
        isAttack = false;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("NPC"))
        {
            target = collision.gameObject.transform;
            collision.GetComponent<NpcController>().Damage(Random.Range(0, damage));
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            target = null;
        }
    }

    public void Damage(int dmg)
    {
        //if (dmg > 0)
        //{
        //    GameObject damage = Instantiate(damagePopUp, transform.position, Quaternion.identity) as GameObject;
        //    damage.transform.GetChild(0).GetComponent<TextMeshPro>().text = dmg.ToString();
        //}
        //else
        //{
        //    GameObject damage = Instantiate(damagePopUp, transform.position, Quaternion.identity) as GameObject;
        //    damage.transform.GetChild(0).GetComponent<TextMeshPro>().text = "MISSED";
        //}
        regenDelay = 3000;

        if (health > 0)
            health -= dmg;
        else
            Die();
    }

    //public void HealthbarUpdate()
    //{
    //    healtbar.GetComponent<Text>().text = health + "";
    //}

    public void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOverScene");
    }
}