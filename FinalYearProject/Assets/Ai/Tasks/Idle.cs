using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : Task//Currently the same as walk will edit once designated idle areas are made
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    public Idle(NavMeshAgent agent, Vector3 destination, BaseAi ai, int StoppingDistance = 6)
    {
        m_Weight = 2.0f;
        m_Task = "Idle";
        m_destination = destination;
        m_agent = agent;
        m_effect = "InRange";
        m_priority = 0;
        m_agent.stoppingDistance = StoppingDistance;
        m_time = 0;
        m_baseAi = ai;
    }
    public Idle()
    {
        m_Weight = 2.0f;
        m_Task = "Idle";
        m_destination = Vector3.zero;
        m_agent = null;
        m_effect = "InRange";
        m_priority = 0;
    }
    public override void StartExecution()
    {
        m_executionStarted = true;
        if (!m_baseAi.getVisible() && !m_baseAi.getMultipleIdle())
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
        return false;
    }
}
