using System;
using Godot;

public partial class Meteor1 : BaseMeteor, IHeathPoint
{
	[Export] public float MaxDistanceFromCamera = 1500f;

	public float HeathPoint { get; private set; }

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

	public override void _Ready()
	{
		base._Ready();
		HeathPoint = Mass * 20;
	}


	public override void _PhysicsProcess(double delta)
	{
		base._Process(delta);

		var collisionInfo = MoveAndCollide(LinearVelocity * (float)delta);
		if (collisionInfo is not null) HandleCollision(collisionInfo, (float)delta);

		Rotation = Mathf.LerpAngle(Rotation, Rotation + RotateSpeed, (float)delta);

		if (camera is not null)
		{
			float distanceFromCamera = GlobalPosition.DistanceTo(camera.GlobalPosition);
			if (distanceFromCamera > MaxDistanceFromCamera) QueueFree();
		}
	}

	public override void Resize(Vector2 scale)
	{
		sprite2D.Scale = scale;
		collisionPolygon2D.Scale = scale;
	}

	public void OnHeathChange(float damage)
	{
		HeathPoint -= damage;
		if (HeathPoint <= 0) QueueFree();
	}

	private void HandleCollision(KinematicCollision2D collisionInfo, float delta)
	{
		var target = collisionInfo.GetCollider();
		var remain = collisionInfo.GetRemainder();

		// Calculate bounce
		LinearVelocity = LinearVelocity.Bounce(collisionInfo.GetNormal()) * (float)delta;

		// Calculate the impulse (change in momentum)
		var impulse = Mass * remain / delta;

		switch (target)
		{
			case IHeathPoint heathPoint:
				if (target.HasMethod("apply_impulse")) target.Call("apply_impulse", impulse);
				heathPoint.OnHeathChange(Damage);
				break;
			default: break;
		}
	}
}
