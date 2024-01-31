using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAi : MonoBehaviour
{
    public List<Task> m_availableActions = new List<Task>();
    public NavMeshAgent m_agent;
    public List<Task> m_tasks = new List<Task>();
    protected List<Item> m_Items = new List<Item>();
    protected bool m_AllConditionsMet;
    public BaseAi() 
    {
        m_AllConditionsMet = false;
        m_availableActions.Add(new Walk());
        m_availableActions.Add(new Idle());
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
}
