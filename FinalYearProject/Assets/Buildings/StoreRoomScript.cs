using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRoomScript : Building
{
    List<Item> m_Stored;
    
    public StoreRoomScript()
    {
        m_Stored = new List<Item>();
    }
}
