using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatGroup
{
    public enum Layer
    {
        FirstValue,
        Stat,
        Equipment,
    }

    Dictionary<Layer, PlayerStat> playerStatLayers = new Dictionary<Layer, PlayerStat>();

    public PlayerStat GetPlayerStat(Layer layer)
    {
        if (playerStatLayers.TryGetValue(layer, out var playerStat) == false)
        {
            return null;
        }

        return playerStat;
    }

    public void SetPlayerStat(Layer layer, PlayerStat playerStat)
    {
        playerStatLayers[layer] = playerStat;
    }


    public void Init()
    {
        foreach (var kv in playerStatLayers)
        {
            var ps = kv.Value;

            ps.Init();
            ps.UpdateAllStat();
        }
    }


    public float GetStat(Status.Stat stat)
    {
        return playerStatLayers.Values.Sum(l => l.GetStat(stat));
    }

}
