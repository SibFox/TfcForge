using Godot;
using System;
using System.Dynamic;

[GlobalClass]
public partial class Item : Resource
{
    [Export]
    public Texture2D Icon;
    [Export]
    public string Name;
    [Export]
    public ItemDatabase.ItemCategory Category;

    // Is set, if has an ancestor (i.e. double ingot)
    [Export]
    public ItemMadeFrom MadeFrom;

    /// <summary>
    /// What this item melts into
    /// </summary>
    [Export]
    public MeltingRecipe MeltsInto;

    // Last required forge actions. Required to make full forge recipe
    // If set, weld recipe doesn't count
    [Export]
    public LastShowForgeActions LastForgeActions;

    // Actual forge recipe. Required to make full forge recipe
    // If set, weld recipe doesn't count
    [Export]
    public ForgeRecipe ForgeRecipe;

    // Items used in welding for a result
    // If set, forge recipe doesn't count
    [Export]
    public WeldRecipe WeldRecipe;
}
