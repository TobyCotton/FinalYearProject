using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWorkerScript : Building
{
    public int m_Handles;
    WittlerAI m_Wittler = null;
    private bool m_handlesRequested = false;

    public void setHandleRequested(bool newB) { m_handlesRequested = newB; }

    public WoodWorkerScript()
    {
        m_Handles = 4;
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_Wittler)
        {
            FindWittler();
        }
        if (m_Wittler)
        {
            if (m_Handles < 4 && !m_handlesRequested)
            {
                if (m_Wittler.PriorityChecker(new CreateHandle()))
                {
                    m_Wittler.AddToTaskList(new CreateHandle(m_Wittler), 0);
                    m_Wittler.SetTaskList();
                }
                else
                {
                    m_Wittler.getToDoGoals().Add(new CreateHandle(m_Wittler));
                }
                m_handlesRequested = true;
            }
        }
    }

    private void FindWittler()
    {
        WittlerAI[] wittlers = Object.FindObjectsOfType<WittlerAI>();

        for (int i = 0; i < wittlers.Length; i++)
        {
            if (!wittlers[i].getWork())
            {
                wittlers[i].setWork(this);
                m_Wittler = wittlers[i];
                return;
            }
        }
    }
}
