using Godot;
using MP.FiniteStateMachine;
using MP.InputWrapper;
using System;

namespace MP.Player
{

    public class Movement2D : StateAction
    {
        [Export(PropertyHint.Range, "0.1, 1")] private float _slowDown;
        [Export(PropertyHint.Range, "0.1, 1")] private float _speedUp;
        [Export] private float _speed;

        private float _currentSpeed;
        private Player2D _player;
        private PlayerInput2D _playerInput;

        public override void Init(StateMachine stateMachine)
        {
            _player = stateMachine.GetNodeOfType<Player2D>();
            _playerInput = stateMachine.GetNodeOfType<PlayerInput2D>();
        }

        public override void Act(float delta)
        {
            if (delta == -1)
                return;

            if (_playerInput.MovementInput.x == 0)
            {
                _player.Velocity.x = Mathf.Lerp(_player.Velocity.x, 0, _slowDown);
                return;
            }

            _currentSpeed = Mathf.Lerp(_currentSpeed, _speed, _speedUp);
            var motion = _playerInput.MovementInput.x;
            motion *= _currentSpeed / delta;

            _player.Velocity.x = motion;
        }
    }
}