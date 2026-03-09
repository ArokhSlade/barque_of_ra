using UnityEngine;
using UnityEngine.Events;

namespace BarqueOfRa.Game.UI
{   
    public class FullScreenMenu : ModalMenu
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("FullscreenMenu.Awake()");
        }

        public override void Open()
        {
            UI.Instance.HideBelow();
            base.Open();
        }

        public override void Close()
        {
            base.Close();
            UI.Instance.ShowBelow();
        }
    }    
}
