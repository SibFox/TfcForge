using Godot;

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

	private static ConfigFile _сonfig;
    public static ConfigFile Config
    {
		get
		{
			if (_сonfig == null)
			{
				_сonfig = new();
				if (_сonfig.Load("res://GlobalConfig.ini") != Error.Ok)
					_сonfig.Save("res://GlobalConfig.ini");
			}

			return _сonfig;
		}
	}

	public static void OpenMaterialSelectionScene(Control scene)
	{
		Main.MaterialSelection.Visible = true;
		Main.Camera.Zoom = Vector2.One;
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

		Main.ItemSelection.ClearCache();
		Main.ItemSelection.SelectedMetal = metalItem;


		float zoom = (float)ProjectSettings.GetSetting("display/window/size/viewport_height") / 360;
		Main.Camera.Zoom = new(zoom, zoom);

		Main.MaterialSelection.Visible = false;
		Main.ItemSelection.Visible = true;
	}

	public static void OpenForgeScene(Item item)
	{
		GD.Print("[Global] Metal TR code: " + item.MetalName);
		GD.Print("[Global] Item TR code: " + item.Name);
		GD.Print("[Global] Item name from resource: " + item.Name.GetNameFromTransltaionCode());
		GD.Print("[Global] Resource id: " + item);

		Main.Forge.SelectedItem = item;

		float zoom = (float)ProjectSettings.GetSetting("display/window/size/viewport_height") / 360;
		Main.Camera.Zoom = new(zoom, zoom);

		Main.ItemSelection.Visible = false;
		Main.Forge.Visible = true;
	}

	public static void OpenForgeSceneWithNewItem(string metalNameTransltaionCode)
	{
		GD.Print("[Global] Metal TR code: " + metalNameTransltaionCode);

		Main.Forge.SelectedItem = new() { Name = "New Item", MetalName = metalNameTransltaionCode };
		GD.Print("[Global] New Item created");

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
