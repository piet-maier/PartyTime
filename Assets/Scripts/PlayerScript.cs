using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 1;

    private Animator _animator;
    private readonly int _isMoving = Animator.StringToHash("isMoving");

    private SpriteRenderer _spriteRenderer;

    public void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        // Get Input
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        // Normalized => Equal Velocity In All Directions
        var direction = new Vector2(horizontal, vertical).normalized;

        // Delta Time => Movement Not Affected By Frame Rate
        transform.Translate(Time.deltaTime * speed * direction);

        // Toggle Movement Animation
        _animator.SetBool(_isMoving, direction != Vector2.zero);

        if (direction.x < 0)
        {
            _spriteRenderer.flipX = true;
        }

        if (direction.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }
}