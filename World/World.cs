using Godot;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public partial class World : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var loadImg = ResourceLoader.Load<CompressedTexture2D>("res://World/Assets/Crosshair/crosshair025.png");

		var img = loadImg.GetImage();
		img.Resize(32, 32);
	
		// Calculate center hotspot
		var hotspot = new Vector2(img.GetWidth() / 2, img.GetHeight() / 2);
		
		Input.SetCustomMouseCursor(img, Input.CursorShape.Arrow, hotspot);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GetTree().GetNodesInGroup(MeteorFactory.GroupName).Count() < 1)
		{
			var meteor = MeteorFactory.CreateMeteor(GetGlobalMousePosition(), new Vector2(1, 0));
			AddChild(meteor);
		}
	}
}
