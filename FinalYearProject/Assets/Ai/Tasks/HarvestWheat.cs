using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HarvestWheat : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    public HarvestWheat()
    {
        m_Weight = 1;
        m_Task = "HarvestWheat";
        m_PreRequisite.Add("InRange");
        m_PreRequisite.Add("Hoe");
        m_effect = "WheatHarvested";
    }
    public override Vector3 getDestination()
    {
        FarmScript[] farms = Object.FindObjectsOfType<FarmScript>();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = m_agent.transform.position;
        for (int i = 0; i < farms.Length; i++)
        {
            float distance = Vector3.Distance(currentPosition, farms[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                m_destination = farms[i].transform.position;
            }
        }
        return m_destination;
    }
}
