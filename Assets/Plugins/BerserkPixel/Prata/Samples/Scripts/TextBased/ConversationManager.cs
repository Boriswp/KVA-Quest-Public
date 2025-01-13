using BerserkPixel.Prata;
using UnityEngine;

namespace Project.Scripts
{
    public class ConversationManager : MonoBehaviour
    {
        private Interaction _interaction;
        private IInteractionHolder _interactionHolder;

        public void Setup(IInteractionHolder interactionHolder)
        {
            _interactionHolder = interactionHolder;
            _interaction = _interactionHolder.GetInteraction();
        }

        public void Interact()
        {
            if (_interaction != null)
            {
                DialogManager.Instance.Talk(_interaction);

                if (!_interaction.HasAnyDialogLeft())
                {
                    _interactionHolder.Reset();
                    _interaction = _interactionHolder.GetInteraction();
                }
            }
            else
            {
                DialogManager.Instance.HideDialog();
            }
        }
    }
}