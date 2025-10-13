using Godot;
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
		{ ItemCategory.All,         "category.all" },
		{ ItemCategory.Metal,       "category.metal" },
		{ ItemCategory.Component,   "category.component" },
		{ ItemCategory.Tool,  		"category.tool" },
		{ ItemCategory.Weapon,      "category.weapon" },
		{ ItemCategory.Equipment,   "category.equipment" },
		{ ItemCategory.Decor,       "category.decor" },
		{ ItemCategory.Misc,        "category.misc" }
	};
}
