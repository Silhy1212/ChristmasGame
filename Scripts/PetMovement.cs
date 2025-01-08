using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public float speed = 4f;
    [SerializeField] public float distanceToPlayer;
    [SerializeField] public float distanceToEnemyThreshold = 5f;
    [SerializeField] public List<Enemy> aliveEnemies = new List<Enemy>();
    public float lastMoveX;
    public float lastMoveY;
    public Animator animator;
    private Vector2 lastMoveDirection;

    private Enemy closestEnemy;
    private float attackCooldown = 2f; 
    private float attackTimer = 0f; 

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        aliveEnemies = EnemyManager.Instance.GetAliveEnemies();

        aliveEnemies.RemoveAll(enemy => enemy.CompareTag("EnemyYellow"));
        if (closestEnemy == null || !aliveEnemies.Contains(closestEnemy))
        {
            FindClosestEnemy();
            attackCooldown = 0f;
        }
        

        if (closestEnemy != null && Vector3.Distance(transform.position, closestEnemy.transform.position) <= distanceToEnemyThreshold)
        {
            if (Vector3.Distance(transform.position, closestEnemy.transform.position) < 0.5f) 
                {
                speed = 0f;
                }
            else { speed = 3f; }
            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.transform.position, speed * Time.deltaTime);
            Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
            animator.SetFloat("LastMoveX", direction.x);
            animator.SetFloat("LastMoveY", direction.y);
            lastMoveDirection = new Vector2(direction.x, direction.y);

            
            if (Vector3.Distance(transform.position, closestEnemy.transform.position) < 0.5f)
            {
                Vector2 lastMove = new Vector2(direction.x, direction.y);

                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                     
                    animator.SetBool("IsAttacking", true);
                    Bite(closestEnemy);
                    animator.SetBool("IsAttacking", false);
                    Debug.Log("Dog bit an enemy");
                    attackTimer = 0f; 
                }
            }
        }
        else
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer >= 5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                Vector3 direction = (player.transform.position - transform.position).normalized;
                animator.SetFloat("LastMoveX", direction.x);
                animator.SetFloat("LastMoveY", direction.y);
                lastMoveDirection = new Vector2(direction.x, direction.y);
            }
            else
            {
                animator.SetFloat("LastMoveX", 0);
                animator.SetFloat("LastMoveY", 0);
            }
        }
    }

    private void FindClosestEnemy()
    {
        float shortestDistance = Mathf.Infinity;
        closestEnemy = null;
        foreach (Enemy enemy in aliveEnemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
    }

    private void Bite(Enemy enemy)
    {
        enemy.HP -= 5;
    }
}
