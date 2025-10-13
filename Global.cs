using Godot;
using static TfcForge.Common.Logger.Logger;

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
				DebugInfo(nameof(Global)).AddLine("Main is set").Push();
			}
		}
	}

	public static string CurrentLocale
	{
		get => (string)Config.GetValue("language", "locale", "en");
		set
		{
			if (value != null || value.Length == 0)
            {
				Config.SetValue("language", "locale", value);
				Config.Save(Paths.GlobalConfigPath);
				LogInfo(nameof(MaterialSelection)).AddLine("Locale changed to:", value).Push();
				DebugInfo(nameof(MaterialSelection)).AddLine("Control value:", Config.GetValue("language", "locale")).Push();
			}
			else
				DebugErr(nameof(Global), nameof(IngotCost)).AddLine("Cannot set empty locale").Push();
        }
	}

	private static ConfigFile _сonfig;
    private static ConfigFile Config
    {
		get
		{
			if (_сonfig == null)
			{
				_сonfig = new();
				if (_сonfig.Load(Paths.GlobalConfigPath) != Error.Ok)
					_сonfig.Save(Paths.GlobalConfigPath);
				DebugInfo(nameof(Global)).AddLine("Global config created").Push();
			}

			return _сonfig;
		}
	}

	public static int IngotCost
    {
		get => (int)Config.GetValue("ingot", "cost", 100);
		set
		{
			if (value > 0 && value < 1000)
			{
				Config.SetValue("ingot", "cost", value);
				Config.Save(Paths.GlobalConfigPath);
				LogInfo(nameof(MaterialSelection)).AddLine("Ingot cost changed to", value).Push();
				DebugInfo(nameof(MaterialSelection)).AddLine("Control value:", Config.GetValue("ingot", "cost")).Push();
			}
			else
				DebugErr(nameof(Global), nameof(IngotCost)).AddLine("Value should be between 0 and 1000").Push();
        }
    }



	public override void _Ready()
	{
		if (!DirAccess.DirExistsAbsolute(Paths.LogsPath[..^1]))
		{
			DirAccess.MakeDirAbsolute(Paths.LogsPath[..^1]);
		}

		FileAccess.Open(Paths.LogsPath + "latest.log", FileAccess.ModeFlags.Write).Close();
		FileAccess.Open(Paths.LogsPath + "debug.log", FileAccess.ModeFlags.Write).Close();

		LogInfo(nameof(Global)).AddLine("TFC Forge initialized").Push();
	}
	


	public static void OpenMaterialSelectionScene(Control scene)
	{
		Main.MaterialSelection.Visible = true;
		Main.Camera.Zoom = Vector2.One;
		scene.Visible = false;
	}

	public static void OpenItemSelectionScene(string metalNameTransltaionCode)
	{
		string metalName = metalNameTransltaionCode.GetNameFromTransltaionCode();
		Item metalItem = ResourceLoader.Load<Item>(Paths.Items + $"{metalName}/Ingot.tres");

		DebugInfo(nameof(Global), "ItemSelection").AddLine("Metal TR code:", metalNameTransltaionCode)
												  .AddLine("Metal name transformed:", metalName)
												  .AddLine("Resource id:", metalItem)
												  .Push();

		Main.ItemSelection.ClearCache();
		Main.ItemSelection.SelectedMetal = metalItem;

		float zoom = (float)ProjectSettings.GetSetting("display/window/size/viewport_height") / 360;
		Main.Camera.Zoom = new(zoom, zoom);

		Main.MaterialSelection.Visible = false;
		Main.ItemSelection.Visible = true;
	}

	public static void OpenForgeScene(Item item)
	{
		DebugInfo(nameof(Global), "Forge").AddLine("Metal TR code:", item.MetalName)
								  		  .AddLine("Item TR code:", item.Name)
								  		  .AddLine("Item name transformed:", item.Name.GetNameFromTransltaionCode())
										  .AddLine("Resource id:", item)
										  .Push();

		Main.Forge.SelectedItem = item;

		float zoom = (float)ProjectSettings.GetSetting("display/window/size/viewport_height") / 360;
		Main.Camera.Zoom = new(zoom, zoom);

		Main.ItemSelection.Visible = false;
		Main.Forge.Visible = true;
	}

	public static void OpenForgeSceneWithNewItem(string metalNameTransltaionCode)
	{
		DebugInfo(nameof(Global), "New Forge").AddLine("Metal TR code:", metalNameTransltaionCode).Push();

		Main.Forge.SelectedItem = new() { Name = "New Item", MetalName = metalNameTransltaionCode };
		DebugInfo(nameof(Global), "New Forge").AddLine("New Item created").Push();

		float zoom = (float)ProjectSettings.GetSetting("display/window/size/viewport_height") / 360;
		Main.Camera.Zoom = new(zoom, zoom);

		Main.ItemSelection.Visible = false;
		Main.Forge.Visible = true;	
	}

	public static void OpenInspectItemScene(Item item)
	{
		Main.ItemSelection.Visible = false;

		Main.InspectItem.CurrentItem = item;
		Main.InspectItem.Visible = true;
	}

	public static class Paths
	{
		public const string LogsPath = "user://Logs/";
		public const string GlobalConfigPath = "user://GlobalConfig.ini";

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
