using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects List", menuName = "Scriptable Objects Lists/Scriptable Objects List")]
public class ScriptableObjectsList : ScriptableObject
{
    [SerializeField] private List<ScriptableObject> _scriptableObjects;

    public List<ScriptableObject> ScriptableObjects => _scriptableObjects;
}
