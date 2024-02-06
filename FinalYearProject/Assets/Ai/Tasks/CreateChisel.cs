using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChisel : Task
{
    private Vector3 m_destination;
    private BlacksmithScript m_BlacksmithScript;
    public CreateChisel()
    {
        m_Weight = 1;
        m_Task = "CreateChisel";
        m_PreRequisite.Add("InRange");
        m_effect = "Chisel";
        m_priority = 2.0f;
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination(BaseAi ai)
    {
        if (ai.m_work)
        {
            m_destination = ai.m_work.transform.position;
            if(!m_BlacksmithScript)
            {
                m_BlacksmithScript = ai.m_work.GetComponent<BlacksmithScript>();
            }
            return m_destination;
        }
        return Vector3.zero;
    }
    public override bool Executing()
    {
        m_BlacksmithScript.m_Chisels++;
        return true;
    }
}
