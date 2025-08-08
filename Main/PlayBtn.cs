using Godot;
using System;

public partial class PlayBtn : Button
{
	public override void _Pressed()
	{
		GetTree().ChangeSceneToFile("res://World/world.tscn");
	}
}
