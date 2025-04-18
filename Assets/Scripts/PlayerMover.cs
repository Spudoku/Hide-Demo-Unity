using UnityEngine;


// Slides the player object using input
public class PlayerMover : MonoBehaviour
{
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");
        //Vector3 totalMov = new Vector3(xMov, 0f, zMov).normalized * speed * Time.deltaTime;
        //transform.position += totalMov;
        transform.Translate(speed * Time.deltaTime * new Vector3(xMov, 0, zMov).normalized, Space.World);
    }
}
