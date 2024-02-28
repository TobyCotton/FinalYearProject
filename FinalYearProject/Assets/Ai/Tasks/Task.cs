using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class Task//Create a cooldown period for tasks if they fail
{
    public List<string> m_PreRequisite;
    public string m_Task;
    public int m_Weight;
    public string m_effect;
    public float m_priority = -1.0f;
    public bool m_executionStarted;
    public float m_time = 2.0f;
    protected BaseAi m_baseAi;

    public Task()
    {
        m_PreRequisite = new List<string>();
        m_executionStarted = false;
    }
    public virtual void StartExecution() 
    { 
        m_executionStarted = true;
        if(m_baseAi.m_visible)
        {
            m_baseAi.ToggleMesh();
        }
    }
    public virtual bool Executing()
    { 
        return true; 
    }
    public virtual Vector3 getDestination() { return Vector3.zero; }
}
