using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRoomScript : Building
{
    public List<Item> m_Stored;
    
    public StoreRoomScript()
    {
        m_Stored = new List<Item>();
    }

    void Update()
    {
        foreach (Item item in m_Stored)
        {
            Debug.Log(item.m_name);
        }
    }
}
