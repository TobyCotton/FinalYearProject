using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHorseshoe : Task
{
    private Vector3 m_destination;
    private BlacksmithScript m_BlacksmithScript;
    public CreateHorseshoe(BaseAi ai)
    {
        m_Weight = 1;
        m_Task = "CreateHorseshoe";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Ore");
        m_effect = "Horseshoe";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
        m_baseAi = ai;
    }
    public CreateHorseshoe()
    {
        m_Weight = 1;
        m_Task = "CreateHorseshoe";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Ore");
        m_effect = "Horseshoe";
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
        m_BlacksmithScript.m_Horshoes++;
        m_BlacksmithScript.m_horseshoeRequested = false;
        return true;
    }
}
