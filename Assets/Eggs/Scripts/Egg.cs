using System;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public event Action<Egg> Collected;

    public void Collect()
    {
        gameObject.SetActive(false);
        Collected?.Invoke(this);
    }
}
