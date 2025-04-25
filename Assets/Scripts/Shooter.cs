using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float fireInterval = 1f;       // can shoot once every fireInterval seconds

    [SerializeField] private AudioClip readyFire;
    [SerializeField] private AudioClip shoot;

    [SerializeField] private AudioClip miss;
    [SerializeField] private AudioClip hit;

    [SerializeField] private AudioSource sfx;

    private float curWait;                          // how long you have to wait until shooting again

    private bool readyToFire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curWait = fireInterval;
        readyToFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        curWait += Time.deltaTime;
        if (curWait > fireInterval && !readyToFire)
        {
            readyToFire = true;
        }

    }

    public bool TryShoot()
    {

        // TODO SHOOTING
        Debug.Log("Trying to shoot here!");
        curWait = 0;
        return true;
    }
}
