using UnityEngine;
using UnityEngine.AI;

public sealed class HiderManager : MonoBehaviour
{

    [SerializeField] GameObject[] models;
    public float maxDist;        // max distance from 0,0
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject target;

    public int hiderCount;

    public int extraProps;

    private const int maxAttempts = 100;

    void Awake()
    {
        World.Instance.Init();
    }

    void Start()
    {

        // spawn hiders
        for (int i = 0; i < hiderCount; i++)
        {
            Vector3 position = FindSpawnPoint();
            GameObject newProp = Instantiate(models[Random.Range(0, models.Length)]);
            newProp.tag = "hider";

            newProp.transform.localScale *= Random.Range(0.8f, 1.2f);
            newProp.transform.position = position;
            Rigidbody rb = newProp.GetComponent<Rigidbody>();
            rb.mass = 0.1f;
            newProp.transform.Rotate(0, Random.Range(0, 360f), 0);
            Bot bot = newProp.AddComponent<Bot>();
            NavMeshObstacle ob = newProp.GetComponent<NavMeshObstacle>();
            Destroy(ob);
            NavMeshAgent agent = newProp.AddComponent<NavMeshAgent>();
            agent.baseOffset = 0f;

            bot.target = target;

            bot.Init();
        }
        // spawn extra props
        for (int i = 0; i < extraProps; i++)
        {
            Vector3 position = FindSpawnPoint();
            GameObject newProp = Instantiate(models[Random.Range(0, models.Length)]);
            newProp.transform.localScale *= Random.Range(0.6f, 2f);
            newProp.transform.position = position;
            newProp.transform.Rotate(0, Random.Range(0, 360f), 0);
            Rigidbody rb = newProp.GetComponent<Rigidbody>();
            rb.mass = 100000000f;
        }

        if (hiderCount <= 0)
        {
            hiderCount = 1;

        }


    }

    void Update()
    {

    }

    // TODO: replace with for loop instead of while loop
    public Vector3 FindSpawnPoint()
    {
        float yPos = 5f;
        Vector3 position = transform.position;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 tentative = new(transform.position.x + Random.Range(-maxDist, maxDist), yPos, transform.position.z + Random.Range(-maxDist, maxDist));
            tentative.y = yPos;
            // check chosen spot for colliders
            Collider[] colliders = Physics.OverlapSphere(tentative, 3f);
            if (colliders.Length <= 0)
            {
                return tentative;
            }
        }

        if (position == transform.position)
        {
            Debug.Log($"No valid spawn point found after {maxAttempts} attempts!");
        }
        return position;
    }


}
