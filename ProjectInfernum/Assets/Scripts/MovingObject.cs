using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {

        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
		
	}

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit, bool checkAndMove)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        float startX = start.x;
        float endX = end.x;

        SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
        Player playerScript = GetComponent<Player>();
        Enemy enemyScript = GetComponent<Enemy>();
        
        
            if (playerScript != null && enemyScript == null)
            {
                if (endX < startX && mySpriteRenderer != null)
                {
                    mySpriteRenderer.flipX = true;
                }
                else if (endX > startX && mySpriteRenderer != null)
                {
                    mySpriteRenderer.flipX = false;
                }
            }
            else if (enemyScript != null && playerScript == null)
            {
                if (endX < startX && mySpriteRenderer != null)
                {
                    mySpriteRenderer.flipX = false;
                }
                else if (endX > startX && mySpriteRenderer != null)
                {
                    mySpriteRenderer.flipX = true;
                }
            }
        

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null && checkAndMove)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }


    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance >float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move (xDir, yDir, out hit, true);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();
        Enemy enemyComponent = hit.transform.GetComponent<Enemy>();
        Player myComponent = GetComponent<Player>();
        Enemy myEnemyComponent = GetComponent<Enemy>();
        Wall wallComponent = hit.transform.GetComponent<Wall>();

        if (!canMove && hitComponent != null && enemyComponent == null)
        {
            OnCantMove(hitComponent);
        }

        if (!canMove && myEnemyComponent != null && wallComponent != null)
        {
            EnemyHitWall(wallComponent);
        }

        if (!canMove && myComponent != null && enemyComponent != null)
        {
            EnemyAttack(enemyComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

    protected abstract void EnemyAttack<T>(T component)
        where T : Component;

    protected abstract void EnemyHitWall<T>(T component)
        where T : Component;
}
