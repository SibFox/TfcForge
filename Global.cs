using Godot;
using System;
using System.IO;
using System.Linq;

[GlobalClass]
public partial class Global : Node
{
	private static Main main;
	public static Main Main
	{
		get => main;
		set
		{
			if (value is Main)
			{
				main = value;
			}
		}
	}

	public static string CurrentLocale { get; set; } = "ru";

	private static ConfigFile _globalConfig;
    public static ConfigFile GlobalConfig
    {
		get
		{
			if (_globalConfig == null)
			{
				_globalConfig = new();
				if (_globalConfig.Load("res://GlobalConfig.ini") != Error.Ok)
					_globalConfig.Save("res://GlobalConfig.ini");
			}

			return _globalConfig;
		}
	}

	public static void OpenMaterialSelectionScene(Control scene)
	{
		main.MaterialSelection.Visible = true;
		main.Camera.Zoom = Vector2.One;
		scene.Visible = false;
	}

	public static void OpenItemSelectionScene(string metalNameTransltaionCode)
	{
		GD.Print("[Global] Metal TR code: " + metalNameTransltaionCode);
		string metalName = metalNameTransltaionCode.GetNameFromTransltaionCode();
		GD.Print("[Global] Metal name translated: " + metalName);

		Item metalItem = ResourceLoader.Load<Item>(Paths.Items + $"{metalName}/Ingot.tres");

		GD.Print("[Global] Metal name from resource: " + metalName);
		GD.Print("[Global] Resource id: " + metalItem);

		main.ItemSelection.ClearCache();
		main.ItemSelection.SetMetal(metalItem, metalName);


		float zoom = (float)ProjectSettings.GetSetting("display/window/size/viewport_height") / 360;
		main.Camera.Zoom = new(zoom, zoom);

		main.MaterialSelection.Visible = false;
		main.ItemSelection.Visible = true;
	}

	public static void OpenForgeScene(string metalNameTransltaionCode, string itemNameTransltaionCode)
	{
		GD.Print("[Global] Metal TR code: " + metalNameTransltaionCode);
		string metalName = metalNameTransltaionCode.GetNameFromTransltaionCode();

		GD.Print("[Global] Item TR code: " + itemNameTransltaionCode);
		string itemName = itemNameTransltaionCode.GetNameFromTransltaionCode();

		Item selectedItem = ResourceLoader.Load<Item>(Paths.Items + $"{metalName}/{itemName}.tres");

		GD.Print("[Global] Item name from resource: " + selectedItem.Name.GetNameFromTransltaionCode());
		GD.Print("[Global] Resource id: " + selectedItem);

		main.Forge.SelectedItem = selectedItem;

		float zoom = (float)ProjectSettings.GetSetting("display/window/size/viewport_height") / 360;
		main.Camera.Zoom = new(zoom, zoom);

		main.ItemSelection.Visible = false;
		main.Forge.Visible = true;
	}

	public static void OpenInspectItemScene(Item item)
	{
		main.ItemSelection.Visible = false;

		main.InspectItem.SetItem(item);
		main.InspectItem.Visible = true;
	}

	public static class Paths
	{
		public const string Content = "res://Content/";
		public const string Assets = "res://Assets/";
		public const string ItemSprites = Assets + "Sprites/";
		public const string ForgeSprites = Assets + "Forge/";
		public const string Items = Content + "Items/";
		public const string LastForgeActions = Content + "Last Shown Forge Actions/";
		public const string MoltenMetals = Content + "MoltenMetals/";

		public const string Scenes = "res://Scenes/";
		public const string UI = Scenes + "UI/";
		
	}
}
