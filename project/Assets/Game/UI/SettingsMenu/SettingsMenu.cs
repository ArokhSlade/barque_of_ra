using UnityEngine;

namespace BarqueOfRa.Game.UI
{   
    public class SettingsMenu : FullScreenMenu
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void OnBackButtonPressed()
        {
            Close();
        }
    }    
}
