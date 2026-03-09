using UnityEngine;
using BarqueOfRa.Game.UI;
using BarqueOfRa.Game;

public class GameOverScreen : PausingPopup
{
    [SerializeField] InGame inGame;
    public void OnTryAgainButtonPressed()
    {
        inGame.ReloadLevel();
    }

}
