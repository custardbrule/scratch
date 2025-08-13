using Godot;
using System;

public partial class BulletBoundary : Area2D
{
	private CollisionShape2D Collision
	{
		get
		{
			return GetNode<CollisionShape2D>("CollisionShape2D");
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyExited += OnBodyExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var camera = GetViewport().GetCamera2D();
		if (camera is not null) Collision.GlobalPosition = camera.GlobalPosition;
	}
	
	private void OnBodyExited(Node2D body)
	{
		switch (body)
		{
			case BaseBullet:
				body.QueueFree();
				break;
			default: break;
		}
	}
}
