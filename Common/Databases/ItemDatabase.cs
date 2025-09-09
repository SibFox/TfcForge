using Godot;
using System;
using System.Collections.Generic;

public partial class ItemDatabase : Node
{
	public enum ItemCategory
	{
		All,
		Metal,
		Component,
		Tool,
		Weapon,
		Equipment,
		Decor,
		Misc
	}

	public static Dictionary<ItemCategory, string> ItemCategoryTRCodes = new()
	{
		{ ItemCategory.All,         "TR_CATEGORY_ALL" },
		{ ItemCategory.Metal,       "TR_CATEGORY_METAL" },
		{ ItemCategory.Component,   "TR_CATEGORY_COMPONENT" },
		{ ItemCategory.Tool,  		"TR_CATEGORY_INSTRUMENT" },
		{ ItemCategory.Weapon,      "TR_CATEGORY_WEAPON" },
		{ ItemCategory.Equipment,   "TR_CATEGORY_EQUIPMENT" },
		{ ItemCategory.Decor,       "TR_CATEGORY_DECOR" },
		{ ItemCategory.Misc,        "TR_CATEGORY_MISC" }
	};
}
