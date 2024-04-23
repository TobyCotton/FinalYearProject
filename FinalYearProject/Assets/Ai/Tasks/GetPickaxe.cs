using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetPickaxe : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    private BlacksmithScript m_blacksmith;

    public GetPickaxe(NavMeshAgent agent, BaseAi ai)
    {
        m_Weight = 1.0f;
        m_Task = "GetPickaxe";
        m_PreRequisite.Add("InRange");
        m_effect = "Pickaxe";
        m_destination = Vector3.zero;
        m_agent = agent;
        m_baseAi = ai;
    }
    public GetPickaxe()
    {
        m_Weight = 1.0f;
        m_Task = "GetPickaxe";
        m_PreRequisite.Add("InRange");
        m_effect = "Pickaxe";
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
                m_blacksmith = blacksmiths[i];
            }
        }
        return m_destination;
    }
    public override bool Executing()
    {
        if (m_baseAi)
        {
            if (m_blacksmith.m_Pickaxes > 0)
            {
                m_baseAi.getItems().Add(new Item("Pickaxe", 3));
                m_blacksmith.m_Pickaxes--;
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
