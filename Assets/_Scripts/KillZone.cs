using System;
using UnityEngine;

namespace _Scripts
{
    public class KillZone : MonoBehaviour
    {
        public enum KillMode { OnlyBlue, OnlyRed, Both }
        [SerializeField] private KillMode _killMode;
        
        private SpriteRenderer m_SpriteRenderer;


        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (m_SpriteRenderer != null)
            {
                Color visualColor = Color.white;
                switch (_killMode)
                {
                    case KillMode.OnlyBlue: visualColor = Color.red; break;
                    case KillMode.OnlyRed: visualColor = Color.blue; break;
                    case KillMode.Both: visualColor = Color.magenta; break;
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