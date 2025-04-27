
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


// Slides the player object using input
public class PlayerMover : MonoBehaviour
{
    GUIStyle debugStyle;
    public float speed;
    public int score;
    public float time;

    public float cappedY = 1f;

    public float sensitivity = 1f;
    public float vertSensitivity = 1f;

    public HiderManager hiderManager;

    Camera cam;

    private float maxVert = -90f;
    private float minVert = 45f;

    private float vertRot = 0f;

    private Shooter shooter;

    private bool hasCollided = false;

    // public Shooter shooter;      // class that handles shooting

    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;

        shooter = GetComponent<Shooter>();
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
        // TODO: rotate camera vertically based on Mouse Y
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);

        //cam.transform.Rotate(, 0, 0);

        vertRot += Input.GetAxis("Mouse Y") * -vertSensitivity;
        vertRot = Mathf.Clamp(vertRot, maxVert, minVert);

        cam.transform.localEulerAngles = new(vertRot, 0, 0);

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit = shooter.TryShoot();

            if (hit.collider != null && hit.collider.gameObject.CompareTag("hider"))
            {
                RemoveHider(hit.collider.gameObject);
            }
            else
            {
                // penalty for missing
            }
        }
    }

    void OnGUI()
    {
        debugStyle = new GUIStyle() { fontSize = 24 };
        GUI.Label(new Rect(10, 10, 500, 50), $"Hiders Left: {hiderManager.hiderCount}", debugStyle);
        GUI.Label(new Rect(10, 60, 500, 50), $"Time: {Mathf.Round(time)}", debugStyle);
        GUI.Label(new Rect(cam.pixelWidth / 2, cam.pixelHeight / 2, 10, 10), "+", debugStyle);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && collision.gameObject.CompareTag("hider"))
        {
            hasCollided = true;
            //bot.Teleport();
            RemoveHider(collision.gameObject);

        }
    }

    void RemoveHider(GameObject go)
    {
        Destroy(go);
        hiderManager.hiderCount--;

        if (hiderManager.hiderCount <= 0)
        {
            // reset
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    void OnCollisionExit(Collision collision)
    {
        hasCollided = false;
    }
}
