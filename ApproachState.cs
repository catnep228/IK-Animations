using UnityEngine;
using UnityEngine.UIElements;

public class ApproachState : EnvironmentInteractionsState
{
    float _elapsedTime = 0.0f;
    float _lerpDuration = 5.0f;
    float _approachDuration = .3f;
    float _approachWeight = 0.5f;
    float _approachRotationWeight = .75f;
    float _rotationSpeed = 500.0f;
    float _riseDistanceThreshold = 1f;
    
    public ApproachState(EnvironmentInteractionsContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
    }
    public override void EnterState() 
    {
        _elapsedTime = 0.0f;
        Debug.Log("APROACH STATE");
    }
    public override void ExitState() { }
    public override void UpdateState() 
    {
        Quaternion expectedGroundedRotation = Quaternion.LookRotation(-Vector3.up, Context.RootTransform.forward);
        _elapsedTime += Time.deltaTime;

        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation, expectedGroundedRotation, _rotationSpeed * Time.deltaTime);

        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight, _approachRotationWeight, _elapsedTime / _lerpDuration);
        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight, _approachWeight, _elapsedTime / _lerpDuration);

    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isOverStateLifeDuration = _elapsedTime >= _approachDuration;
        if (isOverStateLifeDuration || CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }

        bool isWithinArmsReach = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.CurrentShoulderTransform.position) < _riseDistanceThreshold;
        bool isClosestPointOnColliderReal = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;
        if (isClosestPointOnColliderReal && isWithinArmsReach)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Rise;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) 
    {
        StartIkTargetPositionTracking(other);
    }
    public override void OnTriggerStay(Collider other) 
    {
        UpdateIkTargetPosition(other);
    }
    public override void OnTriggerExit(Collider other) 
    {
        ResetIkTargetPositionTracking(other);    
    }
}
