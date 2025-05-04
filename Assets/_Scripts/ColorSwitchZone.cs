using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;


namespace _Scripts
{
    public class ColorSwitchZone : MonoBehaviour
    {
        private Dictionary<MyCharacterController, float> _lastSwitchTimes = new();
        [SerializeField] private float _switchCooldown = 0.5f;

        [SerializeField] private SwitchModeType _modeType = SwitchModeType.Toggle;
        [SerializeField] private Color _colorBlue;
        [SerializeField] private Color _colorRed;
        [SerializeField] private Color _mixColor;
        [SerializeField] private AudioSource _audioSource;
        
        private SpriteRenderer m_SpriteRenderer;

        
        private void Start()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            if (m_SpriteRenderer == null) return;

            var baseColor = _modeType switch
            {
                SwitchModeType.Toggle => _mixColor,
                SwitchModeType.ForceBlue => _colorBlue,
                SwitchModeType.ForceRed => _colorRed,
                _ => Color.white
            };

            baseColor.a = 0.5f;
            m_SpriteRenderer.color = baseColor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var character = other.GetComponent<MyCharacterController>();
            if (character == null) return;

            _lastSwitchTimes.TryAdd(character, -999f);

            if (Time.time - _lastSwitchTimes[character] < _switchCooldown) return;

            var newColor = character.GetColorType();
            var currentColor = character.GetColorType();

            switch (_modeType)
            {
                case SwitchModeType.Toggle:
                    newColor = (character.GetColorType() == CharacterColorType.Blue) ? CharacterColorType.Red : CharacterColorType.Blue;
                    break;
                case SwitchModeType.ForceBlue:
                    newColor = CharacterColorType.Blue;
                    break;
                case SwitchModeType.ForceRed:
                    newColor = CharacterColorType.Red;
                    break;
            }

            if (currentColor != newColor)
            {
                character.ChangeColor(newColor);
                if (_audioSource != null)
                {
                    _audioSource.Play();
                }
                _lastSwitchTimes[character] = Time.time;
            }
        }
    }
}