using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseAi : MonoBehaviour
{
    public List<Task> m_availableActions = new List<Task>();
    public List<Task> m_goals = new List<Task>();
    public NavMeshAgent m_agent;
    public Building m_work;
    public List<Task> m_tasks = new List<Task>();
    protected List<List<Task>> m_taskListOptions = new List<List<Task>>();
    public List<Item> m_Items = new List<Item>();
    protected bool m_AllConditionsMet;
    protected Vector3 m_homePosition = Vector3.zero;
    public BaseAi()
    {
        m_AllConditionsMet = false;
        m_availableActions.Add(new Walk());
        m_goals.Add(new Idle());
        m_work = null;
    }

    protected void FindHome()
    {
        HomeScript[] temp = Object.FindObjectsOfType<HomeScript>();
        for (int i = 0; i<temp.Length; i++)
        {
            if (!temp[i].m_assignedAi)
            {
                temp[i].m_assignedAi = this;
                m_homePosition = temp[i].transform.position;
                i = temp.Length;
            }
        }
    }
    // Update is called once per frame
    protected void Update()
    {
        int taskLength = m_tasks.Count;
        if (taskLength != 0)
        {
            if (m_tasks[taskLength - 1].Executing())
            {
                m_tasks.RemoveAt(taskLength - 1);
                taskLength = m_tasks.Count;
                if (taskLength != 0)
                {
                    m_tasks[taskLength - 1].StartExecution();
                }
            }
        }
        else
        {
            if(m_homePosition != Vector3.zero)
            {
                m_tasks.Add(new Idle(m_agent, m_homePosition));
                m_tasks[0].StartExecution();
            }
        }
    }
    public void SetTaskList()
    {
        float lowestWeight = Mathf.Infinity;
        List<Task> tempTasks = new List<Task>();
        for (int i = 0;i < m_taskListOptions.Count; i++)
        {
            float currentWeight = 0;
            for(int j = 0;j < m_taskListOptions[i].Count;j++)
            {
                currentWeight += m_taskListOptions[i][j].m_Weight;
            }
            if (currentWeight < lowestWeight) 
            {
                lowestWeight = currentWeight;
                tempTasks = m_taskListOptions[i];
            }
        }
        m_tasks.Clear();
        for (int i = 0; i < tempTasks.Count; i++) 
        {
            m_tasks.Add(tempTasks[i]);
        }
        m_tasks[m_tasks.Count - 1].StartExecution();
        m_taskListOptions.Clear();
    }
    protected List<Task> FillTaskCopy(Task[] toCopy)
    {
        List<Task> temp = new List<Task>();
        for(int i = 0;i < toCopy.Length; i++)
        {
            temp.Add(toCopy[i]);
        }
        return temp;
    }
    public bool PriorityChecker(Task a)
    {
        if(m_tasks.Count == 0)
        {
            return true;
        }
        if(a.m_priority > m_tasks[0].m_priority)
        {
            return true;
        }
        return false;
    }
}
