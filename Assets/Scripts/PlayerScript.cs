using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float movementSpeed = 1;
    public float health;
    public float maxHealth;
    public float attackRange;
    public float damage;
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
        
        if (Input.GetButton("Fire1") && !isHitCD)
        {
            Attack();
            StartCoroutine(StartCooldown());

        }
        
        //HealthbarUpdate();
       
        if (health <= 0)
        {
            Die();
        }

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
    
    public void Attack()
    {  
        if (target != null & distToTarget < attackRange)
        {
            isHitCD = true;
            // Toggle Attack Animation
            _animator.SetTrigger(_attack);

            target.GetComponent<NpcController>().Damage(damage);
        }


    }

    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(hitCD);
        isHitCD = false;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            target = collision.gameObject.transform;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            target = null;
        }
    }

    public void Damage(float dmg)
    {
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
        SceneManager.LoadScene("GameOverScene");
        Destroy(gameObject);
        
    }
}