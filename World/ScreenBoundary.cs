using System;
using Godot;

public partial class ScreenBoundary : Area2D
{
	public override void _Ready()
	{
		// Connect signal

		BodyExited += OnBodyExited;
	}

	private void OnBodyExited(Node2D body)
	{
		switch (body)
		{
			case BaseBullet:
			case BaseMeteor:
				body.QueueFree();
				break;
			default: break;
		}
	}
}
