using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : Task
{
    private Vector3 m_destination;
    Item m_itemToStore;

    public StoreItem(Item item, BaseAi ai)
    {
        m_itemToStore = item;
        m_baseAi = ai;
        m_PreRequisite.Add("InRange");
        m_effect = "ItemStored";
        m_priority = 2.0f;
        m_Weight = 1;
        m_Task = "StoreItem";
    }
    public StoreItem()
    {
        m_PreRequisite.Add("InRange");
        m_effect = "ItemStored";
        m_priority = 2.0f;
        m_Weight = 1;
        m_Task = "StoreItem";
        m_destination = Vector3.zero;
    }
    public override Vector3 getDestination()
    {
        StoreRoomScript[] storeRooms = Object.FindObjectsOfType<StoreRoomScript>();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = m_baseAi.m_agent.transform.position;
        for (int i = 0; i < storeRooms.Length; i++)
        {
            float distance = Vector3.Distance(currentPosition, storeRooms[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                m_destination = storeRooms[i].transform.position;
            }
        }
        return m_destination;
    }
}
