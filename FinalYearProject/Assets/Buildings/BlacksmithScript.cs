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
            if (m_Hoes < 3)
            {
                if (m_Smithy.PriorityChecker(new CreateHoe()))
                {
                    m_Smithy.AddToTaskList(new CreateHoe(), 0);
                    m_Smithy.SetTaskList();
                }
            }
            if (m_Horshoes < 4)
            {
                if (m_Smithy.PriorityChecker(new CreateHorseshoe()))
                {
                    m_Smithy.AddToTaskList(new CreateHorseshoe(), 0);
                    m_Smithy.SetTaskList();
                }
            }
            if (m_Chisels < 3)
            {
                if (m_Smithy.PriorityChecker(new CreateChisel()))
                {
                    m_Smithy.AddToTaskList(new CreateChisel(), 0);
                    m_Smithy.SetTaskList();
                }
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
