using UnityEngine;

public class HitBox : MonoBehaviour
{
    private PolygonCollider2D _playerCollider;
    
    private PolygonCollider2D[] _colliders;
    
    public PolygonCollider2D attack;

    public enum HitBoxes
    {
        Attack,
        Clear
    }

    public void Start()
    {
        _playerCollider = gameObject.AddComponent<PolygonCollider2D>();
        _playerCollider.isTrigger = true;
        _playerCollider.pathCount = 0;
        
        _colliders = new PolygonCollider2D[] { attack };
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("The player hit something.");
    }

    public void SetHitBox(HitBoxes box)
    {
        if (box != HitBoxes.Clear) _playerCollider.SetPath(0, _colliders[(int)box].GetPath(0));
        else _playerCollider.pathCount = 0;
    }
}