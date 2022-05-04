using Godot;
using MP.AnimatorWrappers;
using MP.Extensions;

namespace MP.FiniteStateMachine.Actions
{
    public abstract class SetAnimatorValue<T> : StateAction
    {
        [Export] private NodePath _animatedModelPath;
        [Export] protected string PropertyName;
        [Export] protected T Value;

        protected AnimatorSpatial AnimatedModel => _animatedModel;
        private AnimatorSpatial _animatedModel;
        
        public override void Init(StateMachine stateMachine)
        {
            this.TryGetNodeFromPath(_animatedModelPath, out _animatedModel);
        }
    }
}
