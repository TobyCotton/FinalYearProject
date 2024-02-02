using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HarvestWheat : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    public HarvestWheat(NavMeshAgent agent)
    {
        m_agent = agent;
        m_Weight = 1;
        m_Task = "HarvestWheat";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Hoe");
        m_effect = "WheatHarvested";
        m_priority = 2.0f;
    }
    public HarvestWheat()
    {
        m_Weight = 1;
        m_Task = "HarvestWheat";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Hoe");
        m_effect = "WheatHarvested";
        m_priority = 2.0f;
    }
    public override Vector3 getDestination(BaseAi ai)
    {
        if(ai.m_work)
        {
            m_destination = ai.m_work.transform.position;
            return m_destination;
        }
        return Vector3.zero;
    }
}
