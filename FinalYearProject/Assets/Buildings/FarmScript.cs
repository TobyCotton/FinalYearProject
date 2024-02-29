using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmScript : Building
{
    public float m_Timer;
    public bool m_requested = false;
    public float m_TimerReset;
    FarmerAi m_farmer= null;

    public FarmScript(int timer)
    {
        m_TimerReset = timer;
    }
    // Update is called once per frame
    private void Start()
    {
        m_Timer = m_TimerReset;
    }
    void Update()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0 && !m_requested)
        {
            if(!m_farmer)
            {
                FindFarmer();
            }
            if (m_farmer)
            {
                m_Timer = m_TimerReset;
                if (m_farmer.PriorityChecker(new HarvestWheat()))
                {
                    m_farmer.AddToTaskList(new HarvestWheat(m_farmer), 0);
                    m_farmer.SetTaskList();
                }
                else
                {
                    m_farmer.m_toDoGoals.Add(new HarvestWheat(m_farmer));
                }
                m_requested = true;
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
