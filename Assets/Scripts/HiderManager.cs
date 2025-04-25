using UnityEngine;

public sealed class HiderManager : MonoBehaviour
{

    [SerializeField] MeshFilter[] meshes;
    public float maxDist;        // max distance from 0,0
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject target;

    public int hiderCount;


    public MeshFilter[] GetHidingSpots()
    {
        return meshes;
    }

    void Start()
    {
        for (int i = 0; i < hiderCount; i++)
        {
            Vector3 position = FindSpawnPoint();
            GameObject newProp = Instantiate(prefab);
            // assign random mesh filter
            newProp.transform.position = position;
            MeshFilter newMesh = meshes[Random.Range(0, meshes.Length)];
            MeshFilter propMesh = newProp.GetComponent<MeshFilter>();
            propMesh = newMesh;
            Bot bot = newProp.GetComponent<Bot>();

            bot.target = target;
        }
    }

    public Vector3 FindSpawnPoint()
    {
        bool valid = false;
        Vector3 position = transform.position;
        while (!valid)
        {
            Vector3 tentative = new(Random.Range(-maxDist, maxDist), transform.position.y, Random.Range(-maxDist, maxDist));
            // check chosen spot for colliders
            Collider[] colliders = Physics.OverlapSphere(tentative, 0.001f);
            if (colliders.Length <= 0)
            {
                valid = true;
                position = tentative;
            }
        }

        return position;
    }

}
