using System;
using Godot;

public partial class Meteor2 : BaseMeteor
{
    [Export] public float MaxDistanceFromCamera = 1500f;
    public override float HeathPoint
    {
        get => base.HeathPoint; set
        {
            if (value <= 0) HandleDestroyed();
            base.HeathPoint = value;
        }
    }


    private Camera2D camera
    {
        get
        {
            return GetViewport().GetCamera2D();
        }
    }
    public override float Speed { get; set; } = 100.0f;
    private AnimatedSprite2D animatedSprite2D
    {
        get
        {
            return GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        }
    }
    private CollisionPolygon2D collisionPolygon2D
    {
        get
        {
            return GetNode<CollisionPolygon2D>("CollisionPolygon2D");
        }
    }

    public override void _Ready()
    {
        base._Ready();
        animatedSprite2D.Play("default");
    }


    public override void _PhysicsProcess(double delta)
    {
        base._Process(delta);

        var collisionInfo = MoveAndCollide(LinearVelocity * (float)delta);
        if (collisionInfo is not null)
        {
            LinearVelocity = LinearVelocity.Bounce(collisionInfo.GetNormal());
        }

        Rotation = Mathf.LerpAngle(Rotation, Rotation + RotateSpeed, (float)delta);

        float distanceFromCamera = GlobalPosition.DistanceTo(camera.GlobalPosition);
        if (distanceFromCamera > MaxDistanceFromCamera) QueueFree();
    }
    public override void Resize(Vector2 scale)
    {
        animatedSprite2D.Scale = scale;
        collisionPolygon2D.Scale = scale;
    }

    private void HandleDestroyed()
    {
        animatedSprite2D.Play("destroyed");
        collisionPolygon2D.Disabled = true;
        animatedSprite2D.AnimationFinished += QueueFree;
    }

}
