using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefabs List", menuName = "Scriptable Objects Lists/Prefabs List")]
public class PrefabsList : ScriptableObject
{
    [SerializeField] private List<GameObject> _prefabs;

    public List<GameObject> Prefabs => _prefabs;
}
