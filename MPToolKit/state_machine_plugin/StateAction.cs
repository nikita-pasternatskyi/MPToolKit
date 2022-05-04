using Godot;
using MP.FiniteStateMachine;

public abstract class StateAction : Node
{
    [Export] public bool OnEnter;
    [Export] public bool OnExit;
    [Export] public bool OnUpdate;
    [Export] public bool OnFixedUpdate;

    public virtual void Init(StateMachine stateMachine)
    {

    }

    public abstract void Act(float delta);

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}

