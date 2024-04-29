using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.Progress;

public class Sell : Task
{
    private StoreRoomScript m_storeRoom;
    string m_ItemName;
    public Sell(string item,BaseAi ai,StoreRoomScript storeRoom)
    {
        m_storeRoom = storeRoom;
        m_ItemName = item;
        m_Weight = 1.0f;
        m_Task = "Sell";
        m_PreRequisite.Add("InRange");
        m_effect = "Sold";
        m_priority = 2.0f;
        m_baseAi = ai;
    }
    public Sell()
    {
        m_Weight = 1.0f;
        m_Task = "Sell";
        m_PreRequisite.Add("InRange");
        m_effect = "Sold";
        m_priority = 2.0f;
        m_baseAi = null;
    }
    public override Vector3 getDestination()
    {
        return m_storeRoom.transform.position;
    }
    public override bool Executing()
    {
        Item toRemove = null;
        for (int i = 0; i < m_storeRoom.getStored().Count; i++)
        {
            if (m_storeRoom.getStored()[i].m_name == m_ItemName)
            {
                toRemove = m_storeRoom.getStored()[i];
            }
        }
        if (toRemove != null)
        {
            m_storeRoom.getStored().Remove(toRemove);
            if(m_ItemName == "Wheat")
            {
                m_storeRoom.m_WheatStored--;
            }
            else if (m_ItemName == "Ore")
            {
                m_storeRoom.m_OreStored--;
            }
            else
            {
                m_storeRoom.m_WoodStored--;
            }
            MarketScript addMoney = m_baseAi.getWork().GetComponent<MarketScript>();
            if (addMoney != null)
            {
                addMoney.m_money++;
            }

        }
        else
        {
            TaskFailed();
            return false;
        }
        m_storeRoom.setRequested(false);
        return true;
    }
}
