using System;
using Godot;

public partial class BulletV2 : BaseBullet
{
	public new static float FireRate { get; set; } = 200.0f;
	public override float Speed { get; set; } = 500.0f;
	public override int Damage { get; set; } = 200;

	public override void _Ready()
	{
		base._Ready();
		Vector2 mousePosition = GetGlobalMousePosition();
		LookAt(mousePosition);
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
			case BaseMeteor meteor:
				meteor.ApplyForce(impulse);
				meteor.HeathPoint -= Damage;
				break;
			default: break;
		}

		// remove bullet after hit
		QueueFree();
	}
}
