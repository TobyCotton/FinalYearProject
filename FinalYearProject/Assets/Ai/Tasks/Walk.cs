using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walk : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;

    public Walk(NavMeshAgent agent, Vector3 destination, BaseAi ai, int StoppingDistance = 6)
    {
        m_Task = "Walk";
        m_destination = destination;
        m_agent = agent;
        m_agent.stoppingDistance = StoppingDistance;
        m_Weight = m_agent.remainingDistance/10.0f;
        m_effect = "InRange";
        m_time = 0;
        m_baseAi = ai;
    }
    public Walk()
    {
        m_Weight = 1.0f;
        m_Task = "Walk";
        m_destination = Vector3.zero;
        m_effect = "InRange";
    }
    public override void StartExecution()
    {
        m_executionStarted = true;
        if(!m_baseAi.m_visible)
        {
            m_baseAi.ToggleMesh();
        }
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
        else
        {
            if (m_destination != Vector3.zero)
            {
                m_agent.SetDestination(m_destination);
            }
        }
        return false;
    }
}
