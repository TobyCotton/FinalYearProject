using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithScript : Building
{
    public int m_Hoes;
    public int m_Chisels;
    public int m_Pickaxes;
    public int m_Axes;
    private SmithyAI m_Smithy = null;
    private bool m_pickaxeRequested = false;
    private bool m_hoeRequested = false;
    private bool m_chiselRequested = false;
    private bool m_axesRequested = false;

    public void setPickaxeRequested(bool newB) { m_pickaxeRequested = newB; }
    public void setHoeRequested(bool newB) { m_hoeRequested = newB; }
    public void setChiselRequested(bool newB) { m_chiselRequested = newB; }
    public void setAxeRequested(bool newB) { m_axesRequested = newB; }
    public BlacksmithScript()
    {
        m_Pickaxes = 3;
        m_Hoes = 3;
        m_Chisels = 3;
        m_Axes = 3;
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_Smithy)//Null check
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
                    m_Smithy.getToDoGoals().Add(new CreateHoe(m_Smithy));
                }
                m_hoeRequested= true;
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
                    m_Smithy.getToDoGoals().Add(new CreateChisel(m_Smithy));
                }
                m_chiselRequested= true;
            }
            if (m_Pickaxes < 3 && !m_pickaxeRequested)
            {
                if (m_Smithy.PriorityChecker(new CreatePickaxe()))
                {
                    m_Smithy.AddToTaskList(new CreatePickaxe(m_Smithy), 0);
                    m_Smithy.SetTaskList();
                }
                else
                {
                    m_Smithy.getToDoGoals().Add(new CreatePickaxe(m_Smithy));
                }
                m_pickaxeRequested = true;
            }
            if (m_Axes < 3 && !m_axesRequested)
            {
                if (m_Smithy.PriorityChecker(new CreateAxe()))
                {
                    m_Smithy.AddToTaskList(new CreateAxe(m_Smithy), 0);
                    m_Smithy.SetTaskList();
                }
                else
                {
                    m_Smithy.getToDoGoals().Add(new CreateAxe(m_Smithy));
                }
                m_axesRequested = true;
            }
        }
    }

    private void FindBlacksmith()
    {
        SmithyAI[] smithy = Object.FindObjectsOfType<SmithyAI>();

        for (int i = 0; i < smithy.Length; i++)
        {
            if (!smithy[i].getWork())
            {
                smithy[i].setWork(this);
                m_Smithy = smithy[i];
                return;
            }
        }
    }
}
