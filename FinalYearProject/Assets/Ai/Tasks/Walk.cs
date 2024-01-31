using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walk : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;

    public Walk(NavMeshAgent agent, Vector3 destination)
    {
        m_Weight = 0;
        m_Task = "Walk";
        m_PreRequisite = "none";
        m_destination = destination;
        m_agent = agent;
    }
    public override void StartExecution()
    {
        m_agent.SetDestination(m_destination);
    }
    public override bool Executing()
    {
        if (!m_agent.pathPending)
        {
            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
