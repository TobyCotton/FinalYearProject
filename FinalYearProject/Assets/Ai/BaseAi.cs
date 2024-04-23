using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class BaseAi : MonoBehaviour
{
    protected List<Task> m_availableActions = new List<Task>();
    protected List<Task> m_goals = new List<Task>();
    protected List<Task> m_toDoGoals = new List<Task>();
    protected NavMeshAgent m_agent;
    protected Building m_work;
    protected List<Task> m_tasks = new List<Task>();
    protected List<List<Task>> m_taskListOptions = new List<List<Task>>();
    protected List<Item> m_Items = new List<Item>();
    protected Vector3 m_homePosition = Vector3.zero;
    protected float m_hunger;
    private bool m_skip = false;
    protected bool m_visible = true;
    protected bool m_multipleIdle = false;

    public bool getMultipleIdle() { return m_multipleIdle; }

    public bool getVisible() { return m_visible; }
    public void setHunger(float newV) { m_hunger = newV; }
    public List<Item> getItems() { return m_Items; }
    public List<Task> getTasks() { return m_tasks; }
    public Building getWork() { return m_work; }
    public void setWork(Building x) { m_work = x; }
    public NavMeshAgent getNavAgent() { return m_agent; }
    public List<Task> getToDoGoals() { return m_toDoGoals; }
    public BaseAi()
    {
        m_availableActions.Add(new Walk());
        m_goals.Add(new Idle());
        m_goals.Add(new StoreItem());
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
        foreach(Task task in m_toDoGoals)
        {
            task.UpdatecoolDownPeriod();
        }
        m_hunger += Time.deltaTime/2;
        if(m_hunger > 60.0f)//if hungry go eat
        {
            if (m_tasks.Count != 0)
            {
                if (m_tasks[0] is GetFood || m_tasks[0] is BuyFood) { }
                else
                {
                    bool isIn = false;
                    for (int i = 0; i < m_toDoGoals.Count; i++)
                    {
                        if (m_toDoGoals[i] is GetFood || m_toDoGoals[i] is BuyFood)
                        {
                            isIn = true;
                        }
                    }
                    if (!isIn)// we are hungry and no current goal for it present
                    {
                        if (PriorityChecker(new GetFood()))
                        {
                            Task a = new GetFood(m_agent, this);
                            a.getDestination();
                            Task b = new BuyFood(this);
                            b.getDestination();
                            if (WeightChecker(a,b) && !a.getFailed())
                            {
                                m_tasks.Add(new GetFood(m_agent, this));
                                m_skip = true;
                            }
                            else if(!b.getFailed())
                            {
                                m_tasks.Add(new BuyFood(this));
                                m_skip = true;
                            }
                        }
                        else
                        {
                            Task a = new GetFood(m_agent, this);
                            a.getDestination();
                            Task b = new BuyFood(this);
                            b.getDestination();
                            if (WeightChecker(a, b) && !a.getFailed())
                            {
                                m_toDoGoals.Add(new GetFood(m_agent, this));
                            }
                            else if (!b.getFailed())
                            {
                                m_toDoGoals.Add(new BuyFood(this));
                            }
                        }
                    }
                }
            }
            else
            {
                Task a = new GetFood(m_agent, this);
                a.getDestination();
                Task b = new BuyFood(this);
                b.getDestination();
                if (WeightChecker(a, b) && !a.getFailed())
                {
                    m_tasks.Add(new GetFood(m_agent, this));
                    m_skip = true;
                }
                else if (!b.getFailed())
                {
                    m_tasks.Add(new BuyFood(this));
                    m_skip = true;
                }
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
                    if(stored.getPriority() < m_toDoGoals[i].getPriority())
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
            if (!m_skip)//Don't skip task
            {
                m_tasks[m_tasks.Count-1].setTime(m_tasks[m_tasks.Count - 1].getTime() - Time.deltaTime);
                if (m_tasks[m_tasks.Count - 1].getTime() <= 0.0f && m_tasks[taskLength - 1].Executing())
                {
                    if (m_tasks[taskLength - 1] is Idle)
                    {
                        if(m_visible)
                        {
                            ToggleMesh();
                        }
                        m_multipleIdle = true;
                    }
                    m_tasks.RemoveAt(taskLength - 1);
                    taskLength = m_tasks.Count;
                    if (taskLength != 0)
                    {
                        m_tasks[taskLength - 1].StartExecution();
                    }
                }
                else
                {
                    if(m_tasks[taskLength - 1].getFailed())
                    {
                        m_tasks[0].TaskFailed();
                        m_tasks[0].setWeight(false);
                        m_toDoGoals.Add(m_tasks[0]);
                        m_tasks.Clear();
                    }
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
                        if (stored.getPriority() < m_toDoGoals[i].getPriority())
                        {
                            stored = m_toDoGoals[i];
                        }
                    }
                    else
                    {
                        if (m_toDoGoals[i].getCooldown() <= 0)
                        {
                            stored = m_toDoGoals[i];
                        }
                    }
                }
                if (stored != null)
                {
                    m_tasks.Add(stored);
                    m_toDoGoals.Remove(stored);
                    return;
                }
            }
            if(m_homePosition != Vector3.zero)//go home if no tasks to complete
            {
                m_tasks.Add(new Idle(m_agent, m_homePosition, this));
                m_tasks[0].StartExecution();
            }
        }
    }
    public void SetTaskList()//Choose the cheapest option to do primary task
    {
        float lowestWeight = Mathf.Infinity;
        List<Task> tempTasks = new List<Task>();
        bool failedTask = false;
        for (int i = 0;i < m_taskListOptions.Count; i++)
        {
            failedTask = false;
            float currentWeight = 0;
            for(int j = 0;j < m_taskListOptions[i].Count;j++)
            {
                if (m_taskListOptions[i][j].getFailed())
                {
                    failedTask = true;
                }
                currentWeight += m_taskListOptions[i][j].getWeight();
            }
            if (currentWeight < lowestWeight && !failedTask) 
            {
                lowestWeight = currentWeight;
                tempTasks = m_taskListOptions[i];
            }
        }
        if(tempTasks.Count == 0)
        {
            m_taskListOptions[0][0].TaskFailed();
            m_toDoGoals.Add(m_taskListOptions[0][0]);
            m_taskListOptions.Clear();
            return;
        }
        m_tasks.Clear();
        for (int i = 0; i < tempTasks.Count; i++) 
        {
            m_tasks.Add(tempTasks[i]);
        }
        m_tasks[m_tasks.Count - 1].StartExecution();
        m_multipleIdle = false;
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
        if(a.getCooldown() > 0)
        {
            return false;
        }
        if(m_tasks.Count == 0)
        {
            return true;
        }
        if(a.getPriority() > m_tasks[0].getPriority())
        {
            if (m_tasks[0] is Idle) { m_multipleIdle = false; }
            else
            {
                m_tasks[0].setWeight(false);
                m_toDoGoals.Add(m_tasks[0]);
            }
            m_tasks.Clear();
            return true;
        }
        return false;
    }

    protected bool CheckPrerequisite(Task goal, int listIncrement)
    {
        if (goal.getPrerequisite().Count == 0)
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
            if (goal.getPrerequisite()[prerequisiteNumber] == m_Items[z].m_name)
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
    public void ToggleMesh()
    {
        if(m_visible)
        {
            m_visible = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            m_visible = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
    public void CantComplete()
    {
        m_tasks[0].setWeight(false);
        m_tasks[0].setWeight(m_tasks[0].getWeight()-1);
        m_toDoGoals.Add(m_tasks[0]);
        m_tasks.Clear();
    }

    public bool WeightChecker(Task goalA, Task goalB)
    {
        if(goalA.getWeight() <= goalB.getWeight())
        {
            return true;
        }
        return false;
    }
}
