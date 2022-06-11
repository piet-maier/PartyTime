using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 1;

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

    // This method does not depend on the frame rate, so it should be used for calculations related to physics.
    public void FixedUpdate()
    {
        // Delta Time = Time Since Last Function Call => Movement Not Affected By Function Interval
        _rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * speed * _direction);
    }
}