using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] private Transform _target; 
    [SerializeField] private int _maxPoints = 7; 
    [SerializeField] private float _pointSpacing = 0.5f; 

    private LineRenderer _lineRenderer;
    private Vector3[] _positions; 
    private int _currentPointIndex = 0;

    private void OnValidate()
    {
        if (_target == null)
            _target = transform.parent.parent;
    }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _positions = new Vector3[_maxPoints];
    }

    private void Start()
    {
        for (int i = 0; i < _maxPoints; i++)
        {
            _positions[i] = _target.position;
        }
        UpdateLineRenderer();
    }

    private void Update()
    {
        if (_target == null) return;

        if (Vector3.Distance(_positions[_currentPointIndex], _target.position) >= _pointSpacing)
        {
            AddPoint(_target.position);
        }
    }

    private void AddPoint(Vector3 newPoint)
    {
        for (int i = 0; i < _maxPoints - 1; i++)
        {
            _positions[i] = _positions[i + 1];
        }

        _positions[_maxPoints - 1] = newPoint;

        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = _maxPoints;
        _lineRenderer.SetPositions(_positions);
    }
}
