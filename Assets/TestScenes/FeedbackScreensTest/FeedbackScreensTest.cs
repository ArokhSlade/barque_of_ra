using UnityEngine;

public class FeedbackScreensTest : MonoBehaviour
{
    [SerializeField] BattleWonScreen battleWonScreen;

    void Start()
    {
        battleWonScreen.Open();
    }

    void Update()
    {
        
    }
}
