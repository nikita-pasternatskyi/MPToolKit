using Godot;
using MP.Extensions;
using System.Linq;

namespace MP.StateMachine
{
    public enum ConditionOperator
    {
        And, Or
    }

    public sealed class Transition : Node
    {
        [Export] private NodePath _toStatePath;
       
        private Condition[] _conditions;
        private Condition[] _mandatoryConditions;

        public State ToState { get; private set; }

        public override void _Ready()
        {
            ToState = GetNode<State>(_toStatePath);
            this.Disable();
            var conditions = this.GetChildren<Condition>();
            _mandatoryConditions = conditions.Where((cond) => cond.ConditionOperator == ConditionOperator.And).ToArray();
            _conditions = conditions.Where((cond) => cond.ConditionOperator == ConditionOperator.Or).ToArray();
        }

        public bool Check()
        {
            foreach(var condition in _mandatoryConditions)
            {
                if (condition.Check() == false)
                    return false;
            }

            foreach(var condition in _conditions)
            {
                if (condition.Check() == true)
                    return true;
            }

            return false;
        }
    }
}