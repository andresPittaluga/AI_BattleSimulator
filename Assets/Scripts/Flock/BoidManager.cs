using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager Instance;

    public List<Boid> AllBoids { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        AllBoids = new List<Boid>();
    }

    //Ojo que puede agregar repetidos
    public void AddBoid(Boid b) => AllBoids.Add(b);
    public void RemoveBoid(Boid b)
    {
        if (AllBoids.Contains(b))
        {
            AllBoids.Remove(b);
        }
    }
}
