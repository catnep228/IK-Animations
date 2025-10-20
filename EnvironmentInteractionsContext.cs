using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionsContext
{
    public enum EBodySide
    {
        RIGHT,
        LEFT
    }

    private TwoBoneIKConstraint _leftIkConstraint;
    private TwoBoneIKConstraint _rightIkConstraint;
    private MultiRotationConstraint _leftMultiRotationConstraint;
    private MultiRotationConstraint _rightMultiRotationConstraint;
    private CharacterController _characterControler;
    private CapsuleCollider _rootCollider;
    private Transform _rootTransform;
    private Vector3 _leftOriginalTargetPosition;
    private Vector3 _rightOriginalTargetPosition;


    public EnvironmentInteractionsContext(TwoBoneIKConstraint leftIkConstraint, TwoBoneIKConstraint rightIkConstraint, MultiRotationConstraint leftMultiRotationConstraint, MultiRotationConstraint rightMultiRotationConstraint, CharacterController characterController, CapsuleCollider rootCollider, Transform rootTransform)
    {
        _leftIkConstraint = leftIkConstraint;
        _rightIkConstraint = rightIkConstraint;
        _leftMultiRotationConstraint = leftMultiRotationConstraint;
        _rightMultiRotationConstraint = rightMultiRotationConstraint;
        _characterControler = characterController;
        _rootCollider = rootCollider;
        _rootTransform = rootTransform;
        _leftOriginalTargetPosition = _leftIkConstraint.data.target.transform.localPosition;
        _rightOriginalTargetPosition = _rightIkConstraint.data.target.transform.localPosition;
        OriginalTargetRotation = _leftIkConstraint.data.target.rotation;

        CharacterShoulderHeight = leftIkConstraint.data.root.transform.position.y;
        SetCurrentSide(Vector3.positiveInfinity);
    }

    public TwoBoneIKConstraint LeftIkConstraint => _leftIkConstraint;
    public TwoBoneIKConstraint RightIkConstraint => _rightIkConstraint;
    public MultiRotationConstraint LeftMultiRotationConstraint => _leftMultiRotationConstraint;
    public MultiRotationConstraint RightMultiRotationConstraint => _rightMultiRotationConstraint;
    public CharacterController CharacterControler => _characterControler;
    public CapsuleCollider RootCollider => _rootCollider;
    public Transform RootTransform => _rootTransform;


    public Collider CurrentIntersectingCollider { get; set; }
    public TwoBoneIKConstraint CurrentIkConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRotationConstraint { get; private set; }
    public Transform CurrentIkTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public EBodySide CurrentBodySide { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    public float CharacterShoulderHeight {get; set;}
    public float InteractionPointYOffset { get; set; } = 0f;
    public float ColliderCenterY { get; set; }
    public Vector3 CurrentOriginaltargetPosition { get; private set; }
    public Quaternion OriginalTargetRotation { get; private set; }
    public float LowestDistance {  get; set; } = Mathf.Infinity;

    public void SetCurrentSide(Vector3 positionToCheck)
    {
        Vector3 leftShoulder = _leftIkConstraint.data.root.transform.position;
        Vector3 rightShoulder = _rightIkConstraint.data.root.transform.position;

        bool isLeftCloser = Vector3.Distance(positionToCheck, leftShoulder) < Vector3.Distance(positionToCheck, rightShoulder);
        if (isLeftCloser)
        {
            Debug.Log("LEFT SIDE IS CLOSER");
            CurrentBodySide = EBodySide.LEFT;
            CurrentIkConstraint = _leftIkConstraint;
            CurrentMultiRotationConstraint = _leftMultiRotationConstraint;
            CurrentOriginaltargetPosition = _leftOriginalTargetPosition;
            Debug.Log(CurrentIkConstraint);
        }
        else
        {
            Debug.Log("RIGHT SIDE IS CLOSER");
            CurrentBodySide = EBodySide.RIGHT;
            CurrentIkConstraint = _rightIkConstraint;
            CurrentMultiRotationConstraint = _rightMultiRotationConstraint;
            CurrentOriginaltargetPosition = _rightOriginalTargetPosition;
            Debug.Log(CurrentIkConstraint);
        }
        CurrentShoulderTransform = CurrentIkConstraint.data.root.transform;
        CurrentIkTargetTransform = CurrentIkConstraint.data.target.transform;
    }
}