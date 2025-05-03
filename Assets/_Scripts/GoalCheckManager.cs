using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class GoalCheckManager : MonoBehaviour
    {
        private static GoalCheckManager ms_Instance;
        
        private readonly List<GoalArea> m_GoalAreaList = new List<GoalArea>();
        
        private bool m_LevelCompleted = false;


        private void Awake()
        {
            ms_Instance = this;
        }


        public static void RegisterGoalArea(GoalArea area)
        {
            if (!ms_Instance.m_GoalAreaList.Contains(area))
            {
                ms_Instance.m_GoalAreaList.Add(area);
            }
        }

        private void Update()
        {
            if (m_LevelCompleted) return;

            bool allOccupied = true;
            foreach (var goal in m_GoalAreaList)
            {
                if (!goal.IsCorrectlyOccupied())
                {
                    allOccupied = false;
                    break;
                }
            }

            if (allOccupied && m_GoalAreaList.Count > 0)
            {
                m_LevelCompleted = true;
                LevelComplete();
            }
        }

        private void LevelComplete()
        {
            Debug.Log("Level Completed!");
            // TODO: call LevelManager.Instance.LevelComplete() or similar
        }
    }
}