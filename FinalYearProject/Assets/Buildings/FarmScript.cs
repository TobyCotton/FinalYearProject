using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmScript : Building
{
    public float m_Timer;
    public bool m_Harvested;
    public float m_TimerReset;
    FarmerAi m_farmer= null;

    public FarmScript(int timer)
    {
        m_Timer = timer;
        m_Harvested = true;
        m_TimerReset = timer;
    }
    // Update is called once per frame
    void Update()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            if(!m_farmer)
            {
                FindFarmer();
            }
            if (m_farmer)
            {
                m_Harvested = false;
                m_Timer = m_TimerReset;
                if (m_farmer.PriorityChecker(new HarvestWheat()))
                {
                    m_farmer.AddToTaskList(new HarvestWheat(m_farmer.m_agent), 0);
                    m_farmer.SetTaskList();
                }
            }
        }
    }
    private void FindFarmer()
    {
        FarmerAi[] farmers = Object.FindObjectsOfType<FarmerAi>();

        for (int i = 0; i < farmers.Length; i++)
        {
            if (!farmers[i].m_work)
            {
                farmers[i].m_work = this;
                m_farmer = farmers[i];
                return;
            }
        }
    }
}
