﻿using Godot;
using MP.Extensions;
using MP.AnimatorWrappers;

namespace MP.StateMachine.Actions
{
    public class PlayAnimation : StateAction
    {
        [Export] private string _name;
        [Export] private float _transitionDuration = .25f;
        [Export] private float _playbackSpeed = 1;
        [Export] private NodePath _pathToAnimatedModel;

        private Animator _animatedModel;

        public override void Init(StateMachine stateMachine)
        {
            this.TryGetNodeFromPath<Animator>(_pathToAnimatedModel, out _animatedModel);
        }

        public override void Act(float delta)
        {
            _animatedModel.PlayAnimation(_name, _transitionDuration, _playbackSpeed);
        }
    }
}