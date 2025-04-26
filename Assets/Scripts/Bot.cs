

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


public class Bot : MonoBehaviour
{

    public GameObject target;                  // represents an object to avoid
    public float hideDistance;          // how far to stand next to an object
    public float speed;

    public Vector3 destination;

    private float rayLength;

    //public float cappedY = 1f;

    public float maxDist;        // max distance from 0,0
    public float minTeleportDist;

    NavMeshAgent agent;

    Renderer myRenderer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        World.Instance.Init();
        //myRenderer = GetComponent<Renderer>();
    }
    // // Update is called once per frame
    // void Update()
    // {
    //     //!myRenderer.isVisible
    //     if (CanSeeTarget())
    //     {
    //         CleverHide();
    //     }
    // }

    void LateUpdate()
    {
        // only move when not visible to the camera
        // trying to fix it 
        if (!myRenderer.isVisible)
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
            Debug.DrawRay(i.transform.position, (i.transform.position - target.transform.position).normalized * hideDistance, Color.yellow, 3f);

            // position for this object to hide
            Vector3 hidePos = i.transform.position + hideDir.normalized * hideDistance;
            //Debug.DrawLine(target.transform.position, i.transform.position, Color.red, 3f);
            Debug.DrawLine(transform.position, hidePos, Color.blue, 3f);
            hidePos.y = transform.position.y;

            // trying to prevent the seeker from being able to get between the hider and the hiding spot
            // if (Vector3.Distance(target.transform.position, hidePos) < Vector3.Distance(transform.position, hidePos))
            // {
            //     continue;
            // }

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = i;
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        //Collider hideCol = chosenGO.GetComponent<Collider>();
        // find a point just outside of the object's collider
        //Ray backRay = new(chosenSpot, -chosenDir.normalized);
        //hideCol.Raycast(backRay, out RaycastHit info, rayLength);


        Debug.DrawLine(transform.position, chosenSpot, Color.magenta, 3f);
        //SetDestination(info.point + chosenDir.normalized * hideDistance);
        SetDestination(chosenSpot);
    }

    void SetDestination(Vector3 pos)
    {
        destination = pos;
    }

    void MoveTowardsDestination()
    {

        destination.y = transform.position.y;

        //transform.LookAt(destination);
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

    bool CanBeSeen()
    {
        return true;
    }

    // move to a new location once touched by the Seeker
    public void Teleport()
    {
        bool valid = false;
        Vector3 position = transform.position;
        while (!valid)
        {
            Vector3 tentative = new(Random.Range(-maxDist, maxDist), transform.position.y + 5, Random.Range(-maxDist, maxDist));
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

    public void Init()
    {
        hideDistance = 3f;          // how far to stand next to an object
        speed = 20f;

        maxDist = 45f;        // max distance from 0,0
        minTeleportDist = 8f;

        agent = GetComponent<NavMeshAgent>();
        rayLength = hideDistance * 10f;

        // agent initialization
        agent.speed = speed;
        agent.radius = 3f;


    }
}
