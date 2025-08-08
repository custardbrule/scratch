using Godot;
using System;

public partial class ScreenBoundary : Area2D
{
	public override void _Ready()
	{
		// Connect signal
		BodyExited += OnBodyExited;
	}
	
	private void OnBodyExited(Node2D body)
	{
		if (body is BaseBullet bullet) bullet.QueueFree();
	}
}
