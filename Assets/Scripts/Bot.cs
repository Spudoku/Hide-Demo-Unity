
using System.Diagnostics.Contracts;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rayLength = hideDistance * 10f;
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

    void Hide()
    {
        float dist = Mathf.Infinity;

        Vector3 choice = Vector3.zero;

        // loop through all hiding spots

        foreach (GameObject spot in World.Instance.GetHidingSpots())
        {
            Vector3 dir = spot.transform.position - target.transform.position;
            Debug.DrawLine(spot.transform.position, target.transform.position, Color.blue, 5f);
            Vector3 hidePos = spot.transform.position + dir.normalized * hideDistance;
            hidePos.y = transform.position.y;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                choice = hidePos;
                dist = Vector3.Distance(spot.transform.position, hidePos);
            }
        }
        SetDestination(choice);

    }

    void CleverHide()
    {

        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        foreach (GameObject i in World.Instance.GetHidingSpots())
        {
            Debug.Log(i.name);
            Vector3 hideDir = i.transform.position - target.transform.position;
            Vector3 hidePos = i.transform.position + hideDir.normalized * hideDistance;
            hidePos.y = transform.position.y;
            Debug.DrawRay(target.transform.position, hideDir);
            Debug.DrawLine(transform.position, hidePos, Color.red);
            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = i;
                dist = Vector3.Distance(transform.position, hidePos);
                Debug.Log($"{i.name} is the closest!");
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        hideCol.Raycast(backRay, out info, rayLength);

        SetDestination(info.point + chosenDir.normalized * hideDistance);


    }

    void SetDestination(Vector3 pos)
    {
        //Debug.DrawLine(transform.position, pos, Color.red, 5f);
        destination = pos;

    }

    void MoveTowardsDestination()
    {

        destination.y = transform.position.y;

        transform.LookAt(destination);
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

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
