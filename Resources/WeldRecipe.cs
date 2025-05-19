using Godot;
using System;

[GlobalClass]
// Items used in welding for a result
public partial class WeldRecipe : Resource
{
    [Export]
    public Item FirstItem;
    [Export]
    public Item SecondItem;
}
