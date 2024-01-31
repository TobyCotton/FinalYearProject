using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetHoe : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    public GetHoe(NavMeshAgent agent)
    {
        m_Weight = 0;
        m_Task = "GetHoe";
        m_PreRequisite.Add("InRange");
        m_effect = "Hoe";
        m_destination = Vector3.zero;
        m_agent = agent;
    }
    public GetHoe()
    {
        m_Weight = 0;
        m_Task = "GetHoe";
        m_PreRequisite.Add("InRange");
        m_effect = "Hoe";
        m_destination = Vector3.zero;
        m_agent = null;
    }
    public override Vector3 getDestination()
    {
        BlacksmithScript[] blacksmiths = Object.FindObjectsOfType<BlacksmithScript>();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = m_agent.transform.position;
        for (int i = 0; i < blacksmiths.Length; i++)
        {
            float distance = Vector3.Distance(currentPosition, blacksmiths[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                m_destination = blacksmiths[i].transform.position;
            }
        }
        return m_destination;
    }
}