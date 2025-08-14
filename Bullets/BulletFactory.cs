using System;
using Godot;

public static class BulletFactory
{
    public const string GroupName = "bullets";
    private static double lastFireTime = 0.0;

    // Bullet scenes
    private static PackedScene bulletSceneV1 = GD.Load<PackedScene>("res://Bullets/v1/BulletV1.tscn");
    private static PackedScene bulletSceneV2 = GD.Load<PackedScene>("res://Bullets/v2/BulletV2.tscn");

    public static T CreateBullet<T>(Vector2 position, Vector2 direction) where T : BaseBullet
    {
        T bullet = typeof(T) switch
        {
            var v1 when v1 == typeof(BulletV1) && AllowFire(BulletV1.FireRate) => bulletSceneV1.Instantiate<T>(),
            var v2 when v2 == typeof(BulletV2) && AllowFire(BulletV2.FireRate) => bulletSceneV2.Instantiate<T>(),
            _ => null
        };

        bullet?.Initialize(position, direction);
        bullet?.AddToGroup(GroupName);
        return bullet;
    }

    public static bool AllowFire(float fireRate)
    {
        // Use Godot's high-performance time instead of DateTime
        var currentTime = Time.GetUnixTimeFromSystem();
        var fireInterval = 60.0 / fireRate; // Cache this if fireRate doesn't change

        var timePassed = currentTime - lastFireTime;

        if (timePassed < fireInterval) return false;

        lastFireTime = currentTime;
        return true;
    }
}