using System;
using UnityEngine;

namespace _Scripts
{
    public class GoalArea : MonoBehaviour
    {
        [SerializeField] private CharacterColorType _requiredColor;
        [SerializeField] private Color _colorBlue;
        [SerializeField] private Color _colorRed;
        
        private bool m_IsCorrectlyOccupied;
        private SpriteRenderer m_SpriteRenderer;


        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

        }

        private void Start()
        {
            GoalCheckManager.RegisterGoalArea(this);
            UpdateColor();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var character = other.GetComponent<MyCharacterController>();
            if (character && character.GetColorType() == _requiredColor)
            {
                m_IsCorrectlyOccupied = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var character = other.GetComponent<MyCharacterController>();
            if (character && character.GetColorType() == _requiredColor)
            {
                m_IsCorrectlyOccupied = false;
            }
        }


        private void UpdateColor()
        {
            var baseColor = _requiredColor switch
            {
                CharacterColorType.Blue => _colorBlue,
                CharacterColorType.Red => _colorRed,
                _ => Color.white
            };

            baseColor.a = 0.5f;
            m_SpriteRenderer.color = baseColor;
        }
        
        
        public bool IsCorrectlyOccupied()
        {
            return m_IsCorrectlyOccupied;
        }
    }
}