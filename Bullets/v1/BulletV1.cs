using Godot;
using System;

public partial class BulletV1 : BaseBullet
{
	public new static float FireRate { get; set; } = 500.0f;
	public override float Speed { get; set; } = 800.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Vector2 mousePosition = GetGlobalMousePosition();
		LookAt(mousePosition);
		Rotate(MathF.PI / 2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void HitTarget(Node target)
	{
		throw new NotImplementedException();
	}

}
