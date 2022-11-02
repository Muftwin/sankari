﻿namespace Sankari;

public interface IEntityMovement : IEntityMoveable, IEntityDash
{
	// Ground acceleration
	public int SpeedGround { get; set; }

}

public class EntityCommandMovement : EntityCommand<IEntityMovement>
{
	#region Configuration
	public int MaxSpeedWalk     { get; set; } = 350;
	public int MaxSpeedSprint   { get; set; } = 500;
	public int MaxSpeedAir      { get; set; } = 350;
	public int AirAcceleration  { get; set; } = 30;
	public int DampeningAir     { get; set; } = 10;
	public int DampeningGround  { get; set; } = 25;
	#endregion
	public EntityCommandMovement(IEntityMovement entity) : base(entity) { }

	public override void Initialize()
	{
		// if these are equal to each other then the player movement will not work as expected
		if (Entity.SpeedGround == DampeningGround)
			DampeningGround -= 1;
	}

	public override void UpdateGroundWalking(float delta)
	{
		var velocity = Entity.Velocity;
		velocity.x = ClampAndDampen(velocity.x, DampeningGround, MaxSpeedWalk);
		Entity.Velocity = velocity;
	}

	public override void UpdateGroundSprinting(float delta)
	{
		var velocity = Entity.Velocity;
		velocity.x = ClampAndDampen(velocity.x, DampeningGround, MaxSpeedSprint);
		Entity.Velocity = velocity;
	}

	public override void UpdateAir(float delta)
	{
		var velocity = Entity.Velocity;
		velocity.x += Entity.MoveDir.x * AirAcceleration;
		velocity.x = ClampAndDampen(velocity.x, DampeningAir, MaxSpeedAir);
		Entity.Velocity = velocity;
	}

	private float ClampAndDampen(float horzVelocity, int dampening, int maxSpeedGround) 
	{
		if (Mathf.Abs(horzVelocity) <= dampening)
			return 0;
		else if (horzVelocity > 0)
			return Mathf.Min(horzVelocity - dampening, maxSpeedGround);
		else
			return Mathf.Max(horzVelocity + dampening, -maxSpeedGround);
	}
}
