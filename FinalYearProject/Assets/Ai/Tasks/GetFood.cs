using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetFood : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    private BakeryScript m_bakery;

    public GetFood(NavMeshAgent agent, BaseAi ai)
    {
        m_Weight = 1.0f;
        m_Task = "GetFood";
        m_PreRequisite.Add("InRange");
        m_effect = "Full";
        m_destination = Vector3.zero;
        m_agent = agent;
        m_priority = 5;
        m_baseAi = ai;
    }
    public GetFood() 
    {
        m_Weight = 1.0f;
        m_Task = "GetFood";
        m_PreRequisite.Add("InRange");
        m_effect = "Full";
        m_destination = Vector3.zero;
        m_agent = null;
        m_priority = 5;
    }

    public override Vector3 getDestination()
    {
        BakeryScript[] bakerys = Object.FindObjectsOfType<BakeryScript>();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = m_agent.transform.position;
        for (int i = 0; i < bakerys.Length; i++)
        {
            float distance = Vector3.Distance(currentPosition, bakerys[i].transform.position);
            if (distance < shortestDistance && bakerys[i].m_foodCount > 0)
            {
                shortestDistance = distance;
                m_destination = bakerys[i].transform.position;
                m_bakery = bakerys[i];
            }
        }
        if(m_destination ==Vector3.zero)
        {
            TaskFailed();
        }
        return m_destination;
    }

    public override bool Executing()
    {
        if (m_baseAi)
        {
            if (m_bakery.m_foodCount > 0)
            {
                m_baseAi.setHunger(0.0f);
                m_bakery.m_foodCount--;
                return true;
            }
            else
            {
                m_baseAi.getTasks().Add(new BuyFood(m_baseAi));
                m_baseAi.getTasks().RemoveAt(0);
            }
        }
        return false;
    }
}
