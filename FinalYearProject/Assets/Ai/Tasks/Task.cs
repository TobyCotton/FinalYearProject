using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class Task:ICloneable
{
    public List<string> m_PreRequisite;
    public string m_Task;
    public int m_Weight;
    public string m_effect;

    public Task()
    {
        m_PreRequisite = new List<string>();
    }
    public object Clone()
    {
        Task clonedTask = new Task();
        clonedTask.m_PreRequisite = new List<string>();
        foreach (string preRequisite in this.m_PreRequisite)
        {
            string clonedpreRequisite = (string)preRequisite.Clone();

            clonedTask.m_PreRequisite.Add(clonedpreRequisite);
        }
        clonedTask.m_effect = this.m_effect;
        clonedTask.m_Weight = this.m_Weight;
        clonedTask.m_Task = this.m_Task;
        return clonedTask;
    }
    public virtual void StartExecution() { }
    public virtual bool Executing() { return true; }
    public virtual Vector3 getDestination() { return Vector3.zero; }
}
