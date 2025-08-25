using Godot;
using System;
using System.Dynamic;

[GlobalClass]
public partial class ForgeRecipe : Resource
{
    [Export(PropertyHint.Range, "0,150,1")]
    public int RequiredWork;

    [ExportCategory("Actions")]
    [Export(PropertyHint.Range, "0,10,1")]
    public int Shrink;
    [Export(PropertyHint.Range, "0,10,1")]
    public int Upset;
    [Export(PropertyHint.Range, "0,10,1")]
    public int Bend;
    [Export(PropertyHint.Range, "0,10,1")]
    public int Punch;
    [Export(PropertyHint.Range, "0,10,1")]
    public int WeakHit;
    [Export(PropertyHint.Range, "0,10,1")]
    public int MediumHit;
    [Export(PropertyHint.Range, "0,10,1")]
    public int StrongHit;
    [Export(PropertyHint.Range, "0,10,1")]
    public int Draw;

    [ExportCategory("Last actions")]
    [Export]
    public LastForgeActions LastActions;
}
