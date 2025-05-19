using Godot;
using System;

[GlobalClass]
public partial class MeltingRecipe : Resource
{
    
    /// <summary>
    /// What metall this item melts into 
    /// </summary>
    [Export]
    public MoltenMetal MeltsInto;

    [Export(PropertyHint.Range, "100,800,100")]
    public int Millibuckets;
}
