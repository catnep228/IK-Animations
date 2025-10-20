using UnityEngine;
using UnityEngine.UIElements;

public class ResetState : EnvironmentInteractionsState
{
    float _elapsedTime = 0.0f;
    float _resetDuration = 1f;
    float _lerpDuration = 10.0f;
    float _rotationSpeed = 500.0f;
    public ResetState(EnvironmentInteractionsContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate) 
    { 
        EnvironmentInteractionsContext Context = context;
    }
    public override void EnterState()
    {
        Debug.Log("NOW");
        _elapsedTime = 0.0f;
        Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        Context.CurrentIntersectingCollider = null;
    }
    public override void ExitState() { }
    public override void UpdateState() 
    {

        _elapsedTime += Time.deltaTime;
        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset, Context.ColliderCenterY, _elapsedTime / _lerpDuration);

        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight, 0, _elapsedTime / _lerpDuration);
        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight, 0, _elapsedTime / _lerpDuration);
        Context.CurrentIkTargetTransform.localPosition = Vector3.Lerp(Context.CurrentIkTargetTransform.localPosition, Context.CurrentOriginaltargetPosition, _elapsedTime / _lerpDuration);
        Context.CurrentIkTargetTransform.rotation = Quaternion.Lerp(Context.CurrentIkTargetTransform.rotation, Context.OriginalTargetRotation, _rotationSpeed * Time.deltaTime);
    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isMoving = Context.CharacterControler.velocity != Vector3.zero;
        if(_elapsedTime >= _resetDuration && isMoving)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;
        }
        return StateKey;
    } 
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
