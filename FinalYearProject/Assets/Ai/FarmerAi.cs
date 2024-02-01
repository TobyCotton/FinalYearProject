using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAi : BaseAi
{
    public bool m_HasHoe;
    private Vector3 m_destination;
    public FarmerAi()
    {
        m_HasHoe = false;
        m_destination = Vector3.zero;
        m_availableActions.Add(new HarvestWheat());
        m_availableActions.Add(new GetHoe());
    }
    private void Start()
    {
        AddToTaskList(new HarvestWheat(m_agent),0);
        SetTaskList();
        m_tasks[m_tasks.Count - 1].StartExecution();
    }
    void Update()
    {
        base.Update();
    }
    public void AddToTaskList(Task goal,int listIncrement)
    {
        if (goal.m_PreRequisite.Count == 0)
        {
            if (m_taskListOptions.Count == 0)
            {
                m_taskListOptions.Add(new List<Task>());
            }
            m_taskListOptions[listIncrement].Add(goal);
            return;
        }
        for (int i = 0; i < goal.m_PreRequisite.Count; i++)
        {
            for(int z = 0; z < m_Items.Count;z++)
            {
                if (goal.m_PreRequisite[i] == m_Items[z].m_name)
                {
                    return;
                }
            }
            if (i == 0)//There is a prerequisite we have to do so add the goal only do on the first time through
            {
                if (m_taskListOptions.Count == listIncrement)
                {
                    m_taskListOptions.Add(new List<Task>());
                }
                m_taskListOptions[listIncrement].Add(goal);
            }
            int temp = m_taskListOptions.Count;
            string lastKnown = "None";
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
                        case "HarvestWheat":
                            for (int k = 0; k < temp; k++)
                            {
                                AddToTaskList(new HarvestWheat(m_agent), found);
                            }
                            break;
                        case "Walk":
                            Vector3 destination = goal.getDestination();
                            if (destination != Vector3.zero)
                            {
                                AddToTaskList(new Walk(m_agent, destination), found);
                            }
                            else
                            {
                                Debug.Log("Walk failed in farmer");
                            }
                            break;
                        case "Idle":
                            destination = goal.getDestination();
                            if (destination != Vector3.zero)
                            {
                                AddToTaskList(new Idle(m_agent, destination), found);
                            }
                            else
                            {
                                Debug.Log("Idle failed in farmer");
                            }
                            break;
                        case "Idle1":
                            destination = goal.getDestination();
                            if (destination != Vector3.zero)
                            {
                                AddToTaskList(new Idle1(m_agent, destination), found);
                            }
                            else
                            {
                                Debug.Log("Idle1 failed in farmer");
                            }
                            break;
                    }
                    found++;
                    lastKnown = m_availableActions[j].m_Task;
                }
            }
        }
    }
    public bool CheckHoe() { return m_HasHoe; }
}
