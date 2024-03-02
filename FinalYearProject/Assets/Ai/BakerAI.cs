using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakerAI : BaseAi
{
    public BakerAI()
    {
        m_goals.Add(new CreateFood());
        m_availableActions.Add(new GetWheat());
        m_availableActions.Add(new BuyWheat());
    }
    void Start()
    {
        FindHome();
    }
    void Update()
    {
        UpdateToDo();
        if (m_tasks.Count == 1)
        {
            if (!m_tasks[0].m_executionStarted)
            {
                Task tempStore = m_tasks[0];
                m_tasks.Clear();
                AddToTaskList(tempStore, 0);
                SetTaskList();
            }
        }
    }

    public void AddToTaskList(Task goal, int listIncrement)
    {
        if (!CheckPrerequisite(goal, listIncrement))
        {
            return;
        }
        for (int i = 0; i < goal.m_PreRequisite.Count; i++)
        {
            if (CheckInventory(goal, i))
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
                        found = m_taskListOptions.Count - 1;
                    }
                    switch (m_availableActions[j].m_Task)
                    {
                        case "Walk":
                            Vector3 destination = goal.getDestination();
                            if (destination != Vector3.zero)
                            {
                                AddToTaskList(new Walk(m_agent, destination,this), found);
                            }
                            else
                            {
                                AddToTaskList(new Walk(m_agent, destination, this), found);
                                Debug.Log("Walk failed in baker");
                            }
                            break;
                        case "GetWheat":
                            for (int k = 0; k < temp; k++)
                            {
                                AddToTaskList(new GetWheat(m_agent, this), found + k);
                            }
                            break;
                        case "BuyWheat":
                            for (int k = 0; k < temp; k++)
                            {
                                AddToTaskList(new BuyWheat(m_agent, this), found + k);
                            }
                            break;
                    }
                    found++;
                }
            }
        }
    }
}
