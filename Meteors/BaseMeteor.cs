using System;
using Godot;

public abstract partial class BaseMeteor : RigidBody2D
{
    [Export] public virtual float RotateSpeed { get; set; } = 500.0f;
    [Export] public virtual float Speed { get; set; } = 500.0f;
    [Export] public virtual int Damage { get; set; } = 10;

    public virtual void Initialize(Vector2 startPos, Vector2 targetDirection)
    {
        GravityScale = 0.0f;
        GlobalPosition = startPos;
        LinearVelocity = targetDirection * Speed;
    }

    public abstract void Resie(Vector2 scale);
}