using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    public NavMeshAgent m_agent;
    private List<Task> m_tasks;
    void Start()
    {
        m_tasks = new List<Task>();
        for(int i = 0;i < 5;i++)
        {
            if (m_agent != null)
            {
                m_tasks.Add(new Walk(m_agent, new Vector3(Random.Range(1, 40), 0, Random.Range(1, 40))));
            }
        }
        if (m_agent != null)
        {
            m_tasks[m_tasks.Count - 1].StartExecution();
        }
    }
    // Update is called once per frame
    void Update()
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
            m_tasks.Add(new Idle(m_agent, new Vector3(Random.Range(1, 20), 0, Random.Range(1, 20))));
            m_tasks[m_tasks.Count - 1].StartExecution();
        }
    }
}
