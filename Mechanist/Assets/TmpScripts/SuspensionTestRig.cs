using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class SuspensionTestRig : MonoBehaviour
{
    [SerializeField] private float _maxLength = 0.5f;
    [SerializeField] private float _maxMoveSpeed = 0.01f;
    [SerializeField] private float _minMoveSpeed = 0.001f;

    private float _speed = 0;
    private float _initialHeight = 0;
    private float _maxHeight = 0;
    private float _targetHeight = 0;
    private float _direction = 1;

    private Rigidbody _body;

    void Start()
    {
        _initialHeight = transform.localPosition.y;
        _maxHeight = _initialHeight + _maxLength;

        _body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (
            (_direction > 0 && transform.localPosition.y >= _targetHeight)
            || (_direction < 0 && transform.localPosition.y <= _targetHeight)
        )
        {
            _targetHeight = Random.Range(_initialHeight, _maxHeight);
            _speed = Random.Range(_minMoveSpeed, _maxMoveSpeed);
        }

        _direction = Mathf.Sign(_targetHeight - transform.localPosition.y);
        Vector3 v = new Vector3(0, _speed * Time.fixedDeltaTime * _direction, 0);
        transform.Translate(v, Space.Self);
    }
}