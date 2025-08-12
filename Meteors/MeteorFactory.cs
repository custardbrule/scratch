using System;
using System.Linq;
using Godot;

public static class MeteorFactory
{
    public const string GroupName = "meteors";

    private static PackedScene meteorScene1 = GD.Load<PackedScene>("res://Meteors/M1/Meteor_1.tscn");

    public static T CreateMeteor<T>(Vector2 position, Vector2 direction) where T : BaseMeteor
    {
        T m = typeof(T) switch
        {
            var v1 when v1 == typeof(Meteor1) => meteorScene1.Instantiate<T>(),
            _ => null
        };

        m.Initialize(position, direction);
        m.Resie(new Vector2(0.2f, 0.2f));

        m.AddToGroup(GroupName);

        return m;
    }
}