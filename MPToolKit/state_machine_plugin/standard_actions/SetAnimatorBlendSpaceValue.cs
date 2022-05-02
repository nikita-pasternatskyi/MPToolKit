using MP.AnimatorWrappers;

namespace MP.StateMachine.Actions
{
    public class SetAnimatorBlendSpaceValue : SetAnimatorValue<float>
    {
        public override void Act(float delta)
        {
            AnimatedModel.SetBlendPosition(PropertyName, Value);
        }
    }
}
