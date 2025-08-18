using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnemyV1 : CharacterBody2D, IHeathPoint
{
	public const float Speed = 200.0f;
	public const float MaxSpeed = 500.0f;
	public const float AccelerationRate = 700.0f;
	public const float DecelerationRate = 1200.0f;
	public const float AvoidanceStrength = 300.0f;
	public const float PlayerWeight = 1.0f;
	public const float AvoidanceWeight = 2.0f;

	public float HeathPoint { get; set; } = 3000.0f;

	private float _curentSpeed = 0.0f;
	private Vector2 _lastDirection = Vector2.Zero;

	private Area2D _meteorDetectionLayer { get => GetNode<Area2D>("MeteorDetection"); }
	private Player _player { get; set; }
	private List<BaseMeteor> _meteors = [];

	public override void _Ready()
	{
		base._Ready();
		_player = GetTree().CurrentScene.GetNode<Player>("Player");
		_meteorDetectionLayer.BodyEntered += MeteorEntered;
		_meteorDetectionLayer.BodyExited += MeteorExited;
		_curentSpeed = Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player == null) return;

		LookAt(_player.GlobalPosition);

		// Calculate direction forces (normalized vectors)
		var seekDirection = CalculateSeekDirection();
		var avoidanceDirection = CalculateMeteorAvoidance();
		
		// Combine the forces
		var combinedDirection = (seekDirection * PlayerWeight + avoidanceDirection * AvoidanceWeight);
		
		// Check if we have conflicting forces
		var dot = seekDirection.Dot(avoidanceDirection);
		bool forcesConflict = dot < -0.5f; // Forces pointing in opposite directions
		bool hasStrongAvoidance = avoidanceDirection.Length() > 0.5f;
		
		var velocity = Velocity;

		if (!forcesConflict || hasStrongAvoidance)
		{
			// Safe to move - either no conflict or need to dodge
			var desiredDirection = combinedDirection.Normalized();
			
			if (_lastDirection.Dot(desiredDirection) < 0.8f)
			{
				// Sharp direction change - reset speed
				_curentSpeed = Speed;
			}
			else
			{
				// Gradual acceleration
				_curentSpeed = MathF.Min(_curentSpeed + AccelerationRate * (float)delta, MaxSpeed);
			}

			_lastDirection = desiredDirection;
			var targetVelocity = _lastDirection * _curentSpeed;
			velocity = velocity.MoveToward(targetVelocity, AccelerationRate * (float)delta);
		}
		else
		{
			// Forces conflict strongly - decelerate
			if (_curentSpeed > 0.0f)
			{
				_curentSpeed = MathF.Max(_curentSpeed - DecelerationRate * (float)delta, 0.0f);
				var targetVelocity = _lastDirection * _curentSpeed;
				velocity = velocity.MoveToward(targetVelocity, DecelerationRate * (float)delta);
			}
			else
			{
				// Stopped - prepare for new movement
				_lastDirection = Vector2.Zero;
				velocity = velocity.MoveToward(Vector2.Zero, DecelerationRate * (float)delta);
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private Vector2 CalculateSeekDirection()
	{
		var toPlayer = _player.GlobalPosition - GlobalPosition;
		return toPlayer.Normalized();
	}

	private Vector2 CalculateMeteorAvoidance()
	{
		if (_meteors.Count == 0) 
			return Vector2.Zero;

		var avoidanceForce = Vector2.Zero;

		foreach (var meteor in _meteors)
		{
			if (!IsInstanceValid(meteor)) continue;
			
			var toMeteor = meteor.GlobalPosition - GlobalPosition;
			var distance = toMeteor.Length();
			
			if (distance > 0 && distance < 150.0f) // Avoidance range
			{
				// Basic avoidance - move away from meteor
				var avoidDirection = -toMeteor.Normalized();
				
				// Stronger avoidance for closer meteors
				var strength = (150.0f - distance) / 150.0f;
				
				// Predict meteor movement
				var meteorVelocity = meteor.LinearVelocity;
				if (meteorVelocity.Length() > 0)
				{
					// Calculate where meteor will be
					var timeToImpact = distance / (_curentSpeed + 0.1f);
					var predictedPosition = meteor.GlobalPosition + meteorVelocity * timeToImpact;
					var toPredicted = predictedPosition - GlobalPosition;
					
					// If predicted collision, strengthen avoidance
					if (toPredicted.Length() < distance)
					{
						avoidDirection = -toPredicted.Normalized();
						strength *= 1.5f; // Boost avoidance for predicted collisions
					}
				}
				
				avoidanceForce += avoidDirection * strength * AvoidanceStrength;
			}
		}

		// Normalize the combined avoidance force
		return avoidanceForce.Length() > 0 ? avoidanceForce.Normalized() : Vector2.Zero;
	}

	private void MeteorEntered(Node2D node)
	{
		if (node is BaseMeteor meteor && !_meteors.Contains(meteor))
		{
			_meteors.Add(meteor);
		}
	}

	private void MeteorExited(Node2D node)
	{
		if (node is BaseMeteor meteor)
		{
			_meteors.Remove(meteor);
		}
	}
}