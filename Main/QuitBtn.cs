using Godot;
using System;

public partial class QuitBtn : Button
{
	public override void _Pressed()
	{
		GetTree().Quit();
	}
}
