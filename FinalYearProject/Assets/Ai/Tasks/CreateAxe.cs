using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAxe : Task
{
    private Vector3 m_destination;
    private BlacksmithScript m_BlacksmithScript;
    public CreateAxe(BaseAi ai)
    {
        m_Weight = 1.0f;
        m_Task = "CreateAxe";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Ore");
        m_PreRequisite.Add("Handle");
        m_effect = "Axe";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
        m_baseAi = ai;
    }
    public CreateAxe()
    {
        m_Weight = 1.0f;
        m_Task = "CreateAxe";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Ore");
        m_PreRequisite.Add("Handle");
        m_effect = "Axe";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination()
    {
        if (m_baseAi.getWork())
        {
            if (!m_BlacksmithScript)
            {
                m_BlacksmithScript = m_baseAi.getWork().GetComponent<BlacksmithScript>();
            }
            m_destination = m_baseAi.getWork().transform.position;
            return m_destination;
        }
        return Vector3.zero;
    }
    public override bool Executing()
    {
        m_BlacksmithScript.m_Axes++;
        m_BlacksmithScript.setAxeRequested(false);
        Item toRemove = null;
        Item toRemove2 = null;
        foreach (Item item in m_baseAi.getItems())
        {
            if (toRemove == null && item.m_name == "Ore")
            {
                toRemove = item;
            }
        }
        m_baseAi.getItems().Remove(toRemove);
        foreach (Item item in m_baseAi.getItems())
        {
            if (toRemove2 == null && item.m_name == "Handle")
            {
                toRemove2 = item;
            }
        }
        m_baseAi.getItems().Remove(toRemove2);
        return true;
    }
}
