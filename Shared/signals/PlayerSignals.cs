using Godot;

public partial class PlayerSignals : Node
{
    [Signal] public delegate void DamagePlayerEventHandler(float damage);

    [Signal] public delegate void PlayerHeathUpdateEventHandler(float heath);
}