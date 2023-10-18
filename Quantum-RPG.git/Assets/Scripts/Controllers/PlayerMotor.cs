using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if(target!=null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);
    }

    // Update is called once per frame

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }
    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * 0.8f;
        agent.updateRotation = false;
        target = newTarget.interactionTransform;
    }
    public void StopFollowTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
        target = null;
    }
}
