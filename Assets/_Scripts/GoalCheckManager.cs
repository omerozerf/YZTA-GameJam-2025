using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace _Scripts
{
    public class GoalCheckManager : MonoBehaviour
    {
        private static GoalCheckManager ms_Instance;
        
        private readonly List<GoalArea> m_GoalAreaList = new List<GoalArea>();
        
        private bool m_LevelCompleted = false;

        private float m_Timer = 0f;
        private float m_RequiredTime = 1.5f;
        private bool m_WaitingForCompletion = false;
        private Tween m_ShakeTween;
        private Camera m_MainCamera;
        private Vector3 m_CamInitialPos;


        private void Awake()
        {
            ms_Instance = this;
            m_MainCamera = Camera.main;
            m_CamInitialPos = m_MainCamera.transform.position;
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
                if (!m_WaitingForCompletion)
                {
                    m_WaitingForCompletion = true;
                    StartLevelCompleteSequence().Forget();
                }
            }
            else
            {
                m_WaitingForCompletion = false;
                m_Timer = 0f;
                m_ShakeTween?.Kill();
                if (m_MainCamera.transform.position != m_CamInitialPos)
                {
                    m_MainCamera.transform.DOMove(m_CamInitialPos, 0.2f);
                }
            }
        }

        private void LevelComplete()
        {
            Debug.Log("Level Completed!");
            LevelManager.LoadNextLevel();
        }

        private async UniTaskVoid StartLevelCompleteSequence()
        {
            m_ShakeTween = DOVirtual.Float(0, 1, m_RequiredTime, t =>
            {
                float strength = Mathf.Lerp(0.05f, 0.15f, t);
                m_MainCamera.transform.position = m_CamInitialPos + UnityEngine.Random.insideUnitSphere * strength;
            }).SetEase(Ease.Linear);

            while (m_Timer < m_RequiredTime && m_WaitingForCompletion)
            {
                m_Timer += Time.deltaTime;
                await UniTask.Yield();
            }

            m_ShakeTween?.Kill();
            m_MainCamera.transform.position = m_CamInitialPos;

            if (m_Timer >= m_RequiredTime && m_WaitingForCompletion)
            {
                m_LevelCompleted = true;
                LevelComplete();
            }
        }
    }
}