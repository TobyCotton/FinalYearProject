using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class Task
{
    public List<string> m_PreRequisite;
    public string m_Task;
    public int m_Weight;
    public string m_effect;
    public float m_priority = -1.0f;

    public Task()
    {
        m_PreRequisite = new List<string>();
    }
    public virtual void StartExecution() { }
    public virtual bool Executing() { return true; }
    public virtual Vector3 getDestination(BaseAi ai) { return Vector3.zero; }
}
