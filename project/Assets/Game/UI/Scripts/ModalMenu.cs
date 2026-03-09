using UnityEngine;
using UnityEngine.Events;

namespace BarqueOfRa.Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ModalMenu : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;

        protected virtual void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            gameObject.SetActive(true);
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Open()
        {
            Initialize();
            UI.Instance.DisableInteractionBelow();
            UI.Instance.PushMenu(this);
            Show();
            //TODO(Gerald 2025 08 06): this.interactable
            if (!canvasGroup.interactable)
            {
                Debug.LogError("ModalMenu not interactable on Open");
                EnableInteraction();
            }
        }

        public virtual void Close()
        {
            Hide();
            UI.Instance.PopMenu();
            UI.Instance.EnableInteractionBelow();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void EnableInteraction()
        {
            canvasGroup.interactable = true;
        }

        public virtual void DisableInteraction()
        {
            canvasGroup.interactable = false;
        }

    }
}
