using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuyOre : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    private MarketScript m_market;
    public BuyOre(NavMeshAgent agent, BaseAi ai)
    {
        m_Weight = 500;
        m_Task = "BuyOre";
        m_PreRequisite.Add("InRange");
        m_effect = "Ore";
        m_destination = Vector3.zero;
        m_agent = agent;
        m_baseAi = ai;
    }
    public BuyOre()
    {
        m_Weight = 500;
        m_Task = "BuyOre";
        m_PreRequisite.Add("InRange");
        m_effect = "Ore";
        m_destination = Vector3.zero;
        m_agent = null;
    }
    public override Vector3 getDestination()
    {
        MarketScript[] markets = Object.FindObjectsOfType<MarketScript>();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = m_baseAi.m_agent.transform.position;
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
        return m_destination;
    }
    public override bool Executing()
    {
        m_baseAi.m_Items.Add(new Item("Ore", 1));
        return true;
    }
}
