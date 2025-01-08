using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP = 10f;
    public float speed = 3f;
    private float attacksPerSecond = 1f;
    private bool canAttack = true;
    public GameObject tower;
    private Vector3 towerPos;
    private float attackCooldownTimer = 0f;
    public bool IsAlive => HP > 0;
    public float lastMoveX;
    public float lastMoveY;
    public Animator animator;
    private Vector2 lastMoveDirection;

   
    public float healthMultiplier = 1f;
    public float speedMultiplier = 1f;

    void Start()
    {
        tower = GameObject.FindGameObjectWithTag("Tower");
        towerPos = tower.transform.position;
        animator = GetComponent<Animator>();

        
        HP *= healthMultiplier;
        speed *= speedMultiplier;
    }

    void Update()
    {
        HandleAttackCooldown();

        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }

        if (Vector3.Distance(transform.position, towerPos) > 0.5f)
        {
            Vector3 direction = (towerPos - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, towerPos, speed * Time.deltaTime);

            animator.SetFloat("LastMoveX", direction.x);
            animator.SetFloat("LastMoveY", direction.y);

            lastMoveDirection = new Vector2(direction.x, direction.y);
        }
        else
        {
            lastMoveX = lastMoveDirection.x;
            lastMoveY = lastMoveDirection.y;
        }

        if (speed < 1)
        {
            animator.SetFloat("LastMoveX", 0);
            animator.SetFloat("LastMoveY", 0);
        }
    }

    private void HandleAttackCooldown()
    {
        if (!canAttack)
        {
            attackCooldownTimer += Time.deltaTime;
            if (attackCooldownTimer >= 1f / attacksPerSecond)
            {
                canAttack = true;
                attackCooldownTimer = 0f;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            if (canAttack)
            {
                TowerStats towerStats = tower.GetComponent<TowerStats>();
                if (towerStats != null)
                {
                    towerStats.TakeDamage(2);
                    Debug.Log(gameObject.name + " Hit");
                }

                speed = 0.01f;
                canAttack = false;
            }
        }
    }

    private void OnEnable()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RegisterEnemy(this);
        }
    }

    private void OnDisable()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.UnregisterEnemy(this);
        }
    }
}
