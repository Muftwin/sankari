﻿namespace Sankari;

public class PlayerAnimationWalking : EntityAnimation<Player>
{
	public PlayerAnimationWalking(Player player) : base(player) { }

	public override void Enter()
	{
		Entity.AnimatedSprite.Play("walk");
	}

	public override void Update()
	{
		FlipSpriteOnDirection();
	}

	public override void HandleTransitions()
	{
		// Walking -> Idle
		// Walking -> Running
		// Walking -> Dash
		// Walking -> JumpStart

		if (Entity.PlayerInput.IsJump)
			SwitchState(EntityAnimationType.JumpStart);

		else if (Entity.PlayerInput.IsDash && Entity.GetCommandClass<PlayerCommandDash>(PlayerCommandType.Dash).DashReady)
			SwitchState(EntityAnimationType.Dash);

		else if (Entity.PlayerInput.IsSprint)
			SwitchState(EntityAnimationType.Running);

		else if (Entity.MoveDir == Vector2.Zero || Entity.Velocity.y != 0)
			SwitchState(EntityAnimationType.Idle);
	}
}
