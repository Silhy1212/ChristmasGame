using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    private Vector3 targetPosition;

    private float QCoolDownCounter = 0f;
    private float QCoolDownDuration = 1f;

    private float WCoolDownCounter = 0f;
    private float WCoolDownDuration = 5f;

    private EnemyManager enemyManager;

    public float bulletDamage = 10f; 
    public float wAbilityDamage = 5f; 

    
    public GameObject freezeImage; 
    public float freezeEffectDuration = 2f;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        if (freezeImage != null)
        {
            freezeImage.SetActive(false); 
        }
    }

    void Update()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;

        if (QCoolDownCounter > 0)
        {
            QCoolDownCounter -= Time.deltaTime;
            if (QCoolDownCounter < 0) QCoolDownCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (QCoolDownCounter == 0)
            {
                ShootBullet();
                QCoolDownCounter = QCoolDownDuration;
                Debug.Log("Ability used. Cooldown started.");
            }
            else
            {
                Debug.Log($"Ability not ready yet. {QCoolDownCounter:F1} seconds remaining.");
            }
        }

        if (WCoolDownCounter > 0)
        {
            WCoolDownCounter -= Time.deltaTime;
            if (WCoolDownCounter < 0) WCoolDownCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (WCoolDownCounter == 0)
            {
                
                foreach (var enemy in enemyManager.GetAliveEnemies())
                {
                    enemy.HP -= wAbilityDamage;
                    StartCoroutine(FreezeEnemy(enemy, freezeEffectDuration));
                }

                
                StartCoroutine(ShowFreezeImage());

                WCoolDownCounter = WCoolDownDuration;
                Debug.Log("Ability used. Cooldown started.");
            }
            else
            {
                Debug.Log($"Ability not ready yet. {WCoolDownCounter:F1} seconds remaining.");
            }
        }
    }

    private IEnumerator FreezeEnemy(Enemy enemy, float duration)
    {
        float originalSpeed = enemy.speed;
        enemy.speed = 0;
        yield return new WaitForSeconds(duration);
        enemy.speed = originalSpeed;
        Debug.Log($"Enemy {enemy.name} unfrozen.");
    }

    private IEnumerator ShowFreezeImage()
    {
        if (freezeImage == null)
        {
            Debug.LogWarning("No freeze image assigned.");
            yield break;
        }

        freezeImage.SetActive(true); 
        yield return new WaitForSeconds(0.5f);
        freezeImage.SetActive(false); 
    }

    public void ShootBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(targetPosition);
            bulletScript.damage = bulletDamage; 
        }
    }
}
