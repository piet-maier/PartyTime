using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float speed = 1;
    public int health;
    public int attackRange;
    public int damage;
    public int highscore;

    private float distToTarget;

    public Transform target;

    private Vector2 _direction;

    private Rigidbody2D _rigidbody;
    
    private Animator _animator;
    private readonly int _isMoving = Animator.StringToHash("isMoving");

    private SpriteRenderer _renderer;

    // This method is called once at the start of the game.
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // This method is called once per frame.
    public void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        if (target != null)
            distToTarget = Vector2.Distance(transform.position, target.position);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Attack();
        }

        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");

        // Normalized => Equal Velocity In All Directions
        _direction = _direction.normalized;
        
        // Toggle Movement Animation
        _animator.SetBool(_isMoving, _direction != Vector2.zero);

        // Flip Sprite Depending On Direction
        _renderer.flipX = _direction.x switch
        {
            < 0 => true,
            > 0 => false,
            _ => _renderer.flipX
        };
    }
    public void Attack()
    {
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

    // This method does not depend on the frame rate, so it should be used for calculations related to physics.
    public void FixedUpdate()
    {
        // Delta Time = Time Since Last Function Call => Movement Not Affected By Function Interval
        _rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * speed * _direction);
    }

    public void Damage(int dmg)
    {
        if (health > 0)
            health -= dmg;
        else
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOverScene");
    }
}