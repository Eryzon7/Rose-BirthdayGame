using UnityEngine;

public class WallnutGameManager : MonoBehaviour
{
    private bool isPlaying = false;
    private int points = 0;

    [SerializeField] WallnutSpawner wallnutSpawner;
    


    private void OnEnable()
    {
        wallnutSpawner.roller.OnBreak += keepScore;
    }

    private void OnDisable()
    {
        wallnutSpawner.roller.OnBreak -= keepScore;
    }

    private void Update()
    {
        if (!isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        if (isPlaying && points >= 10)
        {
            WinGame();
        }
    }
    void StartGame()
    {
        isPlaying = true;
        StartCoroutine(wallnutSpawner.SpawnLoop());
    }

    void keepScore()
    {
        points++;
    }

    void WinGame()
    {
        StopCoroutine(wallnutSpawner.SpawnLoop());
    }





}
