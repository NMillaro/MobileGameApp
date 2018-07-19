using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int enemyDamage = 1;
    public int pointsPerCoin = 5;
    public float restartLevelDelay = 1f;
    public Text coinText;
    public Text healthText;
    public AudioClip hitSound1;
    public AudioClip attackSound1;
    public AudioClip coinSound1;
    public AudioClip chopSound1;
    public AudioClip gameOverSound;
    public AudioClip exitSound;
    GameObject[] enems;
    

    private Animator animator;
    private int health;
    private int coin;

	// Use this for initialization
	protected override void Start () {

        animator = GetComponent<Animator>();

        health = GameManager.instance.playerHealth;
        coin = GameManager.instance.playerCoinPoints;

        healthText.text = "Health: " + health;
        coinText.text = "Coins: " + coin;

        base.Start();
		
	}

    private void OnDisable()
    {
        GameManager.instance.playerHealth = health;
        GameManager.instance.playerCoinPoints = coin;
    }

    // Update is called once per frame
    void Update () {

        

            if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal !=0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        if (xDir == 1 && yDir == 0 )
        {
            animator.SetTrigger("playerWalk");
        }
        else if (xDir == -1 && yDir == 0)
        {
            animator.SetTrigger("playerWalk");
        }
        else if (xDir == 0 && yDir == 1)
        {
            animator.SetTrigger("playerWalkUp");
        }
        else if (xDir == 0 && yDir == -1)
        {
            animator.SetTrigger("playerWalkDown");
        }


        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            enems = GameObject.FindGameObjectsWithTag("Enemy");
            if (enems.Length == 0)
            {
                SoundManager.instance.RandomiseSfx(exitSound);
                animator.SetTrigger("playerExit");
                Invoke("Restart", restartLevelDelay);
                enabled = false;
            }
        }
        else if (other.tag == "Coin")
        {
            coin += pointsPerCoin;
            coinText.text = "+" + pointsPerCoin + " Coins: " + coin;
            SoundManager.instance.RandomiseSfx(coinSound1);
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
        SoundManager.instance.RandomiseSfx(chopSound1);

    }

    protected override void EnemyAttack<T>(T component)
    {
        
        Enemy hitEnemy = component as Enemy;
        animator.ResetTrigger("playerWalk");
        animator.SetTrigger("playerChop");
        SoundManager.instance.RandomiseSfx(attackSound1);
        hitEnemy.TakeDamage(enemyDamage);
        
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseHealth (int loss)
    {
        animator.SetTrigger("playerHit");
        SoundManager.instance.RandomiseSfx(hitSound1);
        health -= loss;
        healthText.text = "Health: " + health + " -" + loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (health <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    protected override void EnemyHitWall<T>(T component)
    {
    }
}
