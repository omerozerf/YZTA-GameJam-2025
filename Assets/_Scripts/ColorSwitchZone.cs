using UnityEngine;
using UnityEngine.Serialization;


namespace _Scripts
{
    public class ColorSwitchZone : MonoBehaviour
    {
        [SerializeField] private SwitchModeType _modeType = SwitchModeType.Toggle;
        
        private SpriteRenderer m_SpriteRenderer;

        
        private void Start()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            if (m_SpriteRenderer == null) return;

            var baseColor = _modeType switch
            {
                SwitchModeType.Toggle => new Color(1f, 0f, 1f),
                SwitchModeType.ForceBlue => Color.blue,
                SwitchModeType.ForceRed => Color.red,
                _ => Color.white
            };

            baseColor.a = 0.5f;
            m_SpriteRenderer.color = baseColor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var character = other.GetComponent<MyCharacterController>();
            if (character == null) return;

            var newColor = character.GetColorType();

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

            character.ChangeColor(newColor);
        }
    }
}