using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidBody;

    public Vector3 _input;
    public float _moveDistance;
    public float _movementSpeed = 5f;
    public float _margin = 0.2f;
    
    private Vector3 halfExtends = new Vector3( 0.5f, 0.5f, 0.5f );
    private RaycastHit _hitInfo;
    private LayerMask _boxesLayer;
    
    // Jump related
    public bool _jumping = false;
    public float _jumpHeight = 0f;
    public float _maxJumpHeight = 0f;

    public bool _holdingOntoLedge = false;
    public bool topBlocked, frontBlocked, ledge;

    public Transform cubeVisualization;

    // Use this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _boxesLayer = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        if ( !_holdingOntoLedge )
            Movement();
        
        if ( !_jumping && Input.GetKeyDown( KeyCode.Space ) )
        {
            Jump();
        }
        else if ( _jumping )
        {
            CalculateJumpHeight();

            if ( LedgeGrabAllowed() )
            {
                GrabLedge();
            }
        }
    }

    private bool LedgeGrabAllowed()
    {
        if (_rigidBody.velocity.y > 0.5f ||
            _jumpHeight < 0.5f ||
            _input.magnitude < 0.1f)
        {
            return false;
        }

        RaycastHit[] topHits = Physics.BoxCastAll(center: transform.position + Vector3.up * 1.2f,
                                                   halfExtents: halfExtends,
                                                   direction: _input,
                                                   orientation: transform.rotation,
                                                   maxDistance: 0.3f,
                                                   layermask: _boxesLayer);

        if (topHits.Length >= 1)
        {
            topBlocked = true;
            return false;
        }
        else
            topBlocked = false;

        frontBlocked = Physics.BoxCast(center: transform.position + Vector3.up * 1.2f,
                                          halfExtents: halfExtends,
                                          direction: _input,
                                          orientation: transform.rotation,
                                          maxDistance: 0.3f,
                                       layerMask: _boxesLayer);

        if (frontBlocked)
            return false;

        ledge = Physics.BoxCast(center: transform.position + Vector3.up * 1.2f + _input * 0.3f,
                                      halfExtents: halfExtends,
                                      direction: Vector3.down,
                                      hitInfo: out _hitInfo,
                                      orientation: transform.rotation,
                                      maxDistance: 0.5f,
                                      layerMask: _boxesLayer);

        if (ledge)
        {
            cubeVisualization.gameObject.SetActive(true);
            cubeVisualization.rotation = transform.rotation;
            cubeVisualization.position = transform.position + Vector3.up * 1.2f + _input * 0.3f;

            cubeVisualization.position += _hitInfo.distance * Vector3.down;

            return true;
        }
        else
            return false;
    }

    private void GrabLedge()
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        _rigidBody.useGravity = false;
        _jumping = false;
        _holdingOntoLedge = true;
    }

    private void Movement()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.z = Input.GetAxis("Vertical");

        if ( _input.magnitude > 0.1f )
        {
            _input.Normalize();

            bool boxHit = Physics.BoxCast( center: transform.position,
                                           halfExtents: halfExtends,
                                           direction: _input,
                                           hitInfo: out _hitInfo,
                                           orientation: transform.rotation,
                                           maxDistance: _movementSpeed * Time.deltaTime + _margin);

            if (boxHit)
            {
                _moveDistance = _hitInfo.distance - _margin;
            }
            else
            {
                _moveDistance = _movementSpeed * Time.deltaTime;
            }

            transform.position += _moveDistance * _input;
        }
    }

    private void Jump()
    {
        _rigidBody.AddForce(Vector3.up * 5.0f, ForceMode.VelocityChange);
        _jumping = true;
        _rigidBody.useGravity = true;
        _holdingOntoLedge = false;
        cubeVisualization.gameObject.SetActive(false);
    }

    private void CalculateJumpHeight()
    {
        RaycastHit[] boxHits = Physics.BoxCastAll(center: transform.position,
                                                   halfExtents: halfExtends,
                                                   direction: Vector3.down,
                                                   orientation: transform.rotation,
                                                   maxDistance: 10f);

        if (boxHits.Length >= 1)
        {
            _jumpHeight = boxHits[0].distance;

            if (_jumpHeight > _maxJumpHeight)
            {
                _maxJumpHeight = _jumpHeight;
            }

            if (_rigidBody.velocity.y < -0.1f &&
                 _jumpHeight < 0.2f)
            {
                _jumping = false;
            }
        }
    }
}
