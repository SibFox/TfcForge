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

    /// <summary>
    /// Amount of ingots it melts into (can be half or less of an ingot)
    /// </summary>
    [Export(PropertyHint.Range, "0.01,10,0.01")]
    public float Ingots { get; set; } = 1;
}
