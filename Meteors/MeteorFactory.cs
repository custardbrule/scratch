using System;
using System.Linq;
using Godot;

public static class MeteorFactory
{
    public const string GroupName = "meteors";

    private static PackedScene meteorScene1 = GD.Load<PackedScene>("res://Meteors/M1/Meteor_1.tscn");
    private static RandomNumberGenerator RNG = new RandomNumberGenerator();

    public static T CreateMeteor<T>(Vector2 position, Vector2 direction) where T : BaseMeteor
    {
        T m = typeof(T) switch
        {
            var v1 when v1 == typeof(Meteor1) => meteorScene1.Instantiate<T>(),
            _ => null
        };

        if (m is null) return null;

        var rsize = RNG.RandfRange(0.2f, 0.45f);
        var rmass = RNG.RandfRange(500f, 1000f);
        m.Initialize(position, direction, rmass, new Vector2(rsize, rsize));
        m.Resize(new Vector2(rsize, rsize));

        m.AddToGroup(GroupName);

        return m;
    }
}