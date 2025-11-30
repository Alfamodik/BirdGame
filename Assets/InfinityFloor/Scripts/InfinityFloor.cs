using UnityEngine;

public class InfinityFloor : MonoBehaviour
{
    [SerializeField] private int _gridSize;
    [SerializeField] private Transform _target;
    [SerializeField] private ScriptableObjectsList _prefabsScriptableObjectsList;

    [SerializeField] private int _currentLevel;

    private float _tileSize;
    private Vector3 _matrixOffset;

    private Transform[,] _tiles;
    private GameObject[] _tilePrefabs;

    private void Start()
    {
        _tilePrefabs = (_prefabsScriptableObjectsList
            .ScriptableObjects[_currentLevel] as PrefabsList)
            .Prefabs.ToArray();

        InstantiateMatrix();
    }

    private void InstantiateMatrix()
    {
        _matrixOffset = Vector3.zero;
        _tiles = new Transform[_gridSize, _gridSize];
        _tileSize = _tilePrefabs[0].GetComponent<MeshRenderer>().bounds.size.x;

        for (int x = -_gridSize / 2; x <= _gridSize / 2; x++)
        {
            for (int z = -_gridSize / 2; z <= _gridSize / 2; z++)
            {
                Vector3 spawnPoint = new(_target.position.x + x * _tileSize, 0f, _target.position.z + z * _tileSize);
                Transform tile = Instantiate(GetRandomTilePrefab(), spawnPoint, Quaternion.identity, transform).transform;
                tile.gameObject.name += $"x={x};z={z}";
                _tiles[x + _gridSize / 2, z + _gridSize / 2] = tile;
            }
        }
    }

    private void Update()
    {
        if (_target == null)
            return;

        if (_target.position.x > _matrixOffset.x + _tileSize)
        {
            _matrixOffset.x += _tileSize;
            ShiftMatrix(ShiftDirection.Right);
        }
        else if (_target.position.x < _matrixOffset.x - _tileSize)
        {
            _matrixOffset.x -= _tileSize;
            ShiftMatrix(ShiftDirection.Left);
        }

        if (_target.position.z > _matrixOffset.z + _tileSize)
        {
            _matrixOffset.z += _tileSize;
            ShiftMatrix(ShiftDirection.Up);
        }
        else if (_target.position.z < _matrixOffset.z - _tileSize)
        {
            _matrixOffset.z -= _tileSize;
            ShiftMatrix(ShiftDirection.Down);
        }
    }

    private GameObject GetRandomTilePrefab()
        => _tilePrefabs[Random.Range(0, _tilePrefabs.Length)];

    private void ShiftMatrix(ShiftDirection direction)
    {
        switch (direction)
        {
            case ShiftDirection.Left:
                for (int z = 0; z < _gridSize; z++)
                {
                    Transform temp = _tiles[_gridSize - 1, z];
                    for (int x = _gridSize - 1; x > 0; x--)
                    {
                        _tiles[x, z] = _tiles[x - 1, z];
                    }
                    _tiles[0, z] = temp;
                    _tiles[0, z].position += Vector3.left * _gridSize * _tileSize;
                }
                break;

            case ShiftDirection.Right:
                for (int z = 0; z < _gridSize; z++)
                {
                    Transform temp = _tiles[0, z];
                    for (int x = 0; x < _gridSize - 1; x++)
                    {
                        _tiles[x, z] = _tiles[x + 1, z];
                    }
                    _tiles[_gridSize - 1, z] = temp;
                    _tiles[_gridSize - 1, z].position += Vector3.right * _gridSize * _tileSize;
                }
                break;

            case ShiftDirection.Up:
                for (int x = 0; x < _gridSize; x++)
                {
                    Transform temp = _tiles[x, 0];
                    for (int z = 0; z < _gridSize - 1; z++)
                    {
                        _tiles[x, z] = _tiles[x, z + 1];
                    }
                    _tiles[x, _gridSize - 1] = temp;
                    _tiles[x, _gridSize - 1].position += Vector3.forward * _gridSize * _tileSize;
                }
                break;

            case ShiftDirection.Down:
                for (int x = 0; x < _gridSize; x++)
                {
                    Transform temp = _tiles[x, _gridSize - 1];
                    for (int z = _gridSize - 1; z > 0; z--)
                    {
                        _tiles[x, z] = _tiles[x, z - 1];
                    }
                    _tiles[x, 0] = temp;
                    _tiles[x, 0].position += Vector3.back * _gridSize * _tileSize;
                }
                break;
        }
    }

    private enum ShiftDirection
    {
        Left,
        Right,
        Up,
        Down
    }
}
