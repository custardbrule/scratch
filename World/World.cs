using System;
using Godot;

public partial class World : Node2D
{
	private readonly RandomNumberGenerator rng = new RandomNumberGenerator();
	private readonly Timer meteorSpawnTimer = new Timer();
	private float meteorSpawnCooldown { get; set; } = 5.0f;

	private Player player { get => GetNode<Player>("Player"); }
	private Camera2D camera { get => player.GetNode<Camera2D>("Camera2D"); }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetupMouseCursor();
		meteorSpawnTimer.WaitTime = meteorSpawnCooldown;
		meteorSpawnTimer.OneShot = false;
		meteorSpawnTimer.Timeout += CreateMeteor;
		meteorSpawnTimer.Autostart = true;
		AddChild(meteorSpawnTimer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
	}

	private void SetupMouseCursor()
	{
		var loadImg = ResourceLoader.Load<CompressedTexture2D>("res://World/Assets/Crosshair/crosshair025.png");

		var img = loadImg.GetImage();
		img.Resize(32, 32);

		// Calculate center hotspot
		var hotspot = new Vector2(img.GetWidth() / 2, img.GetHeight() / 2);

		Input.SetCustomMouseCursor(img, Input.CursorShape.Arrow, hotspot);
	}

	private void CreateMeteor()
	{
		if (GetTree().GetNodesInGroup(MeteorFactory.GroupName).Count < 5)
		{
			var position = GetRandomPointOutsideCamera();
			var direction = player.GlobalPosition - position;
			var noiseDirection = direction.Rotated(rng.RandfRange(-1f, 1f));
			var t = rng.RandiRange(1, 8);
			BaseMeteor meteor = t switch
			{
				1 => MeteorFactory.CreateMeteor<Meteor1>(position, noiseDirection),
				2 => MeteorFactory.CreateMeteor<Meteor2>(position, noiseDirection),
				_ => MeteorFactory.CreateMeteor<Meteor2>(position, noiseDirection),
			};
			AddChild(meteor);
		}
	}

	private Vector2 GetRandomPointOutsideCamera()
	{
		var camera = GetViewport().GetCamera2D();
		if (camera == null) return Vector2.Zero;

		var viewport = GetViewport();
		var screenSize = viewport.GetVisibleRect().Size;
		var zoom = camera.Zoom;
		var visibleSize = screenSize / zoom;
		var cameraPos = camera.GlobalPosition;

		// Define margin outside camera view
		float margin = 200f;

		// Random side: 0=top, 1=right, 2=bottom, 3=left
		int side = rng.RandiRange(0, 3);

		Vector2 randomPoint;

		switch (side)
		{
			case 0: // Top
				randomPoint = new Vector2(
					rng.RandfRange(cameraPos.X - visibleSize.X / 2 - margin, cameraPos.X + visibleSize.X / 2 + margin),
					cameraPos.Y - visibleSize.Y / 2 - rng.RandfRange(margin, margin * 3)
				);
				break;
			case 1: // Right
				randomPoint = new Vector2(
					cameraPos.X + visibleSize.X / 2 + rng.RandfRange(margin, margin * 3),
					rng.RandfRange(cameraPos.Y - visibleSize.Y / 2 - margin, cameraPos.Y + visibleSize.Y / 2 + margin)
				);
				break;
			case 2: // Bottom
				randomPoint = new Vector2(
					rng.RandfRange(cameraPos.X - visibleSize.X / 2 - margin, cameraPos.X + visibleSize.X / 2 + margin),
					cameraPos.Y + visibleSize.Y / 2 + rng.RandfRange(margin, margin * 3)
				);
				break;
			default: // Left
				randomPoint = new Vector2(
					cameraPos.X - visibleSize.X / 2 - rng.RandfRange(margin, margin * 3),
					rng.RandfRange(cameraPos.Y - visibleSize.Y / 2 - margin, cameraPos.Y + visibleSize.Y / 2 + margin)
				);
				break;
		}

		return randomPoint;
	}
}
