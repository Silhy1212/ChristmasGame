using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public GameObject player;
    public Vector3 movePos;
    public Rigidbody2D rb;
    public float moveSpeed = 3f;
    public Animator animator;
    public float lastMoveX;
    public float lastMoveY;

    private Vector2 lastMoveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePos.z = transform.position.z;
        }

        
        if (Vector3.Distance(transform.position, movePos) > 0.1f)
        {
            Vector3 direction = (movePos - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, movePos, moveSpeed * Time.deltaTime);

           
            animator.SetFloat("LastMoveX", direction.x);
            animator.SetFloat("LastMoveY", direction.y);

            
            lastMoveDirection = new Vector2(direction.x, direction.y);
        }
        else
        {
            animator.SetFloat("LastMoveX", 0);
            animator.SetFloat("LastMoveY", 0);
            lastMoveX = lastMoveDirection.x;
            lastMoveY = lastMoveDirection.y;
        }
    }
}
