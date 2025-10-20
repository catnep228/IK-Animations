using UnityEngine;

public class SearchState : EnvironmentInteractionsState
{
    public float _approachDistanceThreshold = 2.0f;
    public SearchState(EnvironmentInteractionsContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionsContext Context = context;
    }
    public override void EnterState() 
    {
        Debug.Log("ENTERING SEARCH STATE");
    }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        if (CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }
        bool isCloseToTarget = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.RootTransform.position) < _approachDistanceThreshold;
        bool isClosestPointOnColliderValid = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;
        if (isClosestPointOnColliderValid && isCloseToTarget)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Approach;
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
