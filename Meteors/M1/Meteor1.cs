using System;
using Godot;

public partial class Meteor1 : BaseMeteor
{
	[Export] public float MaxDistanceFromCamera = 1500f;
	public override float HeathPoint
	{
		get => base.HeathPoint; set
		{
			if (value <= 0) HandleDestroyed();
			base.HeathPoint = value;
		}
	}

	private Camera2D camera
	{
		get
		{
			return GetViewport().GetCamera2D();
		}
	}
	public override float Speed { get; set; } = 100.0f;
	private Sprite2D sprite2D
	{
		get
		{
			return GetNode<Sprite2D>("Sprite2D");
		}
	}

	private CollisionPolygon2D collisionPolygon2D
	{
		get
		{
			return GetNode<CollisionPolygon2D>("CollisionPolygon2D");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._Process(delta);

		var collisionInfo = MoveAndCollide(LinearVelocity * (float)delta);
		if (collisionInfo is not null)
		{
			LinearVelocity = LinearVelocity.Bounce(collisionInfo.GetNormal());
		}

		Rotation = Mathf.LerpAngle(Rotation, Rotation + RotateSpeed, (float)delta);

		float distanceFromCamera = GlobalPosition.DistanceTo(camera.GlobalPosition);
		if (distanceFromCamera > MaxDistanceFromCamera) QueueFree();
	}

	public override void Resize(Vector2 scale)
	{
		sprite2D.Scale = scale;
		collisionPolygon2D.Scale = scale;
	}

	private void HandleDestroyed()
	{
		QueueFree();
	}
}
