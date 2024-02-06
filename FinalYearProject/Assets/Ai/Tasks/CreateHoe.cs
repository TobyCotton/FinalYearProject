using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHoe : Task
{
    private Vector3 m_destination;
    private BlacksmithScript m_BlacksmithScript;
    public CreateHoe()
    {
        m_Weight = 1;
        m_Task = "CreateHoe";
        m_PreRequisite.Add("InRange");
        m_effect = "Hoe";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination(BaseAi ai)
    {
        if (ai.m_work)
        {
            if (!m_BlacksmithScript)
            {
                m_BlacksmithScript = ai.m_work.GetComponent<BlacksmithScript>();
            }
            m_destination = ai.m_work.transform.position;
            return m_destination;
        }
        return Vector3.zero;
    }
    public override bool Executing()
    {
        m_BlacksmithScript.m_Hoes++;
        return true;
    }
}
