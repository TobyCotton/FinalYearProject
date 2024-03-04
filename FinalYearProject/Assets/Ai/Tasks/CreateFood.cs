using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CreateFood : Task
{
    private Vector3 m_destination;
    private BakeryScript m_BakeryScript;

    public CreateFood(BaseAi ai,float priority = 2.0f)
    {
        m_Weight = 1;
        m_Task = "CreateFood";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Wheat");
        m_effect = "Food";
        m_priority = priority;
        m_destination = Vector3.zero;
        m_baseAi = ai;
    }
    public CreateFood()
    {
        m_Weight = 1;
        m_Task = "CreateFood";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Wheat");
        m_effect = "Food";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination()
    {
        if (m_baseAi.m_work)
        {
            if (!m_BakeryScript)
            {
                m_BakeryScript = m_baseAi.m_work.GetComponent<BakeryScript>();
            }
            m_destination = m_baseAi.m_work.transform.position;
            return m_destination;
        }
        return Vector3.zero;
    }
    public override bool Executing()
    {
        m_BakeryScript.m_requested = false;
        m_BakeryScript.m_foodCount++;
        Item toRemove = null;
        foreach(Item item in m_baseAi.m_Items)
        {
            if(item.m_name == "Wheat")
            {
                toRemove = item;
            }
        }
        if(toRemove != null)
        {
            m_baseAi.m_Items.Remove(toRemove);
        }
        return true;
    }
}
