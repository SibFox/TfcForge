using Godot;
using System;
using System.Dynamic;

[GlobalClass]
public partial class MoltenMetal : Resource
{
    [Export]
    public Texture2D Icon;
    [Export]
    public string Name;

    // Metals used in making of this alloy
    [Export]
    public AlloyIngredient[] AlloyIngredients;
}
