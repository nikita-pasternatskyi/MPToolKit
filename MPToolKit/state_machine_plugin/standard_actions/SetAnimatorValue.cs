using Godot;
using MP.AnimatorWrappers;
using MP.Extensions;

namespace MP.StateMachine.Actions
{
    public abstract class SetAnimatorValue<T> : StateAction
    {
        [Export] private NodePath _animatedModelPath;
        [Export] protected string PropertyName;
        [Export] protected T Value;

        protected Animator AnimatedModel => _animatedModel;
        private Animator _animatedModel;
        
        public override void Init(StateMachine stateMachine)
        {
            this.TryGetNodeFromPath(_animatedModelPath, out _animatedModel);
        }
    }
}
