using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
[RequireComponent(typeof(LineRenderer))]
public class Shooter : MonoBehaviour
{

    [SerializeField] private float fireInterval = 1f;       // can shoot once every fireInterval seconds

    [SerializeField] private AudioClip readyFireSFX;

    [SerializeField] private AudioClip noReadyFireSFX;
    [SerializeField] private AudioClip shootSFX;

    [SerializeField] private AudioClip missSFX;
    [SerializeField] private AudioClip hitSFX;

    [SerializeField] private AudioSource sfx;

    [SerializeField] private LineRenderer lineRenderer;

    private float curWait;                          // how long you have to wait until shooting again

    private bool readyToFire;

    private Camera cam;

    public bool hitSomething;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curWait = fireInterval - 0.1f;
        readyToFire = true;

        lineRenderer = GetComponent<LineRenderer>();

        cam = Camera.main;
        hitSomething = false;
    }

    // Update is called once per frame
    void Update()
    {
        curWait += Time.deltaTime;
        if (curWait > fireInterval && !readyToFire)
        {
            readyToFire = true;
            sfx.PlayOneShot(readyFireSFX);
        }

    }

    public RaycastHit TryShoot()
    {
        Debug.Log("Trying to shoot here!");
        if (readyToFire)
        {
            curWait = 0;
            readyToFire = false;
            sfx.PlayOneShot(shootSFX);
            return Shoot();
        }
        else
        {
            sfx.PlayOneShot(noReadyFireSFX);
        }
        return new RaycastHit();
    }

    private RaycastHit Shoot()
    {
        // find position in screen
        Vector3 point = new(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);

        hitSomething = Physics.Raycast(ray, out RaycastHit hit);

        if (hitSomething)
        {
            Debug.Log("Hit Something!");
            StartCoroutine(DrawTempLine(transform.position, hit.point, 2f));

            if (hit.collider != null && hit.collider.gameObject.CompareTag("hider"))
            {
                sfx.PlayOneShot(hitSFX);
            }
            else
            {
                Debug.Log("Miss SFX here");
                sfx.PlayOneShot(missSFX);
            }
        }
        else
        {
            Debug.Log("Did NOT hit something");
            Debug.Log("Miss SFX here");
            sfx.PlayOneShot(missSFX);
            StartCoroutine(DrawTempLine(transform.position, cam.transform.forward.normalized * 100f, 2f));
        }

        return hit;
    }

    public IEnumerator DrawTempLine(Vector3 point1, Vector3 point2, float duration)
    {
        lineRenderer.enabled = true;
        // draw line between point 1 and 2
        lineRenderer.SetPosition(0, point1);
        lineRenderer.SetPosition(1, point2);
        yield return new WaitForSeconds(duration);

        // hide line between point 1 and 2
        lineRenderer.enabled = false;

    }
}