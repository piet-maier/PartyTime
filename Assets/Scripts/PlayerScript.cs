using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float movementSpeed = 1;
    public int health;
    public float attackRange;
    public int damage;
    public int highscore;
    public string playerName;
    
    // Movement
    private Rigidbody2D _rigidbody;
    private Vector2 _movementDirection;
    
    // Animations
    private Animator _animator;
    private readonly int _isMoving = Animator.StringToHash("isMoving");
    private readonly int _attack = Animator.StringToHash("attack");
    
    private SpriteRenderer _renderer;

    public GameObject healtbar;

    private float distToTarget;

    public Transform target;

    // This method is called once at the start of the game.
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();

        playerName = "Dummy";
    }

    // This method is called once per frame.
    public void Update()
    {
        Move();
        
        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
        
        HealthbarUpdate();
       
        if (health <= 0)
        {
            Die();
        }

        if (target != null)
            distToTarget = Vector2.Distance(transform.position, target.position);
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
    
    private void Attack()
    {
        // Toggle Attack Animation
        _animator.SetTrigger(_attack);
        
        if (target != null & distToTarget < attackRange)
            target.GetComponent<NpcController>().Damage(damage);
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

    public void Damage(int dmg)
    {
        if (health > 0)
            health -= dmg;
        else
            Die();
    }

    public void HealthbarUpdate()
    {
        healtbar.GetComponent<Text>().text = health + "";
    }

    public void Die()
    {
        SceneManager.LoadScene("GameOverScene");
        Destroy(gameObject);
        
    }
}