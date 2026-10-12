using UnityEngine;

public enum GameState
{
    Paused,
    Playing,
    GameOver

}

public class WallnutGameManager : MonoBehaviour
{
  
    [SerializeField]

    private void GameState(enum GameState)
    {
        switch()
            case Paused:
            break;
            case Playing:
            break;
            case GameOver:
            break;
    }
}
