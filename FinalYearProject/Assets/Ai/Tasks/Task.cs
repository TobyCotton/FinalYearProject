using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Task
{
    protected string m_PreRequisite;
    protected string m_Task;
    protected int m_Weight;
    public virtual void StartExecution() { }
    public virtual bool Executing() { return false; }
}
