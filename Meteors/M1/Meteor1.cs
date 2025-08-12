using System;
using Godot;

public partial class Meteor1 : BaseMeteor
{
	public override float Speed { get; set; } = 100.0f;
	private Sprite2D sprite2D
	{
		get
		{
			return GetNode<Sprite2D>("Sprite2D");
		}
	}

	private CollisionPolygon2D collisionPolygon2D
	{
		get
		{
			return GetNode<CollisionPolygon2D>("CollisionPolygon2D");
		}
	}

	public override void Resie(Vector2 scale)
	{
		sprite2D.Scale = scale;
		collisionPolygon2D.Scale = scale;
	}

}
