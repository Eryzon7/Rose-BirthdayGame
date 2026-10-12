using System.Collections;
using UnityEngine;

public class WallnutSpawner : MonoBehaviour
{
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform rightPoint;

    public GameObject wallnutPrefab;
    public float delayAfterDestroyed = 0.5f;

    GameObject current;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnOne();

            // Wait until the spawned object is destroyed
            yield return new WaitUntil(() => current == null);

            yield return new WaitForSeconds(delayAfterDestroyed);
        }
    }

    void SpawnOne()
    {
        bool spawnLeft = UnityEngine.Random.value < 0.5f;
        Transform point = spawnLeft ? leftPoint : rightPoint;

        current = Instantiate(wallnutPrefab, point.position, Quaternion.identity);

        Wallnut roller = current.GetComponent<Wallnut>();
        roller.randomDirection = false;
        roller.direction = spawnLeft ? 1 : -1;   // left point rolls right, right point rolls left
    }
}
