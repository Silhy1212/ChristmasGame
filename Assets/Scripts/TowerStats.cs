using UnityEngine;
using UnityEngine.UI;

public class TowerStats : MonoBehaviour
{
    public float Hp = 100f;
    public float MaxHp = 100f;
    public Image healthBar;

    public float damageMultiplier = 1f; 

    void Update()
    {
        if (Hp <= 0)
        {
            Debug.Log("Game Over. You lose.");
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage * damageMultiplier;
        healthBar.fillAmount = Hp / MaxHp;
    }
}
