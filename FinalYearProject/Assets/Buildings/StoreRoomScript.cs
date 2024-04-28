using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRoomScript : Building
{
    private List<Item> m_Stored;
    public int m_WheatStored;
    public int m_WoodStored;
    public int m_OreStored;
    MerchantAI m_merchant;
    private bool m_requested = false;

    public List<Item> getStored() { return m_Stored; }
    public void setRequested(bool newB) { m_requested = newB; }

    public StoreRoomScript()
    {
        m_Stored = new List<Item>();
    }

    void Update()
    {
        if(!m_merchant)//Null check(merchant is the only one we need to directly give tasks to)
        {
            FindMerchant();
        }
        if(m_merchant && !m_requested)
        {
            if(m_WheatStored > 7)
            {
                m_requested = true;
                if (m_merchant.PriorityChecker(new Sell()))
                {
                    m_merchant.AddToTaskList(new Sell("Wheat", m_merchant, this), 0);//Pass name as item could be taken before merchent gets here whereas there will still be an item of this name here
                    m_merchant.SetTaskList();
                }
                else
                {
                    m_merchant.getToDoGoals().Add(new Sell("Wheat", m_merchant, this));
                }
            }
            if (m_WoodStored > 4)
            {
                m_requested = true;
                if (m_merchant.PriorityChecker(new Sell()))
                {
                    m_merchant.AddToTaskList(new Sell("Wood", m_merchant, this), 0);
                    m_merchant.SetTaskList();
                }
                else
                {
                    m_merchant.getToDoGoals().Add(new Sell("Wood", m_merchant, this));
                }
            }
            if (m_OreStored > 4)
            {
                m_requested = true;
                if (m_merchant.PriorityChecker(new Sell()))
                {
                    m_merchant.AddToTaskList(new Sell("Ore", m_merchant, this), 0);
                    m_merchant.SetTaskList();
                }
                else
                {
                    m_merchant.getToDoGoals().Add(new Sell("Ore", m_merchant, this));
                }
            }
        }
    }

    private void FindMerchant()
    {
        MerchantAI[] merchants = Object.FindObjectsOfType<MerchantAI>();
#pragma warning disable
        for (int i = 0; i < merchants.Length; i++)//disable warning as warning comes up as we always leave this function early
        {
            m_merchant = merchants[i];
            return;
        }
#pragma warning restore
    }
}
