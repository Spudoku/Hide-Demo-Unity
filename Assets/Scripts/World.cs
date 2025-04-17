using UnityEngine;

public sealed class World
{
    private static readonly World instace = new World();

    private static GameObject[] hidingSpots;

    static World()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

}
