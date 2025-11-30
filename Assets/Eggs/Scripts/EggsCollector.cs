using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class EggsCollector : MonoBehaviour
{
    public event Action<Egg> EggCollected;

    [SerializeField, Range(1, 10)] private int _maxEggsCount;
    [SerializeField] private Button _collectButton;

    private readonly List<Egg> _collectedEggs = new();
    private Nest _nearestNest;

    private void Awake()
    {
        _collectButton.onClick.AddListener(TryCollect);
    }

    private void OnDestroy()
    {
        _collectButton.onClick.RemoveListener(TryCollect);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Nest nest))   
            _nearestNest = nest;

        UpdateCollectButtonVisibility();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Nest _))
            _nearestNest = null;

        UpdateCollectButtonVisibility();
    }

    public IReadOnlyList<Egg> GetCollectedEggs() => _collectedEggs;

    public void ClearCollectedEggs() => _collectedEggs.Clear();

    private void TryCollect()
    {
        if (_collectedEggs.Count() >= _maxEggsCount)
            return;

        Egg nearestEgg = GetNearestEgg();

        if (nearestEgg == null)
            return;

        _collectedEggs.Add(nearestEgg);
        _nearestNest.RemoveEgg(nearestEgg);
        EggCollected?.Invoke(nearestEgg);

        Debug.Log("Egg collected");
        UpdateCollectButtonVisibility();
    }

    private Egg GetNearestEgg()
    {
        if (_nearestNest == null)
            return null;
        
        Egg egg = _nearestNest.Eggs
            .OrderBy(egg => (egg.transform.position - transform.position).magnitude)
            .First();

        return egg;
    }

    private void UpdateCollectButtonVisibility()
    {
        bool canCollectAnyEgg = _nearestNest?.Eggs.Any() == true;
        _collectButton.gameObject.SetActive(canCollectAnyEgg);
    }
}
