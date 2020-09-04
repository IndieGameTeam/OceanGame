using UnityEngine;


public class Place
{
    public readonly int Id;
    public float Radius;
    public Vector3 Position;

    public Place(int id)
    {
        Id = id;

        Radius = 0;
        Position = Vector3.zero;
    }

    public Place(int id, float radius, Vector3 position)
    {
        Id = id;
        Radius = radius;
        Position = position;
    }

    public override bool Equals(object obj)
    {
        return obj is Place place && Id == place.Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
}