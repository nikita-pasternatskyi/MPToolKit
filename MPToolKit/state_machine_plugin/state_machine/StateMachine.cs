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
        private Transitions _currentStateTransitions;
        private Dictionary<State, Transitions> _states;
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

            _states = new Dictionary<State, Transitions>();
            this.TryGetNodeFromPath(_defaultStatePath, out _defaultState);

            if ((_defaultState is State) == false)
                throw new System.InvalidCastException(nameof(_defaultStatePath));

            foreach (var child in this.GetChildren<State>())
            {
                List<Transition> stateTransitions = new List<Transition>();

                var transitionsNode = child.FindNode("Transitions", false);
                var target = transitionsNode == null ? child : transitionsNode;

                var children = target.GetChildren<Transition>();

                if(children.IsEmpty())
                {
                    GD.Print($"{child.Name} State has no transitions!");
                }

                foreach (var transition in children)
                {
                    transition.Init(this);
                    stateTransitions.Add(transition);
                }

                child.Init(this);
                _states.Add(child, new Transitions(stateTransitions));
            }
            ChangeState(_defaultState);
        }

        public sealed override void _Process(float delta)
        {
            _currentState.Process(delta);
            var currentTransitionState = _currentStateTransitions.Check();
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