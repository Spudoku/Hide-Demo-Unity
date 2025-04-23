

using UnityEngine;
using UnityEngine.AI;


public class Bot : MonoBehaviour
{

    [SerializeField] GameObject target;                  // represents an object to avoid
    public float hideDistance;          // how far to stand next to an object
    public float speed;

    public Vector3 destination;

    private float rayLength;

    public float cappedY = 1f;

    public float maxDist;        // max distance from 0,0
    public float minTeleportDist;

    NavMeshAgent agent;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rayLength = hideDistance * 10f;
        agent.speed = speed;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new(transform.position.x, cappedY, transform.position.z);
        // hide if can see the target
        if (CanSeeTarget())
        {
            CleverHide();
        }


        MoveTowardsDestination();
    }


    // hide in the nearest possible
    // spot again
    void CleverHide()
    {

        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        // search all hiding spots.
        foreach (GameObject i in World.Instance.GetHidingSpots())
        {
            // direction from target (the Seeker) to a given object
            Vector3 hideDir = i.transform.position - target.transform.position;

            // position for this object to hide
            Vector3 hidePos = i.transform.position + hideDir.normalized * hideDistance;
            hidePos.y = transform.position.y;

            // trying to prevent the seeker from being able to get between the hider and the hiding spot
            if (Vector3.Distance(target.transform.position, hidePos) < Vector3.Distance(transform.position, hidePos))
            {
                continue;
            }

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = i;
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        // find a point just outside of the object's collider
        Ray backRay = new(chosenSpot, -chosenDir.normalized);
        hideCol.Raycast(backRay, out RaycastHit info, rayLength);

        SetDestination(info.point + chosenDir.normalized * hideDistance);
    }

    void SetDestination(Vector3 pos)
    {
        destination = pos;
    }

    void MoveTowardsDestination()
    {

        destination.y = transform.position.y;

        transform.LookAt(destination);
        // transform.Translate(0, 0, speed * Time.deltaTime);
        agent.destination = destination;

    }

    // Line of Sight check
    bool CanSeeTarget()
    {
        RaycastHit raycastHit;
        Vector3 rayToTarget = target.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayToTarget, out raycastHit))
        {
            if (raycastHit.transform.gameObject.tag == "seeker")
            {
                return true;
            }
        }
        return false;
    }

    // move to a new location once touched by the Seeker
    public void Teleport()
    {
        bool valid = false;
        Vector3 position = transform.position;
        while (!valid)
        {
            Vector3 tentative = new(Random.Range(-maxDist, maxDist), transform.position.y, Random.Range(-maxDist, maxDist));
            // check chosen spot for colliders
            Collider[] colliders = Physics.OverlapSphere(tentative, 0.001f);
            if (colliders.Length <= 0 && Vector3.Distance(transform.position, tentative) > minTeleportDist)
            {
                valid = true;
                position = tentative;
            }
        }

        transform.position = position;
    }


}
