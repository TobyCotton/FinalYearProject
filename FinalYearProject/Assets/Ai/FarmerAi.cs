using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAi : BaseAi
{
    public FarmerAi()
    {
        m_goals.Add(new HarvestWheat());
        m_availableActions.Add(new GetHoe());
    }
    private void Start()
    {
        FindHome();
    }

    void Update()
    {
        UpdateToDo();
        if (m_tasks.Count == 1)
        {
            if (!m_tasks[m_tasks.Count-1].m_executionStarted)
            {
                Task tempStore = m_tasks[0];
                m_tasks.Clear();
                AddToTaskList(tempStore, 0);
                SetTaskList();
            }
        }
    }
    public void AddToTaskList(Task goal,int listIncrement)
    {
        if(!CheckPrerequisite(goal,listIncrement))
        {
            return;
        }
        for (int i = 0; i < goal.m_PreRequisite.Count; i++)
        {
            if(CheckInventory(goal,i))
            {
                return;
            }
            AddToList(goal, listIncrement, i);
            int temp = m_taskListOptions.Count;
            int found = listIncrement;
            Task[] Temptasks = m_taskListOptions[found].ToArray();
            for (int j = 0; j < m_availableActions.Count; j++)
            {
                if (goal.m_PreRequisite[i] == m_availableActions[j].m_effect)
                {
                    if (found >= listIncrement + 1)
                    {
                        m_taskListOptions.Add(FillTaskCopy(Temptasks));
                        found = m_taskListOptions.Count-1;
                    }
                    switch (m_availableActions[j].m_Task)
                    {
                        case "GetHoe":
                            for (int k = 0; k < temp; k++)
                            {
                                AddToTaskList(new GetHoe(m_agent), found+k);
                            }
                            break;
                        case "Walk":
                            Vector3 destination = goal.getDestination(this);
                            if (destination != Vector3.zero)
                            {
                                AddToTaskList(new Walk(m_agent, destination), found);
                            }
                            else
                            {
                                Debug.Log("Walk failed in farmer");
                            }
                            break;
                    }
                    found++;
                }
            }
        }
    }
}
