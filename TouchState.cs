using TMPro;
using UnityEngine;

public class TouchState : EnvironmentInteractionsState
{
    public float _elapsedTime = 0.0f;
    public float _resetThreshold = 0.1f;
    public TouchState(EnvironmentInteractionsContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base
        (context, estate)
    {
        EnvironmentInteractionsContext Context = context;
    }
    public override void EnterState() 
    {
        _elapsedTime = 0.0f;
    }
    public override void ExitState() { }
    public override void UpdateState() 
    {
        _elapsedTime += Time.deltaTime;
    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        if (_elapsedTime > _resetThreshold || CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
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
