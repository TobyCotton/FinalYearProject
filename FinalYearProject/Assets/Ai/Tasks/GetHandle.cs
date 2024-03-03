using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetHandle : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    private WoodWorkerScript m_wittler;
    public GetHandle(NavMeshAgent agent, BaseAi ai)
    {
        m_Weight = 0;
        m_Task = "GetHandle";
        m_PreRequisite.Add("InRange");
        m_effect = "Handle";
        m_destination = Vector3.zero;
        m_agent = agent;
        m_baseAi = ai;
    }
    public GetHandle()
    {
        m_Weight = 0;
        m_Task = "GetHandle";
        m_PreRequisite.Add("InRange");
        m_effect = "Handle";
        m_destination = Vector3.zero;
        m_agent = null;
    }
    public override Vector3 getDestination()
    {
        WoodWorkerScript[] woodWorkers = Object.FindObjectsOfType<WoodWorkerScript>();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = m_agent.transform.position;
        for (int i = 0; i < woodWorkers.Length; i++)
        {
            float distance = Vector3.Distance(currentPosition, woodWorkers[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                m_destination = woodWorkers[i].transform.position;
                m_wittler = woodWorkers[i];
            }
        }
        return m_destination;
    }
    public override bool Executing()
    {
        if (m_baseAi)
        {
            if (m_wittler.m_Handles > 0)
            {
                m_baseAi.m_Items.Add(new Item("Handle", 1));
                m_wittler.m_Handles--;
            }
            else
            {
                TaskFailed();
                return false;
            }
        }
        return true;
    }
}
