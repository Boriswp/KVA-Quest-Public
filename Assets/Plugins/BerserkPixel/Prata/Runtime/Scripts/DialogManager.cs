using System;
using UnityEngine;

namespace BerserkPixel.Prata
{
    public class DialogManager : MonoBehaviour
    {
        private static DialogManager _instance;
        public static DialogManager Instance => _instance;

        [SerializeField] private DialogRenderer dialogRenderer;

        /// <summary>
        /// Subscribe to this actions to listen and act according to this different events
        ///
        /// For example on another script you can do:
        ///
        /// private void Start()
        /// {
        ///     DialogManager.Instance.OnDialogStart += HandleDialogStart;
        ///     DialogManager.Instance.OnDialogEnds += HandleDialogEnd;
        ///     DialogManager.Instance.OnDialogCancelled += HandleDialogEnd;
        /// }
        ///
        /// private void OnDisable()
        /// {
        ///     DialogManager.Instance.OnDialogStart -= HandleDialogStart;
        ///     DialogManager.Instance.OnDialogEnds -= HandleDialogEnd;
        ///     DialogManager.Instance.OnDialogCancelled -= HandleDialogEnd;
        /// }
        ///
        /// private void HandleDialogStart()
        /// {
        ///     // Disable player's movement
        /// }
        ///
        /// private void HandleDialogEnd()
        /// {
        ///     // Enable player's movement
        /// }
        ///  
        /// </summary>
        public Action OnDialogStart = delegate { };
        public Action OnDialogEnds = delegate { };
        public Action OnDialogCancelled = delegate { };
        public Action<Dialog> OnDialogChanged = delegate { };

        private bool isInConversation;
        private Interaction lastInteraction;

        private Dialog _currentDialog = null;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        /// <summary>
        /// Starts or continues a conversation.
        /// </summary>
        /// <param name="interaction">The Interaction to use</param>
        public void Talk(Interaction interaction)
        {
            if (interaction.HasAnyDialogLeft())
            {
                if (!isInConversation)
                {
                    isInConversation = true;
                    OnDialogStart?.Invoke();
                }

                if (lastInteraction != interaction)
                {
                    lastInteraction = interaction;
                }

                ShowDialog();
            }
            else
            {
                HideDialog();
            }
        }

        public bool CurrentDialogHasChoices()
        {
            if (_currentDialog == null) return false;

            return _currentDialog.HasChoices();
        }

        public void HideDialog()
        {
            dialogRenderer?.Hide();

            if (lastInteraction != null)
            {
                if (lastInteraction.HasAnyDialogLeft())
                {
                    OnDialogCancelled?.Invoke();
                }
                else
                {
                    OnDialogEnds?.Invoke();
                }

                lastInteraction = null;
            }

            isInConversation = false;
        }

        private void ShowDialog()
        {
            Dialog dialog = lastInteraction.GetDialog();

            OnDialogChanged?.Invoke(dialog);

            _currentDialog = dialog;

            dialogRenderer?.Show();
            dialogRenderer?.Render(dialog);
        }

        public void MakeChoice(string dialogGuid, string choice)
        {
            if (lastInteraction == null) return;

            Dialog dialog = lastInteraction.GetNextDialogFromChoice(dialogGuid, choice);

            OnDialogChanged?.Invoke(dialog);

            _currentDialog = dialog;

            if (dialog == null)
            {
                HideDialog();
                return;
            }

            dialogRenderer?.Render(dialog);
        }
    }
}