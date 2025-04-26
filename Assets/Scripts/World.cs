using UnityEngine;

public sealed class World
{
    // Singleton pattern
    private static readonly World instance = new World();

    private static GameObject[] hidingSpots;

    static World()
    {
        instance.Init();
    }

    private World() { }

    public static World Instance
    {
        get { return instance; }
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }

    public void Init()
    {
        // create an accessible list of hiding spots
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

}
