using System;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public event Action<Egg> EggAdded;
    public event Action<Egg> EggRemoved;

    private List<Egg> _eggs = new();

    public IReadOnlyList<Egg> Eggs => _eggs;

    private void Awake()
    {
        Egg egg = gameObject.AddComponent<Egg>();

        _eggs.Add(egg);
    }

    public void AddEgg(Egg egg)
    {
        _eggs.Add(egg);
        EggAdded?.Invoke(egg);
    }

    public void RemoveEgg(Egg egg)
    {
        _eggs.Remove(egg);
        EggRemoved?.Invoke(egg);
    }
}
