using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryScript : Building
{
    public int m_foodCount;
    private int m_IdealFoodCount;
    private BakerAI m_Baker;
    public BakeryScript()
    {
        m_foodCount = 6;
        m_IdealFoodCount = 5;
    }

    void Update()
    {
        if(!m_Baker)
        {
            FindBaker();
        }
        if(m_Baker)
        {
            if(m_foodCount < m_IdealFoodCount)
            {
                if (m_Baker.PriorityChecker(new CreateFood()))
                {
                    m_Baker.AddToTaskList(new CreateFood(), 0);
                    m_Baker.SetTaskList();
                }
            }
        }
    }
    private void FindBaker()
    {
        BakerAI[] baker = Object.FindObjectsOfType<BakerAI>();

        for (int i = 0; i < baker.Length; i++)
        {
            if (!baker[i].m_work)
            {
                baker[i].m_work = this;
                m_Baker = baker[i];
                return;
            }
        }
    }
}
