using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestWheat : Task
{
    void Start()
    {
        m_Weight = 1;
        m_Task = "HarvestWheat";
        m_PreRequisite = "Hoe";
    }
}
