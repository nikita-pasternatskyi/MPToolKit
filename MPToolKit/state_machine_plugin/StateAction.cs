using Godot;
using MP.FiniteStateMachine;

public abstract class StateAction : Node
{
    [Export] public bool OnUpdate;
    [Export] public bool OnFixedUpdate;
    public virtual bool OnEnter { get => true; set { } }
    public virtual bool OnExit { get => true; set { } }

    public virtual void Init(StateMachine stateMachine)
    {

    }

    public abstract void Act(float delta);

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}

public abstract class InstantStateAction : StateAction
{
    [Export] public override bool OnEnter { get; set; }
    [Export] public override bool OnExit { get; set; }
}

