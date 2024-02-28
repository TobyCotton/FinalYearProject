using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmithyAI : BaseAi
{
    public SmithyAI()
    {
        m_goals.Add(new CreateHoe());
        m_goals.Add(new CreateHorseshoe());
        m_goals.Add(new CreateChisel());
    }
    // Start is called before the first frame update
    void Start()
    {
        FindHome();
    }

    // Update is called once per frame
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
