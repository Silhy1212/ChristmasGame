using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxRange = 15f;
    public float damage;

    private Vector3 direction;
    private Vector3 startPosition;

    public void SetTarget(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
        startPosition = transform.position; 
    }

    void Update()
    {
       
        transform.position += direction * speed * Time.deltaTime;

        
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxRange)
        {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().HP -= 10;
            Debug.Log("HIT");

            Destroy(gameObject); 
        }
        else if (collision.gameObject.CompareTag("EnemyYellow"))
        {
            collision.gameObject.GetComponent<Enemy>().HP -= damage;
            Debug.Log("HIT");

            Destroy(gameObject); 
        }
        else if (collision.gameObject.CompareTag("EnemyBlack"))
        {
            collision.gameObject.GetComponent<Enemy>().HP -= damage;
            Debug.Log("HIT");

            Destroy(gameObject); 
        }
    }
}
