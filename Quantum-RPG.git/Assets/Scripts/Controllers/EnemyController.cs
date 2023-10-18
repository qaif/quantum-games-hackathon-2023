using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius=10;
    Transform target;
    public NavMeshAgent agent;
    CharacterCombat myCombat;
    float r = 0;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        myCombat = GetComponent<CharacterCombat>();
        InvokeRepeating("CalculateR", 0f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (r<(Mathf.Pow(target.gameObject.GetComponentInChildren<SpinState>().spinStateUp,2f)))
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);
                if (distance <= agent.stoppingDistance)
                {
                    CharacterStats tagetStats = target.GetComponent<CharacterStats>();
                    if (tagetStats != null)
                    {
                        myCombat.Attack(tagetStats);
                    }
                    FaceTarget();
                }
            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    void CalculateR()
    {
        r = Random.Range(0f, 1f);
    }
}
