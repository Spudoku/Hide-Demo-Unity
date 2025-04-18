
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rayLength = hideDistance * 10f;
    }

    // Update is called once per frame
    void Update()
    {
        //Hide();
        CleverHide();
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
            //Debug.DrawLine(spot.transform.position, target.transform.position, Color.blue, 0.2f);
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

        Vector3 choice = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject obstacle = World.Instance.GetHidingSpots()[0];
        // loop through all hiding spots
        foreach (GameObject spot in World.Instance.GetHidingSpots())
        {
            //Debug.Log($"Trying {spot.name}");
            Vector3 dir = spot.transform.position - target.transform.position;
            //Debug.DrawLine(spot.transform.position, target.transform.position, Color.blue, 0.2f);
            Vector3 hidePos = spot.transform.position + dir.normalized * hideDistance;
            hidePos.y = transform.position.y;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                choice = hidePos;

                chosenDir = dir;
                obstacle = spot;
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        // backtracking raycast to get opposite side of collider
        Collider col = obstacle.GetComponent<Collider>();
        Ray backRay = new Ray(choice, -chosenDir.normalized);

        RaycastHit info;
        col.Raycast(backRay, out info, rayLength);

        Vector3 finalPoint = info.point + chosenDir.normalized * hideDistance;

        finalPoint.y = transform.position.y;
        Debug.DrawLine(transform.position, finalPoint, Color.cyan);
        SetDestination(finalPoint);

    }

    void SetDestination(Vector3 pos)
    {
        //Debug.DrawLine(transform.position, pos, Color.red, 5f);
        destination = pos;

    }

    void MoveTowardsDestination()
    {
        // Vector3 dir = destination - transform.position;

        // if (dir.magnitude > 0.01f)
        // {

        // }
        transform.LookAt(destination);
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
