using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HarvestWheat : Task
{
    private Vector3 m_destination;
    public HarvestWheat(BaseAi ai)
    {
        m_Weight = 1.0f;
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
        m_Weight = 1.0f;
        m_Task = "HarvestWheat";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Hoe");
        m_effect = "WheatHarvested";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination()
    {
        if (m_baseAi.getWork())
        {
            m_destination = m_baseAi.getWork().transform.position;
            return m_destination;
        }
        return Vector3.zero;
    }
    public override bool Executing()
    {
        Item toRemove = null;
        for(int i = 0; i < m_baseAi.getItems().Count; i++)
        {
            if (m_baseAi.getItems()[i].m_name == "Hoe")
            {
                if(!m_baseAi.getItems()[i].ReduceDurability())
                {
                    toRemove = m_baseAi.getItems()[i];
                }
            }
        }
        if (toRemove != null) 
        {
            m_baseAi.getItems().Remove(toRemove);
        }
        m_baseAi.getItems().Add(new Item("Wheat", 1));
        m_baseAi.getItems().Add(new Item("Wheat", 1));
        FarmScript farm = m_baseAi.getWork().GetComponent<FarmScript>();
        if (farm)
        {
            farm.setRequested(false);
        }
        m_baseAi.getToDoGoals().Add(new StoreItem(m_baseAi.getItems()[m_baseAi.getItems().Count - 1], m_baseAi));
        m_baseAi.getToDoGoals().Add(new StoreItem(m_baseAi.getItems()[m_baseAi.getItems().Count - 2], m_baseAi));
        return true;
    }
}
