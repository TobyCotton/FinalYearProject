using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAi : BaseAi
{
    private TMP_Text m_task;
    private TMP_Text m_Goal;
    private TMP_Text m_job;
    public FarmerAi()
    {
        m_goals.Add(new HarvestWheat());
        m_availableActions.Add(new GetHoe());
    }
    private void Start()
    {
        m_job.text = "Farmer";
        FindHome();
    }

    void Update()
    {
        if (m_hunger < 100.0f)
        {
            if (m_tasks.Count > 0)
            {
                if (m_tasks[m_tasks.Count - 1].getTaskName() == "Walk")
                {
                    m_task.text = m_tasks[m_tasks.Count - 2].getTaskName();
                }
                else
                {
                    m_task.text = m_tasks[m_tasks.Count - 1].getTaskName();
                }
                m_Goal.text = m_tasks[0].getTaskName();
            }
            UpdateToDo();
            if (m_tasks.Count == 1)
            {
                if (!m_tasks[m_tasks.Count - 1].getExcuted())
                {
                    Task tempStore = m_tasks[0];
                    m_tasks.Clear();
                    AddToTaskList(tempStore, 0);
                    SetTaskList();
                }
            }
        }
        else
        {
            Destroy(m_task);
            Destroy(m_Goal);
            Destroy(m_job);
            Destroy(gameObject);
            Destroy(this);
        }
    }
    public void AddToTaskList(Task goal,int listIncrement)
    {
        List<Task[]> saveBank = new List<Task[]>();
        bool triggerSave = false;
        if (!CheckPrerequisite(goal,listIncrement))
        {
            return;
        }
        for (int i = 0; i < goal.getPrerequisite().Count; i++)
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
                if (goal.getPrerequisite()[i] == m_availableActions[j].getEffect())
                {
                    if (found >= listIncrement + 1)
                    {
                        int incrementer = 0;
                        foreach (Task[] list in saveBank)
                        {
                            m_taskListOptions.Add(FillTaskCopy(list));
                            incrementer++;
                        }
                        if (incrementer == 0)
                        {
                            m_taskListOptions.Add(FillTaskCopy(Temptasks));
                            incrementer++;
                        }
                        found = m_taskListOptions.Count - incrementer;
                        triggerSave = true;
                    }
                    switch (m_availableActions[j].getTaskName())
                    {
                        case "GetHoe":
                            for (int k = 0; k < temp; k++)
                            {
                                AddToTaskList(new GetHoe(m_agent,this), found+k);
                            }
                            break;
                        case "Walk":
                            Vector3 destination = goal.getDestination();
                            if (destination != Vector3.zero)
                            {
                                AddToTaskList(new Walk(m_agent, destination,this), found);
                            }
                            else
                            {
                                AddToTaskList(new Walk(m_agent, destination, this), found);
                                Debug.Log("Walk failed in farmer");
                            }
                            break;
                    }
                    found++;
                }
            }
            if (triggerSave)
            {
                triggerSave = false;
                foreach (List<Task> list in m_taskListOptions)
                {
                    saveBank.Add(list.ToArray());
                }
            }
        }
    }
}
