using Godot;
using System;
using System.IO;
using System.Linq;

[GlobalClass]
public partial class Global : Node
{
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

	private static Main main;

	public static string CurrentLocale { get; set; } = "ru";

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

		Item metalItem = ResourceLoader.Load<Item>($"res://Content/Items/{metalName}/{metalName}Ingot.tres");

		GD.Print("[Global] Metal name from resource: " + metalName/*metalItem.Name.GetNameFromTransltaionCode()*/);
		GD.Print("[Global] Resource id: " + metalItem);

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

		Item selectedItem = ResourceLoader.Load<Item>($"res://Content/Items/{metalName}/{metalName}{itemName}.tres");

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
}
