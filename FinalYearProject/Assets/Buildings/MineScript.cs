using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : Building
{
    public float m_Timer;
    private bool m_requested = false;
    public float m_TimerReset;
    AdventurerAI m_adventurer = null;

    public void setRequested(bool newB) { m_requested = newB; }
    public MineScript(float timer) 
    {
        m_TimerReset = timer;
    }
    private void Start()
    {
        m_Timer = m_TimerReset;
    }
    void Update()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0 && !m_requested)
        {
            if (!m_adventurer)//Null check
            {
                FindAdventurer();
            }
            if (m_adventurer)
            {
                m_Timer = m_TimerReset;
                if (m_adventurer.PriorityChecker(new HarvestOre()))
                {
                    m_adventurer.AddToTaskList(new HarvestOre(m_adventurer,this,transform.position), 0);
                    m_adventurer.SetTaskList();
                }
                else
                {
                    m_adventurer.getToDoGoals().Add(new HarvestOre(m_adventurer,this, transform.position));
                }
                m_requested = true;
            }
        }
    }
    private void FindAdventurer()
    {
        AdventurerAI[] adventurers = Object.FindObjectsOfType<AdventurerAI>();

        for (int i = 0; i < adventurers.Length; i++)
        {
            if (adventurers[i].getWork() == null)
            {
                adventurers[i].setWork(this);
                m_adventurer = adventurers[i];
                return;
            }
        }
    }
}
