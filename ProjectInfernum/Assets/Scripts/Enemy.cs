using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDamage;
    public int wallDamage;

    private Animator animator;
    private int health = 1;
    private Transform target;
    private bool skipMove;

	protected override void Start () {

        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
		
	}
	 
    
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {

        if (health > 0)
        {
            int xDir = 0;
            int yDir = 0;

            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            {
                yDir = target.position.y > transform.position.y ? 1 : -1;
            }
            else
            {
                xDir = target.position.x > transform.position.x ? 1 : -1;
            }

            AttemptMove<Player>(xDir, yDir);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        animator.SetTrigger("enemyAttack");

        hitPlayer.LoseHealth(playerDamage);
    }

    protected override void EnemyHitWall<T>(T component)
    {
        
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("enemyAttack");
    }

    public void TakeDamage(int damage)
    { 
        health -= damage;

        if (health <= 0)
        {
            animator.SetTrigger("enemyDead");
            DestroyObject(gameObject, 0.25f);
            //gameObject.SetActive (false);
        }
    }

    protected override void EnemyAttack<T>(T component)
    {
    }

}
