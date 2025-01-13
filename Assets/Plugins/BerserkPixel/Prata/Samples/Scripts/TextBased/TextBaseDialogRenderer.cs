using BerserkPixel.Prata;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.BerserkPixel.Prata.Samples.Scripts
{
    public class TextBaseDialogRenderer : DialogRenderer
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Text authorText;
        [SerializeField] private Text dialogText;
        [SerializeField] private Image dialogLeftImage;
        [SerializeField] private Image dialogRightImage;
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private GameObject choiceButtonPrefab;
        [SerializeField] private Button _nextButton;

        public override void Show()
        {
            container.SetActive(true);
        }

        public override void Render(Dialog dialog)
        {
            dialogText.text = dialog.text;
            authorText.text = dialog.character.characterName;

            dialogLeftImage.preserveAspect = true;
            dialogRightImage.preserveAspect = true;

            if (dialog.character.isPlayer)
            {
                dialogLeftImage.gameObject.SetActive(true);
                dialogLeftImage.sprite = dialog.GetImage();
                dialogRightImage.gameObject.SetActive(false);
            }
            else
            {
                dialogRightImage.gameObject.SetActive(true);
                dialogRightImage.sprite = dialog.GetImage();
                dialogLeftImage.gameObject.SetActive(false);
            }

            if (dialog.choices.Count > 1)
            {
                RemoveChoices();

                foreach (var choice in dialog.choices)
                {
                    GameObject choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
                    choiceButton.GetComponentInChildren<Text>().text = choice;
                    choiceButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        DialogManager.Instance.MakeChoice(dialog.guid, choice);
                    });
                }

                choicesContainer.gameObject.SetActive(true);

                _nextButton.gameObject.SetActive(false);
            }
            else
            {
                choicesContainer.gameObject.SetActive(false);
                _nextButton.gameObject.SetActive(true);
            }
        }

        public override void Hide()
        {
            RemoveChoices();
            _nextButton.gameObject.SetActive(false);
            container.SetActive(false);
        }

        private void RemoveChoices()
        {
            if (choicesContainer.childCount > 0)
            {
                foreach (Transform child in choicesContainer)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}