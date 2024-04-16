using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestOre : Task
{
    private Vector3 m_destination;
    MineScript m_mine;
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
        for (int i = 0; i < m_baseAi.m_Items.Count; i++)
        {
            if (m_baseAi.m_Items[i].m_name == "Pickaxe")
            {
                if (!m_baseAi.m_Items[i].ReduceDurability())
                {
                    toRemove = m_baseAi.m_Items[i];
                }
            }
        }
        if (toRemove != null)
        {
            m_baseAi.m_Items.Remove(toRemove);
        }
        m_baseAi.m_Items.Add(new Item("Ore", 1));
        m_baseAi.m_Items.Add(new Item("Ore", 1));
        m_mine.m_requested = false;
        m_baseAi.m_toDoGoals.Add(new StoreItem(m_baseAi.m_Items[m_baseAi.m_Items.Count - 1], m_baseAi));
        m_baseAi.m_toDoGoals.Add(new StoreItem(m_baseAi.m_Items[m_baseAi.m_Items.Count - 2], m_baseAi));
        m_baseAi.m_work = null;
        return true;
    }
}
