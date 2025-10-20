using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset,
    }
    private EnvironmentInteractionsContext _context;

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private CharacterController _characterControler;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, .03f);
        }
    }

    private void Awake()
    {
        VallidationConstraints();
        _context = new EnvironmentInteractionsContext(_leftIkConstraint, _rightIkConstraint, _leftMultiRotationConstraint, _rightMultiRotationConstraint, _characterControler , _rootCollider, transform.root);
        InitializedSates();
        ConstructEnvironmentDetectionCollider();

    }

    private void VallidationConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "leftIK");
        Assert.IsNotNull(_rightIkConstraint, "rightIK");
        Assert.IsNotNull(_leftMultiRotationConstraint, "leftMulti");
        Assert.IsNotNull(_rightMultiRotationConstraint, "rightMulti");
        Assert.IsNotNull(_characterControler, "CharacterControler");
        Assert.IsNotNull(_rootCollider, "RootCollider");

    }

    private void InitializedSates()
    {
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));
        CurrentState = States[EEnvironmentInteractionState.Reset];
        
    }
    private void ConstructEnvironmentDetectionCollider()
    {
        float wingspan = _rootCollider.height;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3 (wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_rootCollider.center.x, _rootCollider.center.y + (.25f * wingspan), _rootCollider.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        _context.ColliderCenterY = _rootCollider.center.y;
    }
}

