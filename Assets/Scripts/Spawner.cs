using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int spawnCount;
    [SerializeField] private float randomRadius;
    [SerializeField] private GameObject spawnedPrefab;

    private void Start()
    {
        for (var i = 0; i < spawnCount; i++)
        {
            var randomPoint = Random.insideUnitCircle * randomRadius;
            var t = transform;
            var spawnedInstance = Instantiate(spawnedPrefab,
                t.position + new Vector3(randomPoint.x, randomPoint.y), Quaternion.identity, t);
            spawnedInstance.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        }
    }
}