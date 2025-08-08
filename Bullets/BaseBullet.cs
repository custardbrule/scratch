using Godot;
using System;

public abstract partial class BaseBullet : RigidBody2D
{
    [Export] public virtual float Speed { get; set; } = 500.0f;
    [Export] public virtual int Damage { get; set; } = 10;
    [Export] public virtual float Lifetime { get; set; } = 5.0f;
    public static readonly float FireRate = 500.0f; //RPM

    protected Vector2 direction;
    
    public override void _Ready()
    {
        // Start lifetime timer
        GetTree().CreateTimer(Lifetime).Timeout += OnLifetimeExpired;
    }
    
    public virtual void Initialize(Vector2 startPos, Vector2 targetDirection)
    {
        GlobalPosition = startPos;
        direction = targetDirection.Normalized();
        LinearVelocity = direction * Speed;
    }
    
    protected virtual void OnLifetimeExpired()
    {
        QueueFree();
    }

    public abstract void HitTarget(Node target);
}