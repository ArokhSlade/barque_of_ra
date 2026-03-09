using UnityEngine;
using TMPro;
using BarqueOfRa.Game.UI;

public class LevelWonScreen : PausingPopup
{
    [SerializeField] TextMeshProUGUI winMessage;
    [SerializeField] HUD hud;


    public void LoadText(int SoulCount ,string LevelName)
    {
        winMessage.text = "Magnificent!\nYou passed the first Gate of Duat.\nYou safely guided " + SoulCount + " through " + LevelName + "!";
    }

    public void OnHardModeButtonClicked()
    {
        Close();
        hud.OnHardModeRequested();
    }

    public void OnBackToMenuButtonClicked()
    {
        Close();
        hud.OnLevelWonConfirmed();
    }
}
