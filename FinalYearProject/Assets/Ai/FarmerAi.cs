using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAi : BaseAi
{
    public bool m_HasHoe;
    private Vector3 m_destination;
    public FarmerAi()
    {
        m_HasHoe = false;
        m_destination = Vector3.zero;
        m_availableActions.Add(new HarvestWheat());
        m_availableActions.Add(new GetHoe());
    }
    void Update()
    {
        if (m_tasks.Count != 0)
        {
            int currentTask = 0;
            while (m_tasks[currentTask].m_PreRequisite.Count != 0 || m_AllConditionsMet)
            {
                for (int i = 0; i < m_tasks[currentTask].m_PreRequisite.Count; i++)
                {

                }
            }
        }
        base.Update();
        }
        public bool CheckHoe() { return m_HasHoe; }
}
