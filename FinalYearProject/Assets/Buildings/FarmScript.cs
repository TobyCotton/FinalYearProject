using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmScript : MonoBehaviour
{
    private float m_Timer;
    public bool m_Harvested;
    public float m_TimerReset;

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
            m_Harvested = false;
            FarmerAi[] farmers = Object.FindObjectsOfType<FarmerAi>();

            for (int i = 0; i < farmers.Length; i++)
            {
                farmers[i].m_tasks.Add(new HarvestWheat(farmers[i]));
            }
            m_Timer = m_TimerReset;
        }
    }
}
