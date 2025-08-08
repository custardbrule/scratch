using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float MaxSpeed = 500.0f;
	public const float AccelerationRate = 500.0f;
	public const float DecelerationRate = 1000.0f;

	private float _curentSpeed = 0.0f;
	private Vector2 _lastDirection = Vector2.Zero;

	// flags
	public bool IsShooting { get; set; }

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			LookAt(GetGlobalMousePosition());
		}

		if (@event is InputEventMouseButton)
		{
			if (Input.IsMouseButtonPressed(MouseButton.Left)) IsShooting = true;
			else IsShooting = false;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		ShootingHandler(delta);
		MoveHandler(delta);
		MoveAndSlide();
	}

	private void ShootingHandler(double delta)
	{
		if (!IsShooting) return;

		var direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
		var spawnPosition = GlobalPosition + direction * 20.0f; // Spawn away from ship

		var bullet = BulletFactory.CreateBullet<BulletV1>(spawnPosition, direction);
		// Add bullet to the scene tree
		if (bullet is not null) GetTree().CurrentScene.AddChild(bullet);
	}

	private void MoveHandler(double delta)
	{
		Vector2 velocity = Velocity;

		var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			if (_lastDirection.Dot(direction) < 0.8)
				_curentSpeed = Speed;
			else
				_curentSpeed = MathF.Min(_curentSpeed + AccelerationRate * (float)delta, MaxSpeed);

			_lastDirection = direction;
			velocity = velocity.MoveToward(_lastDirection * _curentSpeed, 2 * AccelerationRate * (float)delta);
		}
		else
		{
			if (_curentSpeed > 0.0f)
			{
				_curentSpeed = MathF.Max(_curentSpeed - DecelerationRate * (float)delta, 0.0f);
				velocity = velocity.MoveToward(_lastDirection * _curentSpeed, DecelerationRate * (float)delta);
			}
			else
			{
				_curentSpeed = Speed;
				_lastDirection = Vector2.Zero;
				velocity = velocity.MoveToward(_lastDirection, _curentSpeed);
			}
		}

		Velocity = velocity;
	}
}
