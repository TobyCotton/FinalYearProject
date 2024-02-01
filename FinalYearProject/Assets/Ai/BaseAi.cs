using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseAi : MonoBehaviour
{
    public List<Task> m_availableActions = new List<Task>();
    public NavMeshAgent m_agent;
    public List<Task> m_tasks = new List<Task>();
    protected List<List<Task>> m_taskListOptions = new List<List<Task>>();
    protected List<Item> m_Items = new List<Item>();
    protected bool m_AllConditionsMet;
    public BaseAi() 
    {
        m_AllConditionsMet = false;
        m_availableActions.Add(new Walk());
        m_availableActions.Add(new Idle());
        m_availableActions.Add(new Idle1());
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
            //m_tasks.Add(new Idle(m_agent, new Vector3(Random.Range(1, 20), 0, Random.Range(1, 20))));
            //m_tasks[m_tasks.Count - 1].StartExecution();
        }
    }
    protected void SetTaskList()
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
}
