using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HarvestWheat : Task
{
    private Vector3 m_destination;
    public HarvestWheat(BaseAi ai)
    {
        m_Weight = 1;
        m_Task = "HarvestWheat";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Hoe");
        m_effect = "WheatHarvested";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
        m_baseAi = ai;
    }
    public HarvestWheat()
    {
        m_Weight = 1;
        m_Task = "HarvestWheat";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Hoe");
        m_effect = "WheatHarvested";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination()
    {
        if (m_baseAi.m_work)
        {
            m_destination = m_baseAi.m_work.transform.position;
            return m_destination;
        }
        return Vector3.zero;
    }
    public override bool Executing()
    {
        for(int i = 0; i < m_baseAi.m_Items.Count; i++)
        {
            if (m_baseAi.m_Items[i].m_name == "Hoe")
            {
                if(!m_baseAi.m_Items[i].ReduceDurability())
                {
                    m_baseAi.m_Items.Remove(m_baseAi.m_Items[i]);
                }
            }
        }
        m_baseAi.m_Items.Add(new Item("Wheat", 1));
        return true;
    }
}
