using UnityEngine;


// Rotate the camera using mouse input around a target
// (the player)
public class CamOrbit : MonoBehaviour
{
    [SerializeField] GameObject target;

    public float targetYOffset;
    public float targetHorizOffset;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = target.transform.position;
        // turn clockwise if mouse moves left, counterclockwise
        // if moves right
        Vector3 horizOffset = -target.transform.forward.normalized * targetHorizOffset;
        Vector3 offset = new(horizOffset.x, targetYOffset, horizOffset.z);
        position += offset;



        transform.position = position;
        transform.LookAt(target.transform.position);
    }
}
