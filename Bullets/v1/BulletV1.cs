using System;
using Godot;

public partial class BulletV1 : BaseBullet
{
	public new static float FireRate { get; set; } = 500.0f;
	public override int Damage { get; set; } = 100;
	public override float Speed { get; set; } = 800.0f;

	public override void _Ready()
	{
		base._Ready();
		Vector2 mousePosition = GetGlobalMousePosition();
		LookAt(mousePosition);
		Rotate(MathF.PI / 2);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		var collisionInfo = MoveAndCollide(LinearVelocity * (float)delta);
		if (collisionInfo is not null) HitTarget(collisionInfo, (float)delta);
	}

	private void HitTarget(KinematicCollision2D collisionInfo, float delta)
	{
		var target = collisionInfo.GetCollider();
		var remain = collisionInfo.GetRemainder();

		// Calculate the impulse (change in momentum)
		var impulse = Mass * remain / delta;

		switch (target)
		{
			case IHeathPoint heathPoint:
				if (target.HasMethod("apply_force")) target.Call("apply_force", impulse);
				heathPoint.OnHeathChange(Damage);
				break;
			default: break;
		}

		QueueFree();
	}
}
