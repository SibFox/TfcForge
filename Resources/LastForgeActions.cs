using Godot;
using System;

[GlobalClass]
public partial class LastForgeActions : Resource
{
    // Last Required forge actions
    [Export]
    public ForgeDatabase.Action FirstAction;
    [Export]
    public ForgeDatabase.Action SecondAction;
    [Export]
    public ForgeDatabase.Action ThirdAction;
}
