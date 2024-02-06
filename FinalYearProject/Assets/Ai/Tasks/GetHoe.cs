using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetHoe : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    private BaseAi m_ai;
    private BlacksmithScript m_blacksmith;
    public GetHoe(NavMeshAgent agent)
    {
        m_Weight = 0;
        m_Task = "GetHoe";
        m_PreRequisite.Add("InRange");
        m_effect = "Hoe";
        m_destination = Vector3.zero;
        m_agent = agent;
        m_ai = null;
    }
    public GetHoe()
    {
        m_Weight = 0;
        m_Task = "GetHoe";
        m_PreRequisite.Add("InRange");
        m_effect = "Hoe";
        m_destination = Vector3.zero;
        m_agent = null;
        m_ai = null;
    }
    public override Vector3 getDestination(BaseAi ai)
    {
        m_ai = ai;
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
                m_blacksmith = blacksmiths[i];
            }
        }
        return m_destination;
    }
    public override bool Executing()
    {
        if (m_ai)
        {
            m_ai.m_Items.Add(new Item("Hoe"));
            m_blacksmith.m_Hoes--;
        }
        return true; 
    }
}