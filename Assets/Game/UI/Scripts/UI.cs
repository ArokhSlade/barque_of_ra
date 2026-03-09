using UnityEngine;

namespace BarqueOfRa.Game.UI
{

    public class UI : MonoBehaviour
    {
        class UIStack
        {
            int capacity;
            ModalMenu[] uiLayers;
            int size;

            public UIStack(int capacity)
            {
                this.capacity = capacity;
                uiLayers = new ModalMenu[capacity];
                size = 0;
            }

            public void Push(ModalMenu newTop)
            {
                if (Full)
                {
                    Debug.LogError($"cannot Push {newTop}. stack full ({capacity})");
                    return;
                }
                else
                {
                    uiLayers[size] = newTop;
                    size++;
                }
            }

            public ModalMenu Pop()
            {
                if (Empty)
                {
                    Debug.LogError("cannot pop. stack empty");
                    return null;
                }
                else
                {
                    size--;
                    ModalMenu oldTop = uiLayers[size];
                    uiLayers[size] = null;
                    return oldTop;
                }
            }

            public bool Empty => size == 0;
            public bool Full => size == capacity;
            public ModalMenu Top
            {
                get
                {
                    if (Empty)
                    {
                        Debug.LogError("no Top. stack empty.");
                        return null;
                    }
                    ModalMenu top = uiLayers[size - 1];
                    return top;
                }
            }
        }

        public static UI Instance
        {
            get
            {
                if (s_instance == null)
                {
                    Debug.LogError("UI instance not initialized yet.");
                }
                return s_instance;
            }
        }

        private static UI s_instance;

        [SerializeField] InGame inGame;
        [SerializeField] PauseMenu pauseMenu;
        [SerializeField] SettingsMenu settingsMenu;
        [SerializeField] RectTransform mainMenuBackground;
        [SerializeField] CreditsScreen creditsScreen;


        const int maxModalMenusCount = 10;
        UIStack uiStack = new(maxModalMenusCount);
        ModalMenu firstPauser;

        void Awake()
        {
            if (s_instance != null)
            {
                Debug.LogError("UI Instance already exists");
            }
            s_instance = this;
        }

        public void PushMenu(ModalMenu menu)
        {
            uiStack.Push(menu);
        }

        public ModalMenu PopMenu()
        {
            var oldTop = uiStack.Pop();
            return oldTop;
        }

        public void EnableInteractionBelow()
        {
            if (!uiStack.Empty)
            {
                uiStack.Top.EnableInteraction();
            }
        }

        public void DisableInteractionBelow()
        {
            if (!uiStack.Empty)
            {
                uiStack.Top.DisableInteraction();
            }
        }
        public void ShowBelow()
        {
            if (!uiStack.Empty)
            {
                uiStack.Top.Show();
            }
        }

        public void HideBelow()
        {
            if (!uiStack.Empty)
            {
                uiStack.Top.Hide();
            }
        }

        public ModalMenu TopMenu => uiStack.Empty ? null : uiStack.Top;


        public void RequestPause(ModalMenu menu)
        {
            //NOTE(Gerald 2025 08 06): only the first menu to request a pause actually causes it
            if (firstPauser != null) { return; }
             
            firstPauser = menu;
            inGame.Pause();
            
        }

        public void ShowMainMenuBackground()
        {
            mainMenuBackground.gameObject.SetActive(true);
        }

        public void HideMainMenuBackground()
        {
            mainMenuBackground.gameObject.SetActive(false);
        }

        public void RequestUnpause(ModalMenu menu)
        {
            // NOTE(Gerald 2025 08 06): only the first menu who requested a pause may unpause
            if (firstPauser != menu) { return; }

            firstPauser = null;
            inGame.Unpause();
        }

        public void OpenSettingsMenu()
        {
            settingsMenu.Open();
        }

        public void OpenCreditsScreen()
        {
            creditsScreen.Open();
        }

        public void TogglePauseMenu()
        {
            pauseMenu.Toggle();
        }


        public void Reset()
        {
            while(!uiStack.Empty)
            {
                TopMenu.Close();
            }
        }
    }
}