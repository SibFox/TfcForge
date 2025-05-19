using Godot;
using System;

[GlobalClass]
public partial class LastShowForgeActions : Resource
{
    // Last Required forge actions
    [Export]
    public ForgeDatabase.ShownAction FirstAction;
    [Export]
    public ForgeDatabase.ShownAction SecondAction;
    [Export]
    public ForgeDatabase.ShownAction ThirdAction;
}
