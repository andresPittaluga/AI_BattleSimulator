using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory<T> : MonoBehaviour
{
    [SerializeField] protected int _startAmount = 25;
    public abstract T Create();
}
