using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryScript : Building
{
    public int m_foodCount;
    private int m_IdealFoodCount;
    private BakerAI m_Baker;
    public bool m_requested = false;
    public BakeryScript()
    {
        m_IdealFoodCount = 9;
    }
    private void Start()
    {
        m_foodCount = m_IdealFoodCount;
    }

    void Update()
    {
        if(!m_Baker)
        {
            FindBaker();
        }
        if(m_Baker)
        {
            if((m_foodCount < m_IdealFoodCount && !m_requested) || m_foodCount == 0)
            {
                float priority = 2.0f;
                if(m_foodCount == 0)
                {
                    priority = 500.0f;
                }
                if (m_Baker.PriorityChecker(new CreateFood(m_Baker,priority)))
                {
                    m_Baker.AddToTaskList(new CreateFood(m_Baker,priority), 0);
                    m_Baker.SetTaskList();
                }
                else
                {
                    m_Baker.m_toDoGoals.Add(new CreateFood(m_Baker));
                }
                m_requested = true;
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
