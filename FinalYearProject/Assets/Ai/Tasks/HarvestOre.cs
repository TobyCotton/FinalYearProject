using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestOre : Task
{
    private Vector3 m_destination;
    private MineScript m_mine;
    public HarvestOre(BaseAi ai,MineScript mine,Vector3 location)
    {
        m_Weight = 1.0f;
        m_Task = "HarvestOre";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Pickaxe");
        m_effect = "OreHarvested";
        m_priority = 2.0f;
        m_destination = location;
        m_baseAi = ai;
        m_mine = mine;
    }
    public HarvestOre()
    {
        m_Weight = 1.0f;
        m_Task = "HarvestOre";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Pickaxe");
        m_effect = "OreHarvested";
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
            if (m_baseAi.getItems()[i].m_name == "Pickaxe")
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
        m_baseAi.getItems().Add(new Item("Ore", 1));
        m_baseAi.getItems().Add(new Item("Ore", 1));
        m_mine.setRequested(false);
        m_baseAi.getToDoGoals().Add(new StoreItem(m_baseAi.getItems()[m_baseAi.getItems().Count - 1], m_baseAi));
        m_baseAi.getToDoGoals().Add(new StoreItem(m_baseAi.getItems()[m_baseAi.getItems().Count - 2], m_baseAi));
        m_baseAi.setWork(null);
        return true;
    }
}
