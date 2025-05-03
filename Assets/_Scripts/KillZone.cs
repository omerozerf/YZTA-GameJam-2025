using UnityEngine;

namespace _Scripts
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var character = other.GetComponent<MyCharacterController>();
            if (character != null)
            {
                character.Die();
            }
        }
    }
}