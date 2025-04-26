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

    void Awake()
    {
        World.Instance.Init();
    }

    void Start()
    {
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
    }

    void Update()
    {

    }

    // TODO: replace with for loop instead of while loop
    public Vector3 FindSpawnPoint()
    {
        bool valid = false;
        float yPos = 3f;
        Vector3 position = transform.position;
        while (!valid)
        {
            Vector3 tentative = new(Random.Range(-maxDist, maxDist), transform.position.y + 0.1f, Random.Range(-maxDist, maxDist));
            // check chosen spot for colliders
            //Collider[] colliders = Physics.OverlapSphere(tentative, 3f);
            valid = true;
            position = tentative;
        }
        position.y = yPos;
        return position;
    }


}
