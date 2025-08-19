using System;
using Godot;

public partial class PlayerHeathBar : TextureProgressBar
{
    private PlayerSignals _playerSignals => GetNode<PlayerSignals>("/root/PlayerSignals");

    public override void _Ready()
    {
        var player = GetTree().CurrentScene.GetNode<Player>("Player");
        MaxValue = player.HeathPoint;
        MinValue = 0;
        Value = player.HeathPoint;

        _playerSignals.PlayerHeathUpdate += OnPlayerHeathChangeSignal;
    }

    private void OnPlayerHeathChangeSignal(float heath)
    {
        Value = heath;
    }
}
