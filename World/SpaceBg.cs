using Godot;
using System;

public partial class SpaceBg : ParallaxLayer
{
	private float time = 0.0f;
	private Vector2 velocity = Vector2.Zero;
	[Export] public float Friction { get; set; } = 0.95f;
	[Export] public float Force { get; set; } = 200.0f;

	public override void _Process(double delta)
	{
		var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		// Apply auto drift
		if (input == Vector2.Zero)
		{
			time += (float)delta;
			input = new Vector2(
					Mathf.Sin(time * 0.5f) * 2,      // Slow horizontal drift
					Mathf.Cos(time * 0.4f) * 0.5f        // Slow vertical drift
				);
		}

		// Apply force
		velocity += input * Force * (float)delta;

		// Apply friction
		velocity *= Friction;

		// Update position
		MotionOffset += velocity * (float)delta;
	}
}
