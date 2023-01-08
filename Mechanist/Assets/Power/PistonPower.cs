using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PistonPower : MonoBehaviour
{
    [SerializeField] private Vector3 _direction = Vector3.down;
    private Rigidbody _body;
    private Vector3 _initialPos;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _initialPos = transform.position;
        _initialPos.y = 10;
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        if ((pos - _initialPos).magnitude <= 0.2
            && Vector3.SignedAngle(_body.velocity, _direction, Vector3.forward) < 90
           )
        {
            _body.AddForce(500 * _body.mass * _direction, ForceMode.Impulse);
        }
    }
}