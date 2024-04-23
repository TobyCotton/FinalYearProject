using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChisel : Task
{
    private Vector3 m_destination;
    private BlacksmithScript m_BlacksmithScript;
    public CreateChisel(BaseAi ai)
    {
        m_Weight = 1.0f;
        m_Task = "CreateChisel";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Ore");
        m_PreRequisite.Add("Handle");
        m_effect = "Chisel";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
        m_baseAi = ai;
    }
    public CreateChisel()
    {
        m_Weight = 1.0f;
        m_Task = "CreateChisel";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Ore");
        m_PreRequisite.Add("Handle");
        m_effect = "Chisel";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination()
    {
        if (m_baseAi.getWork())
        {
            m_destination = m_baseAi.getWork().transform.position;
            if(!m_BlacksmithScript)
            {
                m_BlacksmithScript = m_baseAi.getWork().GetComponent<BlacksmithScript>();
            }
            return m_destination;
        }
        return Vector3.zero;
    }
    public override bool Executing()
    {
        m_BlacksmithScript.m_Chisels++;
        m_BlacksmithScript.setChiselRequested(false);
        Item toRemove = null;
        Item toRemove2 = null;
        foreach (Item item in m_baseAi.getItems())
        {
            if (toRemove == null && item.m_name == "Ore")
            {
                toRemove = item;
            }
            if (toRemove2 == null && item.m_name == "Handle")
            {
                toRemove2 = item;
            }
        }
        m_baseAi.getItems().Remove(toRemove);
        m_baseAi.getItems().Remove(toRemove2);
        return true;
    }
}
