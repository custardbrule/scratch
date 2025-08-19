using System;
using Godot;

public partial class Player : CharacterBody2D, IHeathPoint
{
	public const float Speed = 200.0f;
	public const float MaxSpeed = 500.0f;
	public const float AccelerationRate = 500.0f;
	public const float DecelerationRate = 1000.0f;

	private float _curentSpeed = 0.0f;
	private Vector2 _lastDirection = Vector2.Zero;
	private int[] _weapons = [1, 2];
	private int _currentWeapon { get; set; } = 0;

	// flags
	private bool _isShooting { get; set; }

	public float HeathPoint { get; set; } = 100;

	// signals
	private PlayerSignals _playerSignals => GetNode<PlayerSignals>("/root/PlayerSignals");

	public override void _Input(InputEvent @event)
	{
		switch (@event)
		{
			case InputEventKey inputEventKey when inputEventKey.IsActionPressed("change_weapon_left"):
			case InputEventMouseButton inputEventMouseButton when inputEventMouseButton.IsActionPressed("change_weapon_left"):
				if (_currentWeapon == 0) _currentWeapon = _weapons.Length - 1;
				else _currentWeapon -= 1;
				break;
			case InputEventKey inputEventKey when inputEventKey.IsActionPressed("change_weapon_right"):
			case InputEventMouseButton inputEventMouseButton when inputEventMouseButton.IsActionPressed("change_weapon_right"):
				if (_currentWeapon == _weapons.Length - 1) _currentWeapon = 0;
				else _currentWeapon += 1;
				break;
			case InputEventMouseButton:
				if (Input.IsMouseButtonPressed(MouseButton.Left)) _isShooting = true;
				else _isShooting = false;
				break;
			default: break;
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
		if (!_isShooting) return;

		var direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
		var spawnPosition = GlobalPosition + direction * 20.0f; // Spawn away from ship

		BaseBullet bullet = _weapons[_currentWeapon] switch
		{
			1 => BulletFactory.CreateBullet<BulletV1>(spawnPosition, direction),
			2 => BulletFactory.CreateBullet<BulletV2>(spawnPosition, direction),
			_ => null
		};
		// Add bullet to the scene tree

		if (bullet is not null) GetTree().CurrentScene.AddChild(bullet);
	}

	private void MoveHandler(double delta)
	{
		Vector2 velocity = Velocity;

		LookAt(GetGlobalMousePosition());

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

	public void OnHeathChange(float damage)
	{
		HeathPoint -= damage;
		_playerSignals.EmitSignal(nameof(_playerSignals.PlayerHeathUpdate), HeathPoint);
		if (HeathPoint <= 0) GetTree().ChangeSceneToFile("res://Main/Main.tscn");
	}

}
