using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsContext : MonoBehaviour
{
    [SerializeField] private BasePlayerStats baseStats;

    private readonly List<IPlayerStatsDecorator> decorators = new List<IPlayerStatsDecorator>();

    public IPlayerStats CurrentStats =>
        decorators.Count > 0 ? decorators[decorators.Count - 1] : baseStats;

    private void Awake()
    {
        if (!baseStats)
        {
            baseStats = GetComponent<BasePlayerStats>();
        }

        if (!baseStats)
        {
            baseStats = gameObject.AddComponent<BasePlayerStats>();
        }
    }

    public void AddDecorator(IPlayerStatsDecorator decorator)
    {
        if (decorator == null || decorators.Contains(decorator))
        {
            return;
        }

        decorator.SetInner(CurrentStats);
        decorators.Add(decorator);
    }

    public void RemoveDecorator(IPlayerStatsDecorator decorator)
    {
        if (decorator == null)
        {
            return;
        }

        if (!decorators.Remove(decorator))
        {
            return;
        }

        RebuildChain();
    }

    private void RebuildChain()
    {
        IPlayerStats current = baseStats;

        for (int i = 0; i < decorators.Count; i++)
        {
            IPlayerStatsDecorator decorator = decorators[i];
            decorator.SetInner(current);
            current = decorator;
        }
    }
}
