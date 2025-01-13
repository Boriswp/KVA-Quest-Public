using BerserkPixel.Prata;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class Conversation : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        private IInteractionHolder _interactionHolder;

        private ConversationManager _manager;

        public void StartConversation()
        {
            if (TryGetComponent(out _interactionHolder))
            {
                _manager = FindObjectOfType<ConversationManager>();

                _manager.Setup(_interactionHolder);
                _manager.Interact();
            }

            _startButton.gameObject.SetActive(false);
        }
    }
}