using MP.AnimatorWrappers;

namespace MP.FiniteStateMachine.Actions
{
    public class SetAnimatorOneShotBoolean : SetAnimatorValue<bool>
    {
        public override void Act(float delta)
        {
            AnimatedModel.SetOneShotBool(PropertyName, Value);
        }
    }
}
