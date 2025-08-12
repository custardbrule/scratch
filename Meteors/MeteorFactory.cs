using System;
using Godot;

public partial class Meteor: RigidBody2D {}

public static class MeteorFactory
{
    public const string GroupName = "meteors";
    public static Meteor CreateMeteor(Vector2 position, Vector2 direction)
    {
        // load img
        var img = ResourceLoader.Load<CompressedTexture2D>("res://Meteors/Assets/Rocks/rock-1.png");

        // create sprite
        var sprite = new Sprite2D();
        sprite.Texture = img;
        sprite.Scale = new Vector2(0.2f, 0.2f);

        // create RigidBody2D
        var body = new Meteor
        {
            GravityScale = 0.0f,
            Position = position,
            LinearVelocity = direction * 100.0f  // Set velocity instead
        };
        body.AddChild(sprite);
        body.AddToGroup(GroupName);


        body.SetCollisionLayerValue(7, true);
        body.SetCollisionLayerValue(8, true);

        body.SetCollisionMaskValue(7, true);
        body.SetCollisionMaskValue(8, true);

        return body;
    }
}