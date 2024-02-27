using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class BaseAi : MonoBehaviour
{
    protected List<Task> m_availableActions = new List<Task>();
    protected List<Task> m_goals = new List<Task>();
    private List<Task> m_toDoGoals = new List<Task>();
    public NavMeshAgent m_agent;
    public Building m_work;
    protected List<Task> m_tasks = new List<Task>();
    protected List<List<Task>> m_taskListOptions = new List<List<Task>>();
    public List<Item> m_Items = new List<Item>();
    protected Vector3 m_homePosition = Vector3.zero;
    public float m_hunger;
    private bool m_skip = false;
    public BaseAi()
    {
        m_availableActions.Add(new Walk());
        m_goals.Add(new Idle());
        m_work = null;
        m_hunger = 0;
        m_goals.Add(new GetFood());
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
    protected void UpdateToDo()
    {
        m_hunger += Time.deltaTime/2;
        if(m_hunger > 60.0f)//if hungry go eat
        {
            if (m_tasks.Count != 0)
            {
                if (m_tasks[0] is GetFood) { }
                else
                {
                    bool isIn = false;
                    for (int i = 0; i < m_toDoGoals.Count; i++)
                    {
                        if (m_toDoGoals[i] is GetFood)
                        {
                            isIn = true;
                        }
                    }
                    if (!isIn)// we are hungry and no current goal for it present
                    {
                        if (PriorityChecker(new GetFood()))
                        {
                            m_tasks.Add(new GetFood(m_agent,this));
                            m_skip = true;
                        }
                        else
                        {
                            m_toDoGoals.Add(new GetFood(m_agent,this));
                        }
                    }
                }
            }
            else
            {
                m_tasks.Add(new GetFood(m_agent, this));
                m_skip = true;
            }
        }
        int taskLength = m_tasks.Count;
        if (taskLength != 0)//There is a task to execute
        {
            Task stored = null;
            for (int i = 0; i < m_toDoGoals.Count; i++)//check if task priority is greater than current executing task
            {
                if (stored == null)
                {
                    if (PriorityChecker(m_toDoGoals[i]))
                    {
                        stored = m_toDoGoals[i];
                    }
                }
                else
                {
                    if(stored.m_priority < m_toDoGoals[i].m_priority)
                    {
                        stored = m_toDoGoals[i];
                    }
                }
            }
            if(stored != null)
            {
                m_toDoGoals.Remove(stored);
                m_tasks.Add(stored);
                m_skip = true;
            }
            if (!m_skip && m_tasks[taskLength - 1].Executing())//Execute task
            {//if Finished
                m_tasks.RemoveAt(taskLength - 1);
                taskLength = m_tasks.Count;
                if (taskLength != 0)
                {
                    m_tasks[taskLength - 1].StartExecution();
                }
            }
            m_skip = false;
        }
        else
        {
            if (m_toDoGoals.Count != 0)//Do we have tasks to complete
            {
                Task stored = null;
                for (int i = 0; i < m_toDoGoals.Count; i++)
                {
                    if (stored != null)
                    {
                        if (stored.m_priority < m_toDoGoals[i].m_priority)
                        {
                            stored = m_toDoGoals[i];
                        }
                    }
                    else
                    {
                        stored = m_toDoGoals[i];
                    }
                }
                m_tasks.Add(stored);
                m_toDoGoals.Remove(stored);
            }
            else if(m_homePosition != Vector3.zero)//go home if no tasks to complete
            {
                m_tasks.Add(new Idle(m_agent, m_homePosition));
                m_tasks[0].StartExecution();
            }
        }
    }
    public void SetTaskList()//Choose the cheapest option to do primary task
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
    public bool PriorityChecker(Task a)//check if priority is greater than current task
    {
        if(m_tasks.Count == 0)
        {
            return true;
        }
        if(a.m_priority > m_tasks[0].m_priority)
        {
            if (m_tasks[0] is Idle) { }
            else
            {
                m_tasks[0].m_executionStarted = false;
                m_toDoGoals.Add(m_tasks[0]);
            }
            m_tasks.Clear();
            return true;
        }
        return false;
    }

    protected bool CheckPrerequisite(Task goal, int listIncrement)
    {
        if (goal.m_PreRequisite.Count == 0)
        {
            if (m_taskListOptions.Count == 0)
            {
                m_taskListOptions.Add(new List<Task>());
            }
            m_taskListOptions[listIncrement].Add(goal);
            return false;
        }
        return true;
    }

    protected bool CheckInventory(Task goal,int prerequisiteNumber)
    {
        for (int z = 0; z < m_Items.Count; z++)
        {
            if (goal.m_PreRequisite[prerequisiteNumber] == m_Items[z].m_name)
            {
                return true;
            }
        }
        return false;
    }

    protected void AddToList(Task goal, int listIncrement,int prerequisiteNumber)
    {
        if (prerequisiteNumber == 0)//There is a prerequisite we have to do so add the goal only do on the first time through
        {
            if (m_taskListOptions.Count == listIncrement)
            {
                m_taskListOptions.Add(new List<Task>());
            }
            m_taskListOptions[listIncrement].Add(goal);
        }
    }
}
