using System;
using Godot;

public abstract partial class BaseMeteor : RigidBody2D
{
    [Export] public virtual float RotateSpeed { get; set; } = 500.0f;
    [Export] public virtual float Speed { get; set; } = 500.0f;
    [Export] public virtual int Damage { get; set; } = 10;

    public virtual void Initialize(Vector2 startPos, Vector2 targetDirection, float mass, Vector2 scale)
    {
        // let rng make some noise
        var rng = new RandomNumberGenerator();
        var noise = rng.RandfRange(-0.1f, 0.1f);
        GravityScale = 0.0f;
        Scale = scale.Normalized() - scale.Normalized() * noise;
        Mass = mass - mass * noise;
        Speed = Speed - Speed * noise;
        RotateSpeed = RotateSpeed - RotateSpeed * noise;
        GlobalPosition = startPos;
        LinearVelocity = targetDirection * Speed;
    }

    public abstract void Resize(Vector2 scale);
}