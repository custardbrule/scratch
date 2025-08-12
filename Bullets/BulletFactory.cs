using System;
using Godot;

public static class BulletFactory
{
    public const string GroupName = "bullets";
    private static double lastFireTime = 0.0;

    // Bullet scenes
    private static PackedScene bulletSceneV1 = GD.Load<PackedScene>("res://Bullets/v1/BulletV1.tscn");

    public static T CreateBullet<T>(Vector2 position, Vector2 direction) where T : BaseBullet
    {
        T bullet = typeof(T) switch
        {
            var v1 when v1 == typeof(BulletV1) && AllowFire(BulletV1.FireRate) => bulletSceneV1.Instantiate<T>(),
            _ => null
        };

        bullet?.Initialize(position, direction);
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