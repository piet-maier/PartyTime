using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geysir : MonoBehaviour
{

    public int damage;
    public bool persistent;
    private int attackCountdown;
    private bool planAttack;
    internal Transform thisTransform;
    public float attackRange;
    private float distToTarget;
    public Rigidbody2D target;
    
    private Animator _animator;
    private readonly int _attack = Animator.StringToHash("Attack");

    // Start is called before the first frame update
    void Start()
    {
        attackCountdown = 501;
        planAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        distToTarget = Vector2.Distance(transform.position, target.position);
        
        if(planAttack){
            if(attackCountdown-- == 0){
                Attack();
                planAttack = false;
            }
        } else if(distToTarget < attackRange){
            planAttack = true;
        }

    }

    public void Attack()
    {
        _animator.SetTrigger(_attack);
        target.GetComponent<PlayerScript>().Damage(Random.Range(0, damage));
        if(!persistent){
            //Die after animation
        }
        attackCountdown = 501;
    }
}
