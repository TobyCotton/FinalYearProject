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
    public bool m_executionStarted;

    public Task()
    {
        m_PreRequisite = new List<string>();
        m_executionStarted = false;
    }
    public virtual void StartExecution() { m_executionStarted = true; }
    public virtual bool Executing() { return true; }
    public virtual Vector3 getDestination(BaseAi ai) { return Vector3.zero; }
}
