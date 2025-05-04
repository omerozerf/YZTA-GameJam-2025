using System;
using UnityEngine;

namespace _Scripts
{
    public class KillZone : MonoBehaviour
    {
        public enum KillMode { OnlyBlue, OnlyRed, Both }
        [SerializeField] private KillMode _killMode;
        [SerializeField] private Color _colorBlue;
        [SerializeField] private Color _colorRed;
        [SerializeField] private Color _mixColor;
        [SerializeField] private bool _canChangeColor = true;
        
        private SpriteRenderer m_SpriteRenderer;


        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (!_canChangeColor) return;
            
            if (m_SpriteRenderer != null)
            {
                Color visualColor = Color.white;
                switch (_killMode)
                {
                    case KillMode.OnlyBlue: visualColor = _colorRed; break;
                    case KillMode.OnlyRed: visualColor = _colorBlue; break;
                    case KillMode.Both: visualColor = _mixColor; break;
                }
                m_SpriteRenderer.color = visualColor;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var character = other.gameObject.GetComponent<MyCharacterController>();
            if (character == null) return;

            var charColor = character.GetColorType();
            if ((_killMode == KillMode.Both) ||
                (_killMode == KillMode.OnlyBlue && charColor == CharacterColorType.Blue) ||
                (_killMode == KillMode.OnlyRed && charColor == CharacterColorType.Red))
            {
                character.Die();
            }
        }
    }
}