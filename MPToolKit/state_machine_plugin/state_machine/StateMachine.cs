using Godot;
using System.Collections.Generic;
using MP.Extensions;
using System.Linq;

namespace MP.FiniteStateMachine
{
    public sealed class StateMachine : Node, IStateMachine
    {
        [Export] private NodePath _defaultStatePath;
        [Signal] private delegate Node StateChanged();

        private bool _isSubStateMachine = false;
        private StateMachine _currentSubStateMachine;
        private State _defaultState;
        private State _currentState;
        private IReadOnlyList<Transition> _currentStateTransitions;
        private Dictionary<System.Type, Node> _nodes = new Dictionary<System.Type, Node>();

        private List<StateAction> _enterActions = new List<StateAction>();
        private List<StateAction> _updateActions = new List<StateAction>();
        private List<StateAction> _fixedUpdateActions = new List<StateAction>();
        private List<StateAction> _exitActions = new List<StateAction>();

        public override void _Ready()
        {
            var def = GetNode(_defaultStatePath);

            if((def is State) == false)
                throw new System.InvalidCastException(nameof(_defaultStatePath));

            _defaultState = def as State;

            foreach (var child in this.GetChildren<State>(false))
            {
                child.Init(this);
            }

            var actionsNode = FindNode("Actions", false);
            var target = actionsNode == null ? this : actionsNode;

            var actions = target.GetChildren<StateAction>();
            foreach (var action in actions)
            {
                AddActionToAList(action);
                action.Init(this);
            }

            ChangeState(_defaultState); 
            
            foreach (var child in this.GetChildren<StateMachine>(true))
            {
                child._isSubStateMachine = true;
                child.ExitMachine();
            }
        }

        public override void _Process(float delta)
        {
            CallActions(_updateActions, delta);
            _currentState.Process(delta);
            var currentTransitionState = CheckTransitions();
            if (currentTransitionState.change == true)
                ChangeState(currentTransitionState.newState);
        }

        public override void _PhysicsProcess(float delta)
        {
            CallActions(_fixedUpdateActions, delta);
            _currentState.PhysicsProcess(delta);
        }

        private void ExitMachine()
        {
            CallExitActions();
            _currentState?.Exit();
            this.Disable();
        }

        private void EnterMachine()
        {
            this.Enable();
            CallEnterActions();
            ChangeState(_defaultState);
        }

        public T GetNodeOfType<T>() where T:Node
        {
            if(_nodes.TryGetValue(typeof(T), out Node value))
            {
                return (T)value;
            }

            if(this.TryGetNodeInMeAndParent<T> (out T res, recursive: true) == false)
            {
                GD.PrintErr($"No node of type {typeof(T)} was found!");
                return null;
            }
            _nodes.Add(typeof(T), res);
            return res;
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

        private void ChangeState(State newState)
        {
            if (newState.StateMachine != this && _isSubStateMachine == true)
            {
                return;
            }

            _currentSubStateMachine?.ExitMachine();
            _currentState?.Exit();

            if (newState.StateMachine != this)
                _currentSubStateMachine = newState.StateMachine;
            else
                _currentSubStateMachine = null;
            _currentState = newState;
            _currentStateTransitions = _currentState.Transitions;

            _currentState.Enter();
            _currentSubStateMachine?.EnterMachine();
            EmitSignal(nameof(StateChanged), newState);
        }

        private void AddActionToAList(StateAction action)
        {
            if (action.OnEnter == true)
            {
                _enterActions.Add(action);
            }
            if (action.OnExit == true)
            {
                _exitActions.Add(action);
            }
            if (action.OnFixedUpdate == true)
            {
                _fixedUpdateActions.Add(action);
            }
            if (action.OnUpdate == true)
            {
                _updateActions.Add(action);
            }
        }

        private void CallActions(in List<StateAction> actionList, float delta = -1)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                StateAction item = actionList[i];
                item.Act(delta);
            }
        }

        private void CallEnterActions()
        {
            for (int i = 0; i < _enterActions.Count; i++)
            {
                StateAction item = _enterActions[i];
                item.OnStateEnter();
                item.Act(-1);
            }
        }

        private void CallExitActions()
        {
            for (int i = 0; i < _exitActions.Count; i++)
            {
                StateAction item = _exitActions[i];
                GD.Print(this.Name);
                item.OnStateExit();
                item.Act(-1);
            }
        }
    }
}