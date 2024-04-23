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
    protected List<string> m_PreRequisite;
    protected string m_Task;
    protected float m_Weight;
    protected string m_effect;
    protected float m_priority = -1.0f;
    protected bool m_executionStarted;
    protected float m_time = 2.0f;
    protected BaseAi m_baseAi;
    protected float m_cooldownPeriod = 0f;
    protected bool m_failed = false;

    public string getTaskName() { return m_Task; }
    public List<string> getPrerequisite() { return m_PreRequisite; }
    public float getWeight() { return m_Weight; }
    public string getEffect() { return m_effect; }
    public float getPriority() { return m_priority; }
    public void setWeight(float newV) { m_priority = newV; }
    public bool getExcuted() { return m_executionStarted; }
    public void setWeight(bool newB) { m_executionStarted = newB; }
    public float getTime() { return m_time; }
    public void setTime(float newTime) { m_time = newTime; }
    public float getCooldown() { return m_cooldownPeriod; }
    public bool getFailed() { return m_failed; }
    public Task()
    {
        m_PreRequisite = new List<string>();
        m_executionStarted = false;
    }

    public void UpdatecoolDownPeriod()
    {
        if(m_cooldownPeriod > 0)
        {
            m_cooldownPeriod -= Time.deltaTime;
            if(m_cooldownPeriod <= 0)
            {
                m_cooldownPeriod = 0;
                m_failed = false;
            }
        }
    }
    public virtual void StartExecution() 
    { 
        m_executionStarted = true;
        if(m_baseAi.getVisible())
        {
            m_baseAi.ToggleMesh();
        }
    }
    public virtual bool Executing()
    { 
        return true; 
    }
    public virtual Vector3 getDestination() { return Vector3.zero; }

    public void TaskFailed()
    {
        m_failed = true;
        m_cooldownPeriod = 15.0f;
    }
}
