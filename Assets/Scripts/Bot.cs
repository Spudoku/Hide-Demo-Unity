
using TMPro;
using UnityEngine;


public class Bot : MonoBehaviour
{

    [SerializeField] GameObject target;                  // represents an object to avoid
    public float hideDistance;          // how far to stand next to an object
    public float speed;

    public Vector3 destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Hide();
        MoveTowardsDestination();
    }

    void Hide()
    {
        float dist = Mathf.Infinity;

        Vector3 choice = Vector3.zero;

        // loop through all hiding spots
        // for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        // {
        //     Vector3 dir = World.
        // }
        foreach (GameObject spot in World.Instance.GetHidingSpots())
        {
            Vector3 dir = spot.transform.position - target.transform.position;
            Vector3 hidePos = spot.transform.position + dir.normalized * hideDistance;
            hidePos.y = transform.position.y;

            if (Vector3.Distance(spot.transform.position, hidePos) < dist)
            {
                choice = hidePos;
                dist = Vector3.Distance(spot.transform.position, hidePos);
            }
        }
        SetDestination(choice);

    }

    void SetDestination(Vector3 pos)
    {
        Debug.DrawLine(transform.position, pos, Color.red, 5f);
        destination = pos;

    }

    void MoveTowardsDestination()
    {
        Vector3 dir = destination - transform.position;

        transform.LookAt(destination);

        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
