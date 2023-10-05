using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth{get; private set;}
    public Stat damage;
    public Stat armour;
    public Stat max;
    public Stat heal;
    public event System.Action<int, int> OnHealthChanged;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    TakeDamage(10);
        //}
    }
    public void TakeDamage(int damage)
    {
        
        damage -= armour.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");
        if (OnHealthChanged!=null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, -1, maxHealth);
        Debug.Log(transform.name + " takes " + heal + " heal.");

        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }
    }
    public virtual void Die()
    {
        Debug.Log(transform.name + " died.");
    }

}
