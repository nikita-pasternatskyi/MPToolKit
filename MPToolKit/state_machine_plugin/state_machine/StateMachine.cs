using Godot;
using System.Collections.Generic;
using MP.Extensions;

namespace MP.FiniteStateMachine
{
    public sealed class StateMachine : Node, IStateMachine
    {
        [Export] private NodePath _defaultStatePath;
        [Signal] private delegate Node StateChanged();

        private State _defaultState;
        private State _currentState;
        private List<Transition> _currentStateTransitions;
        private Dictionary<State, List<Transition>> _states;
        private Dictionary<System.Type, Node> _nodes;

        public T GetNodeOfType<T>() where T:Node
        {
            if(_nodes.TryGetValue(typeof(T), out Node value))
            {
                return (T)value;
            }

            if(this.TryGetNodeInMeAndParent<T> (out T res) == false)
            {
                GD.PrintErr($"No node of type {typeof(T)} was found!");
                return null;
            }
            _nodes.Add(typeof(T), res);
            return res;
        }

        public override void _Ready()
        {
            _nodes = new Dictionary<System.Type, Node>();

            _states = new Dictionary<State, List<Transition>>();
            this.TryGetNodeFromPath(_defaultStatePath, out _defaultState);

            if ((_defaultState is State) == false)
                throw new System.InvalidCastException(nameof(_defaultStatePath));

            foreach (var child in this.GetChildren<State>(true))
            {
                child.Init(this);
                _states.Add(child, child.GetTransitions(this));
            }
            ChangeState(_defaultState);
        }

        private (bool change, State newState) CheckTransitions()
        {
            foreach(var transition in _currentStateTransitions)
            {
                if (transition.Check() == true)
                    return (true, transition.ToState);
            }
            return (false, null);
        }

        public sealed override void _Process(float delta)
        {
            _currentState.Process(delta);
            var currentTransitionState = CheckTransitions();
            if (currentTransitionState.change == true)
                ChangeState(currentTransitionState.newState);
        }
        public sealed override void _PhysicsProcess(float delta)
        {
            if(_currentState == null)
            {
                GD.PrintErr("Current state is null!");
            }
            _currentState.PhysicsProcess(delta);
        }

        private void ChangeState(State newState)
        {
            if (_states.ContainsKey(newState) == false)
            {
                GD.Print($"{newState.Name} was not added to the dictionary! Therefore it is not this stateMachine state!");
                return;
            }

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
            _currentStateTransitions = _states[newState];
            EmitSignal(nameof(StateChanged), newState);
        }
    }
}