using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    CharacterStats myStats;
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    const float combatCooldown = 5f;
    float lastAttackTime;
    public float attackDelay = 0.6f;

    public bool InCombat { get; private set; }
    public event System.Action OnAttack;
    private void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }
    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        if(Time.time-lastAttackTime>combatCooldown)
        {
            InCombat = false;
        }
    }
    public void Attack(CharacterStats targetStats)
    {
        if(attackCooldown<0)
        {
            StartCoroutine(DoDamage(targetStats,attackDelay));
            if (OnAttack != null)
                OnAttack();
            attackCooldown = 1 / attackSpeed;
            InCombat = true;
            lastAttackTime = Time.time;
        }
    }
    IEnumerator DoDamage(CharacterStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (stats != null)
        {
            int r = Random.Range(0, 10);
            //print(myStats.name + myStats.max.GetValue()+r);
            //if(r==0)
            //    stats.TakeDamage(myStats.damage.GetValue());
            //if (r<(10-myStats.max.GetValue()))
                stats.TakeDamage(myStats.damage.GetValue());
            //else
            //    stats.Heal(myStats.damage.GetValue());
            if (stats.currentHealth <= 0)
            {
                InCombat = false;
            }
        }
    }
}
