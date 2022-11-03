﻿namespace Sankari;

public class EntityAnimationJumpStart : EntityAnimation<MovingEntity>
{
	private GTimer TimerDontCheckOnGround;

	public EntityAnimationJumpStart(MovingEntity entity) : base(entity) { }

	public override void EnterState()
	{
		TimerDontCheckOnGround = Entity.Timers.CreateTimer(new Callable(() => { }), 100, false, true);
		Entity.AnimatedSprite.Play("jump_start");
	}

	public override void UpdateState()
	{
		FlipSpriteOnDirection();
	}

	public override void HandleStateTransitions()
	{
		// JumpStart -> Idle
		// JumpStart -> JumpFall
		// JumpStart -> Dash


		if (Entity.IsFalling())
		{
			SwitchState(EntityAnimationType.JumpFall);
		}
		else if 
		(
			Entity is Player player &&
			player.PlayerInput.IsDash && 
			player.GetCommandClass<MovingEntityCommandDash>(EntityCommandType.Dash).DashReady
		)
		{
			SwitchState(EntityAnimationType.Dash);
		}
		else if (Entity.IsNearGround() && Entity.MoveDir == Vector2.Zero && !TimerDontCheckOnGround.IsActive())
		{
			SwitchState(EntityAnimationType.Idle);
		}
	}

	public override void ExitState()
	{
		
	}
}
