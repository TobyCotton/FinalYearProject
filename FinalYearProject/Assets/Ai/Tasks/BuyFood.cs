using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuyFood : Task
{
    private Vector3 m_destination;
    private MarketScript m_market;
    public BuyFood(BaseAi ai)
    {
        m_Weight = 500.0f;
        m_Task = "BuyFood";
        m_PreRequisite.Add("InRange");
        m_effect = "Food";
        m_destination = Vector3.zero;
        m_baseAi = ai;
        m_priority = 6;
    }
    public BuyFood()
    {
        m_Weight = 500.0f;
        m_Task = "BuyFood";
        m_PreRequisite.Add("InRange");
        m_effect = "Food";
        m_destination = Vector3.zero;
        m_priority = 6;
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
            m_Weight = 10000.0f;
        }
        else
        {
            if (m_market.m_money <= 1)
            {
                TaskFailed();
                m_Weight = 10000.0f;
            }
            else
            {
                m_Weight = 10.0f / m_market.m_money;
            }
        }
        return m_destination;
    }
    public override bool Executing()
    {
        if(m_market.m_money <= 0)
        {
            TaskFailed();
            return false;
        }
        m_market.m_money -= 1;
        m_baseAi.setHunger(0.0f);
        return true;
    }
}
