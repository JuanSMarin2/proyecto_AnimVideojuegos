using UnityEngine;

public interface IPlayerStats
{
    float MoveSpeedMultiplier { get; }
    float DamageMultiplier { get; }
    bool IsInvulnerable { get; }
}

public interface IPlayerStatsDecorator : IPlayerStats
{
    IPlayerStats Inner { get; }
    void SetInner(IPlayerStats inner);
}
