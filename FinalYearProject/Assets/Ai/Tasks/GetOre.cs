using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetOre : Task
{
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    private StoreRoomScript m_storeRoom;
    public GetOre(NavMeshAgent agent, BaseAi ai)
    {
        m_Weight = 1.0f;
        m_Task = "GetOre";
        m_PreRequisite.Add("InRange");
        m_effect = "Ore";
        m_destination = Vector3.zero;
        m_agent = agent;
        m_baseAi = ai;
    }
    public GetOre()
    {
        m_Weight = 1.0f;
        m_Task = "GetOre";
        m_PreRequisite.Add("InRange");
        m_effect = "Ore";
        m_destination = Vector3.zero;
        m_agent = null;
    }
    public override Vector3 getDestination()
    {
        StoreRoomScript[] storeRooms = Object.FindObjectsOfType<StoreRoomScript>();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = m_baseAi.getNavAgent().transform.position;
        m_destination = Vector3.zero;
        bool hasItem = false;
        for (int i = 0; i < storeRooms.Length; i++)
        {
            hasItem = false;
            foreach (Item items in storeRooms[i].getStored())
            {
                if (items.m_name == "Ore")
                {
                    hasItem = true;
                }
            }
            float distance = Vector3.Distance(currentPosition, storeRooms[i].transform.position);
            if (distance < shortestDistance && hasItem)
            {
                m_storeRoom = storeRooms[i];
                shortestDistance = distance;
                m_destination = storeRooms[i].transform.position;
            }
        }
        if (m_destination == Vector3.zero)
        {
            TaskFailed();
        }
        return m_destination;
    }
    public override bool Executing()
    {
        foreach (Item item in m_storeRoom.getStored())
        {
            if (item.m_name == "Ore")
            {
                m_baseAi.getItems().Add(item);
                m_storeRoom.getStored().Remove(item);
                m_storeRoom.m_OreStored--;
                return true;
            }
        }
        TaskFailed();
        return false;
    }
}
