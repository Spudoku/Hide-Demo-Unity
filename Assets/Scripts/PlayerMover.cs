
using UnityEngine;


// Slides the player object using input
public class PlayerMover : MonoBehaviour
{
    GUIStyle debugStyle;
    public float speed;
    public int score;
    public float time;

    public float cappedY = 1f;

    public float sensitivity = 1f;

    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        transform.position = new(transform.position.x, cappedY, transform.position.z);
        time += Time.deltaTime;
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");
        Vector3 moveDir = transform.forward * zMov + transform.right * xMov;
        moveDir.Normalize();

        rb.linearVelocity = moveDir * speed;


        // rotate based on mouse input

        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }

    void OnGUI()
    {
        debugStyle = new GUIStyle() { fontSize = 24 };
        GUI.Label(new Rect(10, 10, 500, 50), $"Score: {score}", debugStyle);
        GUI.Label(new Rect(10, 60, 500, 50), $"Time: {Mathf.Round(time)}", debugStyle);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Bot>(out var bot))
        {
            bot.Teleport();
            score++;
        }
    }
}
