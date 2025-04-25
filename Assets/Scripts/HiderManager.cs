using UnityEngine;
using UnityEngine.AI;

public sealed class HiderManager : MonoBehaviour
{

    [SerializeField] GameObject[] models;
    public float maxDist;        // max distance from 0,0
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject target;

    public int hiderCount;


    void Start()
    {
        for (int i = 0; i < hiderCount; i++)
        {
            Vector3 position = FindSpawnPoint();
            GameObject newProp = Instantiate(models[Random.Range(0, models.Length)]);
            newProp.tag = "hider";

            newProp.transform.position = position;
            Rigidbody rb = newProp.GetComponent<Rigidbody>();
            rb.mass = 0.1f;

            Bot bot = newProp.AddComponent<Bot>();
            NavMeshObstacle ob = newProp.GetComponent<NavMeshObstacle>();
            Destroy(ob);
            NavMeshAgent agent = newProp.AddComponent<NavMeshAgent>();
            agent.baseOffset = 0f;

            bot.target = target;

            bot.Init();
        }
    }

    public Vector3 FindSpawnPoint()
    {
        bool valid = false;
        float yPos = 3f;
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
        position.y = yPos;
        return position;
    }

}
