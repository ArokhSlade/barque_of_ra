using UnityEngine;

namespace BarqueOfRa.Game.UI
{
    public class PausingPopup : Popup
    {
        public override void Open()
        {
            base.Open();
            UI.Instance.RequestPause(this);
        }

        public override void Close()
        {
            UI.Instance.RequestUnpause(this);
            base.Close();
        }

        public void OnPopupConfirmClicked()
        {
            Close();
        }
    }

}
