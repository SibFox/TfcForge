using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ForgeDatabase : Node
{
    public enum Action
    {
        Pull = -15, //Протянуть
        StrongHit = -9,
        MediumHit = -6,
        WeakHit = -3,
        Stamp = 2,
        Bend = 7,
        Crimp = 13, //Обжать
        Shrink = 16 //Усадить
    }

    public enum ShownAction
    {
        Pull, //Протянуть
        Hit,
        Stamp,
        Bend,
        Crimp, //Обжать
        Shrink //Усадить
    }

    public static Dictionary<Action, string> ActionTRCodes = new()
    {
        { Action.Pull, "TR_PULL" },
        { Action.StrongHit, "TR_STRONGHIT" },
        { Action.MediumHit, "TR_MEDIUMHIT" },
        { Action.WeakHit, "TR_WEAKHIT" },
        { Action.Stamp, "TR_STAMP" },
        { Action.Bend, "TR_BEND" },
        { Action.Crimp, "TR_CRIMP" },
        { Action.Shrink, "TR_SHRINK" }
    };
}
