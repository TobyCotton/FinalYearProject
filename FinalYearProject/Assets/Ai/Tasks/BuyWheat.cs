using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuyWheat : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    private MarketScript m_market;
    public BuyWheat(NavMeshAgent agent, BaseAi ai)
    {
        m_Weight = 500.0f;
        m_Task = "BuyWheat";
        m_PreRequisite.Add("InRange");
        m_effect = "Wheat";
        m_destination = Vector3.zero;
        m_agent = agent;
        m_baseAi = ai;
    }
    public BuyWheat()
    {
        m_Weight = 500.0f;
        m_Task = "BuyWheat";
        m_PreRequisite.Add("InRange");
        m_effect = "Wheat";
        m_destination = Vector3.zero;
        m_agent = null;
    }
    public override Vector3 getDestination()
    {
        MarketScript[] markets = Object.FindObjectsOfType<MarketScript>();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = m_baseAi.getNavAgent().transform.position;
        m_destination = Vector3.zero;
        for (int i = 0; i < markets.Length; i++)
        {
            float distance = Vector3.Distance(currentPosition, markets[i].transform.position);
            if (distance < shortestDistance)
            {
                m_market = markets[i];
                shortestDistance = distance;
                m_destination = markets[i].transform.position;
            }
        }

        if (m_destination == Vector3.zero)
        {
            TaskFailed();
        }
        else
        {
            if (m_market.m_money <= 1)
            {
                TaskFailed();
            }
            m_Weight = 10.0f / m_market.m_money;
        }
        return m_destination;
    }
    public override bool Executing()
    {
        m_market.m_money -= 2;
        m_baseAi.getItems().Add(new Item("Wheat", 1));
        return true;
    }
}
