using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            float spawnY = Random.Range(-2f, 2f);
            Instantiate(obstaclePrefab, new Vector2(10f, spawnY), Quaternion.identity);
            nextSpawnTime = Time.time + spawnRate;
        }
    }
}
