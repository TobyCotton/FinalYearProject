using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestWood : Task
{
    private Vector3 m_destination;
    private ForestScript m_woods;
    public HarvestWood(BaseAi ai, ForestScript woods, Vector3 location)
    {
        m_Weight = 1.0f;
        m_Task = "HarvestWood";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Axe");
        m_effect = "WoodHarvested";
        m_priority = 2.0f;
        m_destination = location;
        m_baseAi = ai;
        m_woods = woods;
    }
    public HarvestWood()
    {
        m_Weight = 1.0f;
        m_Task = "HarvestWood";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Axe");
        m_effect = "WoodHarvested";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination()
    {
        return m_destination;
    }
    public override bool Executing()
    {
        Item toRemove = null;
        for (int i = 0; i < m_baseAi.getItems().Count; i++)
        {
            if (m_baseAi.getItems()[i].m_name == "Axe")
            {
                if (!m_baseAi.getItems()[i].ReduceDurability())
                {
                    toRemove = m_baseAi.getItems()[i];
                }
            }
        }
        if (toRemove != null)
        {
            m_baseAi.getItems().Remove(toRemove);
        }
        m_baseAi.getItems().Add(new Item("Wood", 1));
        m_baseAi.getItems().Add(new Item("Wood", 1));
        m_woods.setRequested(false);
        m_baseAi.getToDoGoals().Add(new StoreItem(m_baseAi.getItems()[m_baseAi.getItems().Count - 1], m_baseAi));
        m_baseAi.getToDoGoals().Add(new StoreItem(m_baseAi.getItems()[m_baseAi.getItems().Count - 2], m_baseAi));
        m_baseAi.setWork(null);
        return true;
    }
}
