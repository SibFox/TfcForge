using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ForgeDatabase : Node
{
	public enum Action
	{
		None = 0,
		Draw = -15, //Протянуть
		StrongHit = -9,
		MediumHit = -6,
		WeakHit = -3,
		Punch = 2,
		Bend = 7,
		Upset = 13, //Обжать
		Shrink = 16 //Усадить
	}

	public enum ShownAction
	{
		None = -1,
		Draw, //Протянуть
		Hit,
		Punch,
		Bend,
		Upset, //Обжать
		Shrink //Усадить
	}

	public static Dictionary<Action, string> ActionTRCodes = new()
	{
		{ Action.None, "" },
		{ Action.Draw, "action.draw" },
		{ Action.StrongHit, "action.stronghit" },
		{ Action.MediumHit, "action.mediumhit" },
		{ Action.WeakHit, "action.weakhit" },
		{ Action.Punch, "action.punch" },
		{ Action.Bend, "action.bend" },
		{ Action.Upset, "action.upset" },
		{ Action.Shrink, "action.shrink" }
	};
}
