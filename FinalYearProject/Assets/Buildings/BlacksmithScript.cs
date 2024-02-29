using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithScript : Building
{
    public int m_Hoes;
    public int m_Horshoes;
    public int m_Chisels;
    SmithyAI m_Smithy = null;
    public bool m_hoeRequested = false;
    public bool m_horseshoeRequested = false;
    public bool m_chiselRequested = false;

    public BlacksmithScript()
    {
        m_Hoes = 3;
        m_Horshoes = 4;
        m_Chisels = 3;
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_Smithy)
        {
            FindBlacksmith();
        }
        if (m_Smithy)
        {
            if (m_Hoes < 3 && !m_hoeRequested)
            {
                if (m_Smithy.PriorityChecker(new CreateHoe()))
                {
                    m_Smithy.AddToTaskList(new CreateHoe(m_Smithy), 0);
                    m_Smithy.SetTaskList();
                }
                else
                {
                    m_Smithy.m_toDoGoals.Add(new CreateHoe(m_Smithy));
                }
                m_hoeRequested= true;
            }
            if (m_Horshoes < 4 && !m_horseshoeRequested)
            {
                if (m_Smithy.PriorityChecker(new CreateHorseshoe()))
                {
                    m_Smithy.AddToTaskList(new CreateHorseshoe(m_Smithy), 0);
                    m_Smithy.SetTaskList();
                }
                else
                {
                    m_Smithy.m_toDoGoals.Add(new CreateHorseshoe(m_Smithy));
                }
                m_horseshoeRequested= true;
            }
            if (m_Chisels < 3 && !m_chiselRequested)
            {
                if (m_Smithy.PriorityChecker(new CreateChisel()))
                {
                    m_Smithy.AddToTaskList(new CreateChisel(m_Smithy), 0);
                    m_Smithy.SetTaskList();
                }
                else
                {
                    m_Smithy.m_toDoGoals.Add(new CreateChisel(m_Smithy));
                }
                m_chiselRequested= true;
            }
        }
    }

    private void FindBlacksmith()
    {
        SmithyAI[] smithy = Object.FindObjectsOfType<SmithyAI>();

        for (int i = 0; i < smithy.Length; i++)
        {
            if (!smithy[i].m_work)
            {
                smithy[i].m_work = this;
                m_Smithy = smithy[i];
                return;
            }
        }
    }
}
