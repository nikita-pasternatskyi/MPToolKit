using Godot;

namespace MP.StateMachine
{
    public abstract class Condition : Node
    {
        [Export] private ConditionOperator _conditionOperator;
        [Export] private bool _awaitedCondition;

        public ConditionOperator ConditionOperator => _conditionOperator;

        public virtual void Init(StateMachine baseStateMachine) { }

        public virtual bool Check()
        {
            return ConditionCheck() == _awaitedCondition;
        }

        protected abstract bool ConditionCheck();
    }
}