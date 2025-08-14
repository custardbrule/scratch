using Godot;
using System;

public partial class BulletV1 : BaseBullet
{
	public new static float FireRate { get; set; } = 500.0f;
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
		if (collisionInfo is not null)
		{
			var collider = (Node)collisionInfo.GetCollider();
			collider.QueueFree();
			LinearVelocity = LinearVelocity.Bounce(collisionInfo.GetNormal());
		}
	}

	public override void HitTarget(Node target)
	{
		throw new NotImplementedException();
	}

}
