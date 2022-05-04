using Godot;
using MP.AnimatorWrappers;
using MP.Extensions;

namespace MP.FiniteStateMachine.Actions
{
    public abstract class SetAnimatorValue<T> : StateAction
    {
        [Export] private NodePath _animatorPath;
        [Export] protected string PropertyName;
        [Export] protected T Value;

        protected Animator Animator => _animator;
        private Animator _animator;
        
        public override void Init(StateMachine stateMachine)
        {
            GetNodeOrNull<Animator>(_animatorPath);
        }
    }
}
