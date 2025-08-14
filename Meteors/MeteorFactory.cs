using System;
using Godot;

public static class MeteorFactory
{
    public const string GroupName = "meteors";

    private static readonly PackedScene meteorScene1 = GD.Load<PackedScene>("res://Meteors/M1/Meteor_1.tscn");
    private static readonly PackedScene meteorScene2 = GD.Load<PackedScene>("res://Meteors/M2/Meteor_2.tscn");
    private static readonly RandomNumberGenerator RNG = new RandomNumberGenerator();

    public static T CreateMeteor<T>(Vector2 position, Vector2 direction) where T : BaseMeteor
    {
        T m = typeof(T) switch
        {
            var v1 when v1 == typeof(Meteor1) => meteorScene1.Instantiate<T>(),
            var v2 when v2 == typeof(Meteor2) => meteorScene2.Instantiate<T>(),
            _ => null
        };

        if (m is null) return null;

        var rsize = RNG.RandfRange(0.1f, 0.3f);
        var rmass = RNG.RandfRange(5000f, 7000f) * rsize;
        m.Initialize(position, direction, rmass, new Vector2(rsize, rsize));
        m.Resize(new Vector2(rsize, rsize));
        m.AddToGroup(GroupName);

        return m;
    }
}