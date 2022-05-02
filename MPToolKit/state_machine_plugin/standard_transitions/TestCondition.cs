using Godot;
namespace MP.StateMachine
{
    public class TestCondition : Condition
    {
        [Export] private bool _condition;

        protected override bool ConditionCheck()
        {
            return _condition;
        }
    }
}