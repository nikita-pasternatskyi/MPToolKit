using MP.AnimatorWrappers;

namespace MP.StateMachine.Actions
{
    public class SetAnimatorOneShotBoolean : SetAnimatorValue<bool>
    {
        public override void Act(float delta)
        {
            AnimatedModel.SetOneShotBool(PropertyName, Value);
        }
    }
}
