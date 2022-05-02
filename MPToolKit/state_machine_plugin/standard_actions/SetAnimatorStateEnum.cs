using MP.AnimatorWrappers;

namespace MP.StateMachine.Actions
{
    public class SetAnimatorStateEnum : SetAnimatorValue<int>
    {
        public override void Act(float delta)
        {
            AnimatedModel.SetAnimatorEnum(PropertyName, Value);
        }
    }
}
