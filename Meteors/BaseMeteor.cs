using System;
using Godot;

public abstract partial class BaseMeteor : RigidBody2D
{
    [Export] public virtual float RotateSpeed { get; set; } = 0.5f;
    [Export] public virtual float Speed { get; set; } = 500.0f;
    [Export] public virtual int Damage { get; set; } = 10;
    [Export] public virtual float HeathPoint { get; set; } = 0;

    protected readonly RandomNumberGenerator rng = new RandomNumberGenerator();

    public virtual void Initialize(Vector2 startPos, Vector2 targetDirection, float mass, Vector2 scale)
    {
        // let rng make some noise
        var noise = rng.RandfRange(-0.1f, 0.1f);
        GravityScale = 0.0f;
        Scale = scale.Normalized() - scale.Normalized() * noise;
        Mass = mass - mass * noise;
        Speed -= Speed * noise;
        RotateSpeed -= RotateSpeed * noise;
        GlobalPosition = startPos;
        LinearVelocity = targetDirection.Normalized() * Speed;
        if (HeathPoint == 0) HeathPoint = Mass * 10;
    }

    public abstract void Resize(Vector2 scale);
}