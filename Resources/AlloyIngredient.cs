using Godot;
using System;

[GlobalClass]
// Metals used to make an alloy
public partial class AlloyIngredient : Resource
{
    [Export]
    public MoltenMetal Metal;
    [Export(PropertyHint.Range, "0,100,1")]
    public int Percentage;
}
