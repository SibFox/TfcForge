using Godot;
using System;

[GlobalClass]
// Items used in welding for a result
public partial class ItemMadeFrom : Resource
{
    [Export]
    public Item OriginalItem;
    [Export]
    public Item AdditionalItem;
}
