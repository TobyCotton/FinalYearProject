using System.Collections;
using System.Collections.Generic;
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
        AddToTaskList(new HarvestWheat(m_agent));
        m_tasks[m_tasks.Count - 1].StartExecution();
    }
    void Update()
    {
        base.Update();
    }
    public void AddToTaskList(Task goal)
    {
        if (goal.m_PreRequisite.Count == 0)
        {
            m_tasks.Add(goal);
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
            if(i == 0)//There is a prerequisite we have to do so add the goal only do on the first time through
            {
                m_tasks.Add(goal);
            }
            for (int j = 0; j < m_availableActions.Count; j++)
            {
                if (goal.m_PreRequisite[i] == m_availableActions[j].m_effect)
                {
                    switch (m_availableActions[j].m_Task)
                    {
                        case "GetHoe":
                            AddToTaskList(new GetHoe(m_agent));
                            break;
                        case "HarvestWheat":
                            AddToTaskList(new HarvestWheat(m_agent));
                            break;
                        case "Walk":
                            Vector3 destination = goal.getDestination();
                            if (destination != Vector3.zero)
                            {
                                AddToTaskList(new Walk(m_agent,destination));
                            }
                            else
                            {
                                Debug.Log("Walk failed in farmer");
                            }
                            break;
                    }
                }
            }
        }
    }
    public bool CheckHoe() { return m_HasHoe; }
}
