using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePickaxe : Task
{
    private Vector3 m_destination;
    private BlacksmithScript m_BlacksmithScript;
    public CreatePickaxe(BaseAi ai)
    {
        m_Weight = 1;
        m_Task = "CreatePickaxe";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Ore");
        m_PreRequisite.Add("Wood");
        m_effect = "Pickaxe";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
        m_baseAi = ai;
    }
    public CreatePickaxe()
    {
        m_Weight = 1;
        m_Task = "CreatePickaxe";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Ore");
        m_effect = "Pickaxe";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination()
    {
        if (m_baseAi.m_work)
        {
            if (!m_BlacksmithScript)
            {
                m_BlacksmithScript = m_baseAi.m_work.GetComponent<BlacksmithScript>();
            }
            m_destination = m_baseAi.m_work.transform.position;
            return m_destination;
        }
        return Vector3.zero;
    }
    public override bool Executing()
    {
        m_BlacksmithScript.m_Pickaxes++;
        m_BlacksmithScript.m_pickaxeRequested = false;
        Item toRemove = null;
        Item toRemove2 = null;
        foreach (Item item in m_baseAi.m_Items)
        {
            if (toRemove == null && item.m_name == "Ore")
            {
                toRemove = item;
            }
            if (toRemove2 == null && item.m_name == "Wood")
            {
                toRemove = item;
            }
        }
        m_baseAi.m_Items.Remove(toRemove);
        m_baseAi.m_Items.Remove(toRemove2);
        return true;
    }
}
