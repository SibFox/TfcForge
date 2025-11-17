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
    [Export]
    public string MetalName;

    [Export]
    public bool ShowInSelection { get; set; } = true;

    /// <summary>
    /// Is set, if has an ancestor (i.e. double ingot)
    /// </summary>
    [Export]
    public ItemMadeFrom MadeFrom;

    /// <summary>
    /// What this item melts into
    /// </summary>
    [Export]
    public MeltingRecipe MeltsInto;

    /// <summary>
    /// Last required forge actions. Required to make full forge recipe
    /// If set, weld recipe doesn't count
    /// </summary>    
    [Export]
    public LastShowForgeActions LastForgeActions;

    /// <summary>
    /// Actual forge recipe. Required to make full forge recipe
    /// If set, weld recipe doesn't count
    /// </summary>
    [Export]
    public ForgeRecipe ForgeRecipe;

    /// <summary>
    /// Items used in welding for a result
    /// If set, forge recipe doesn't count
    /// </summary>
    [Export]
    public WeldRecipe WeldRecipe;
}
