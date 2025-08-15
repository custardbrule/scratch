using Godot;
using System;

public partial class EnemyV1 : CharacterBody2D
{
	[Export] public float ChaseSpeed = 300.0f;
	[Export] public float AvoidanceForce = 150.0f;
	[Export] public float MaxSpeed = 400.0f;
	
	private Area2D DetectionLayer { get => GetNode<Area2D>("Area2D"); }
	private Player player { get; set; }
	private Vector2 avoidanceVelocity = Vector2.Zero;

	public override void _Ready()
	{
		base._Ready();
		player = GetTree().CurrentScene.GetNode<Player>("Player");
		DetectionLayer.BodyEntered += CalculateObjectEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = ChasePlayer((float)delta);
		
		// Apply avoidance (decays over time)
		avoidanceVelocity = avoidanceVelocity.MoveToward(Vector2.Zero, 500.0f * (float)delta);
		
		// Combine chase and avoidance
		var finalVelocity = velocity + avoidanceVelocity;
		
		// Clamp to max speed
		if (finalVelocity.Length() > MaxSpeed)
		{
			finalVelocity = finalVelocity.Normalized() * MaxSpeed;
		}
		
		Velocity = finalVelocity;
		MoveAndSlide();
	}

	private Vector2 ChasePlayer(float delta)
	{
		LookAt(player.GlobalPosition);
		var direction = (player.GlobalPosition - GlobalPosition).Normalized();
		return Velocity.MoveToward(direction * ChaseSpeed, 1000.0f * delta);
	}

	private void CalculateObjectEntered(Node2D node)
	{
		switch (node)
		{
			case BaseMeteor meteor:
				var direction = (GlobalPosition - meteor.GlobalPosition).Normalized();
				avoidanceVelocity += direction * AvoidanceForce;
				break;
			default: 
				break;
		}
	}
}
