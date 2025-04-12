/*

Based off rigidbody movement tutorial by catlikecoding at:

https://catlikecoding.com/unity/tutorials/movement/

*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyBallPlayer : MonoBehaviour
{
    [Header("Speed and acceleration")]
    [SerializeField, Range(0f, 100f), Tooltip("'WITH MAXIMUM VELOCITY' - Cuno")]
    float _maxSpeed = 10f, _maxClimbSpeed = 2;

    // ! Rework acceleration system, it isn't responsive enough
    [SerializeField, Range(0f, 1000f)]
    float _maxAcceleration = 10f, _maxAirAcceleration = 1f, _maxClimbAcceleration;

    [Header("Jumping")]
    [SerializeField, Range(0f, 10f)]
    float _maxJumpHeight = 2f;

    [SerializeField, Range(0, 5)]
    int _maxAirJumps = 0;

    [Header("Ground snapping")]
    [SerializeField, Range(0f, 100f)]
    [Tooltip("Defines the max speed for which object will be snapped to ground when moving off a ledger."
    + "Warning! Shouldn't be equal to MaxSpeed")]
    float _maxSnapSpeed = 100f;

    [SerializeField, Min(0f)]
    [Tooltip("Defines max snapping distance.")]
    float _maxSnapDistance = 1f;

    [SerializeField]
    [Tooltip("Defines layers to which controller can snap.")]
    LayerMask _snapProbeMask = -1;

    [Header("Stairs")]
    [SerializeField, Range(0, 90)]
    float _maxStairsAngle = 50f;

    [SerializeField]
    LayerMask _stairsMask = -1;

    [Header("cd")]
    [SerializeField, Range(0, 90)]
    float _maxGroundAngle = 25f;

    [SerializeField]
    bool _printDebugs = false;

    [SerializeField]
    Button _debugContinueButton;

    [SerializeField]
    [Tooltip("Used to determine the transformation space for user input controls.")]
    Transform playerInputSpace = default;

    [Header("Climbing")]
    [SerializeField, Range(90, 180)]
    float _maxClimbAngle = 140f;

    [SerializeField]
    LayerMask _climbMask = -1;

    [SerializeField]
    Transform _catModel;

    // TODO move signal cube freidns somewhere else
    [SerializeField]
    Transform _signalCube;
    
    /* 
     Interpolate mode of a Rigidbody. Setting it to Interpolate makes it linearly 
     interpolate between its last and current position, so it will lag a bit behind 
     its actual position according to PhysX. The other option is Extrapolate, which 
     interpolates to its guessed position according to its velocity, which is only
     really acceptable for objects that have a mostly constant velocity.
    */
    Rigidbody _body;
    MeshRenderer _meshRenderer;
    Vector2 _playerInput;
    
    Vector3 _velocity;
    Vector3 _desiredVelocity;
    Vector3 _contactNormal;
    Vector3 _steepContactNormal;
    Vector3 _climbNormal;
    Vector3 _lastClimbNormal;
    Vector3 _upAxis;
    Vector3 _rightAxis;
    Vector3 _forwardAxis;
    bool _desiredJump;
    int _jumpPhase;
    int _groundContactCount;
    int _steepContactCount;
    int _climbContactCount;
    float _minGroundDotProduct;
    float _minStairsDotProduct;
    float _minClimbDotProduct;

    // TODO Temporary only possibly
    List<ContactPoint> _currentContactPoints = new List<ContactPoint>(10);
    Collision _currentCollision;

    bool IsGrounded => _groundContactCount > 0;
    bool IsOnSteep => _steepContactCount > 0;
    bool IsClimbing => _climbContactCount > 0;

    void Awake()
    {
        // -------------

        //TODO move mouse management somewhere else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _body = GetComponent<Rigidbody>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();

        // Skipped curved world gravity
        _upAxis = Vector3.up;

        OnValidate();

        // -------------
    }

    void OnValidate()
    {
        // -------------

        // ? If we are then comparing it against the y component of normal vectors why do we use cos and not sin
        // ? I know it is dot product but
        _minGroundDotProduct = Mathf.Cos(_maxGroundAngle * Mathf.Deg2Rad);
        _minStairsDotProduct = Mathf.Cos(_maxStairsAngle * Mathf.Deg2Rad);
        _minClimbDotProduct = Mathf.Cos(_maxClimbAngle * Mathf.Deg2Rad);

        // -------------
    }

    void Update()
    {
        // -------------

        _playerInput.x = Input.GetAxis("Horizontal");
        _playerInput.y = Input.GetAxis("Vertical");
        _playerInput = Vector2.ClampMagnitude(_playerInput, 1f);

        // Update desired velocity on each frame
        // TODO 
        if (playerInputSpace)
        {
            _rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, _upAxis);
			_forwardAxis = ProjectDirectionOnPlane(playerInputSpace.forward, _upAxis);
        }
        else
        {
            _rightAxis = ProjectDirectionOnPlane(Vector3.right, _upAxis);
			_forwardAxis = ProjectDirectionOnPlane(Vector3.forward, _upAxis);
        }

        // * Cool use of OR assignment, this will only be able to set to true
        _desiredJump |= Input.GetButtonDown("Jump");

        UpdateBallColourDependingOnContact();

        // -------------
    }

    void FixedUpdate()
    {
        // -------------

        _velocity = _body.linearVelocity; // Reset velocity to recalculate it

        UpdateContactState();
        AdjustVelocity();

        if (_desiredJump)
        {
            _desiredJump = false;
            Jump();
        }

        if (IsClimbing)
        {
            // _velocity += _body.grav Physics.gravity * Time.deltaTime;
            _body.useGravity = false;
            _velocity -= _contactNormal * (_maxClimbAcceleration * 0.9f * Time.deltaTime);
        }
        else _body.useGravity = true;

        _body.linearVelocity = _velocity; // Assign calcuclated velocity

        ClearState();

        // -------------
    }

    void LateUpdate()
    {
        // TODO Obviously this can't stay like that
        // ! Handling cat model rotation
        Vector3 euler = Vector3.zero;

        if (_velocity != Vector3.zero)
            euler = new Vector3(0, Quaternion.LookRotation(_velocity).eulerAngles.y, 0);

        if ((euler != Vector3.zero))
            _catModel.rotation = Quaternion.Euler(euler);
    }

    void UpdateBallColourDependingOnContact()
    {
        if (IsClimbing)
        {
            // _meshRenderer.material.SetColor("_Color", Color.black);
            _signalCube.gameObject.SetActive(true);
            _signalCube.GetComponentInChildren<TextMeshPro>().text = "Climbing";
        }
        else if (IsOnSteep)
        {
            // _meshRenderer.material.SetColor("_Color", Color.black);
            _signalCube.gameObject.SetActive(true);
            _signalCube.GetComponentInChildren<TextMeshPro>().text = "Steep";
        }
        else
        {
            _signalCube.gameObject.SetActive(false);
        }
    }

    /// Update information about contacts that the body has with ground
    void UpdateContactState()
    {
        // -------------

        GetContactNormal();

        // if (CheckClimbing() || IsGrounded || SnapToGround())
        // {
        //     // All these checks to reset jump phase
        //     _jumpPhase = 0;
        // }
        // else
        // {
        //     // Reset contact normal
        //     // ! this is really misleading
        //     _contactNormal = _upAxis;
        // }

        // -------------
    }

    bool CheckClimbing () {
		if (IsClimbing) {
			if (_climbContactCount > 1) {
				_climbNormal.Normalize();
				float upDot = Vector3.Dot(_upAxis, _climbNormal);
				if (upDot >= _minGroundDotProduct) {
					_climbNormal = _lastClimbNormal;
				}
			}
			_groundContactCount = 1;
			_contactNormal = _climbNormal;
			return true;
		}
		return false;
	}

    /*
    Preventing "stumbling"
    Manage snapping to ground on low displacements due to hitting edges etc.
    */
    bool SnapToGround()
    {
        // -------------

        // * consider blocking that for a gliding goblin
        // TODO I really don't like this thingy, cryptiiic
        if (_jumpPhase > 0)
        {
            return false;
        }

        // We want to avoid snapping for greater speeds
        float speed = _velocity.magnitude;
        if (speed > _maxSnapSpeed)
        {
            return false;
        }

        /*
        Note that we're only considering a single point below us to decide whether we're above ground. 
        This works fine as long as the level geometry isn't too noisy nor too detailed. For example a 
        tiny deep crack could cause this to fail if the ray happened to be cast into it.
        
        If we don't hit anything, skip snapping
        */
        if (!Physics.Raycast(
            _body.position, -_upAxis, out RaycastHit hit,
            _maxSnapDistance, _snapProbeMask
        ))
        {
            return false;
        }

        if (hit.normal.y < GetMinGroundedDotByLayer(hit.collider.gameObject.layer))
        {
            return false;
        }

        _groundContactCount = 1;
        _contactNormal = hit.normal;

        float dot = Vector3.Dot(_velocity, hit.normal);
        // Only make changes to velocity if the _velocity and hit.normal point
        // in the same general direction (~UP)
        // this is because we don't want to readjust it if the case is of
        // free falling and velocity is already pulling somewhat down
        if (dot > 0f)
        {
            _velocity = (_velocity - hit.normal * dot).normalized * speed;
        }

        return true;

        // -------------
    }

    // TODO I don't like this part, too specific
    float GetMinGroundedDotByLayer(int layer)
    {
        // -------------

        return IsLayerInMask(layer, _stairsMask) ?
            _minGroundDotProduct : _minStairsDotProduct;

        // -------------
    }

    bool IsLayerInMask(int layer, LayerMask mask)
    {
        return !((mask & (1 << layer)) == 0);
    }

    /*
    Handling crevasses
    TODO Rename method, it is misleading 
    */
    bool GetContactNormal()
    {
        // -------------

        if (_steepContactCount > 1)
        {
            _steepContactNormal.Normalize();

            // TODO check situations where this would be still not satisfied and player would get stuck
            if (_steepContactNormal.y >= _minGroundDotProduct)
            {
                _groundContactCount = 1;
                _contactNormal = _steepContactNormal;
                return true;
            }
        }
        else if (_steepContactCount == 1)
        {
            // todo consider case when multiple contacts
            Debug.Log($"woof");
            
            _contactNormal = _steepContactNormal;
        }
        else
        {
            _contactNormal = _upAxis;
        }

        return false;

        // -------------
    }

    // Without this we will get weird boyancy effect on accelerating down the slope
    // We project the velocity along the slope we are moving over
    void AdjustVelocity()
    {
        // -------------
        float acceleration, maxSpeed;
        Vector3 xAxis, zAxis;
		// if (IsClimbing) {
        //     // TODO this doesnt work on slopes
        //     // ! Should instead depend on direction of looking
        //     acceleration = _maxClimbAcceleration;
		// 	maxSpeed = _maxClimbSpeed;
		// 	xAxis = Vector3.Cross(_contactNormal, Vector3.up);
		// 	zAxis = Vector3.up;
		// }
		// else {
            acceleration = IsGrounded ? _maxAcceleration : _maxAirAcceleration;
			maxSpeed = _maxSpeed;
			xAxis = _rightAxis;
			zAxis = _forwardAxis;
		// }

		xAxis = ProjectDirectionOnPlane(xAxis, _contactNormal);
		zAxis = ProjectDirectionOnPlane(zAxis, _contactNormal);

        _velocity = xAxis * _playerInput.x * maxSpeed + zAxis * _playerInput.y;

        return;

        // project the current velocity on both vectors to get the relative X and Z speeds.
        float currentX = Vector3.Dot(_velocity, xAxis);
        float currentZ = Vector3.Dot(_velocity, zAxis);

        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, _playerInput.x * maxSpeed, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, _playerInput.y * maxSpeed, maxSpeedChange);

        // _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        _velocity += xAxis * newX + zAxis * newZ;

        // -------------
    }

    void Jump()
    {
        // -------------

        Vector3 jumpDirection;

        // TODO This is the place to rework jump direction method
        if (IsGrounded)
        {
            // jumpDirection = _contactNormal;
            jumpDirection = _upAxis;
        }
        else if (IsOnSteep)
        {
            jumpDirection = _steepContactNormal;
        }
        else if (_jumpPhase <= _maxAirJumps)
        {
            // jumpDirection = _contactNormal;
            jumpDirection = _upAxis;
        }
        else
        {
            return;
        }

        //* Jump speed determined purely by max jump height
        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * _maxJumpHeight);

        // TODO This is a shitty way to avoid being vertically stuck when
        // TODO jumping between two vertical walls, create proper jump
        // TODO direction controlling.
        jumpDirection = (jumpDirection + Vector3.up).normalized;

        float alignedSpeed = Vector3.Dot(_velocity, jumpDirection);
        if (_velocity.y > 0)
        {
            // To limit the jump speed to its maximum
            // TODO no idea how it does what it's supposed to do
            // TODO just subtracting that dot product?
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        };

        // To jump away from ground surface.
        //* That is just a design decision about jump behaviour
        _velocity += jumpDirection * jumpSpeed;
        _jumpPhase++;

        // -------------
    }

    void ClearState()
    {
        // -------------

        _groundContactCount = _steepContactCount = _climbContactCount = 0;
        _contactNormal = _steepContactNormal = _climbNormal = Vector3.zero;

        // -------------
    }

    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        //* This skip is important to avoid IsGrounded overwriting at the beginning of a jump phase
        //todo this will require reworking on wall running probably
        if (_jumpPhase == 0)
        {
            EvaluateCollision(collision);
        }
    }

    void EvaluateCollision(Collision collision)
    {
        _currentCollision = collision;

        int layer = collision.gameObject.layer;
        float minDot = GetMinGroundedDotByLayer(layer);

        for (int i = 0; i < collision.contactCount; i++)
        {
            _currentContactPoints.Add(collision.GetContact(i));

            Vector3 normal = collision.GetContact(i).normal;

            if (normal.y >= minDot)
            {
                // Consider contacts satisfying ground angle condition
                _groundContactCount++;
                _contactNormal += normal; // ? shouldn't we normalize it at the end
            }
            else
            {
                if (normal.y > -0.01f)
                {
                    // Consider steep contacts which are still almost facing up
                    // Thus excluding overhangs (except this tiny margin of -0.01f) and ceilings
                    _steepContactCount++;
                    _steepContactNormal += normal;
                }

                // TODO Rework so climbing isnt so "gluey" as soon as you get close to a climb surface
                if (normal.y >= _minClimbDotProduct && IsLayerInMask(layer, _climbMask))
                {
                    _climbContactCount += 1;
                    _climbNormal += normal;
                    // TODO Add moveable surfaces
                    // connectedBody = collision.rigidbody;
                }
            }
        }

        if (_groundContactCount > 1)
        {
            _contactNormal.Normalize();
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position

        for (int i = 0; i < _currentContactPoints.Count; i++)
        {
            ContactPoint pt = _currentContactPoints[i];

            Vector3 normal = pt.normal;
            Vector3 position = pt.point;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(position, position + (normal * 0.2f));
        }

        var sphere = GetComponent<SphereCollider>();

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + sphere.center, transform.position + sphere.center + (_body.linearVelocity * 0.2f));

        _currentContactPoints.Clear();
    }

    static Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 planeNormal)
    {
        // todo Refresh on dot product:)
        return (direction - planeNormal * Vector3.Dot(direction, planeNormal)).normalized;
    }

    void DebugStep(string message, Color color, bool pauseGame = false)
    {
        if (_printDebugs)
        {
            // Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");

            if (pauseGame)
            {
                float fixedDeltaTime = Time.fixedDeltaTime;
                float timeScale = Time.timeScale;

                _debugContinueButton.gameObject.SetActive(true);
                _debugContinueButton.onClick.RemoveAllListeners();
                _debugContinueButton.onClick.AddListener(() =>
                {
                    Time.fixedDeltaTime = fixedDeltaTime;
                    Time.timeScale = timeScale;
                    _debugContinueButton.gameObject.SetActive(false);
                });

                Time.fixedDeltaTime = 0f;
                Time.timeScale = 0f;
            }
        }
    }
}
