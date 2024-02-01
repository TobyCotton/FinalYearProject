using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle1 : Task//Currently the same as walk will edit once designated idle areas are made
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    public Idle1(NavMeshAgent agent, Vector3 destination)
    {
        m_Weight = 0;
        m_Task = "Idle1";
        m_destination = destination;
        m_agent = agent;
        m_effect = "InRange";
    }
    public Idle1()
    {
        m_Weight = 0;
        m_Task = "Idle1";
        m_destination = Vector3.zero;
        m_agent = null;
        m_effect = "InRange";
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
