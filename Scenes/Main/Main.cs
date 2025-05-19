using Godot;
using System;

public partial class Main : Node
{
    public MaterialSelection MaterialSelection => GetNode<MaterialSelection>("MaterialSelection");
    public ItemSelection ItemSelection => GetNode<ItemSelection>("ItemSelection");
    public InspectItem InspectItem => GetNode<InspectItem>("InspectItem");
    public Forge Forge => GetNode<Forge>("Forge");

    public Camera2D Camera => GetNode<Camera2D>("Camera");

    public override void _Ready()
    {
        Global.Main = this;
    }


    void OnChangeLanguageButtonPressed()
    {
        Global.CurrentLocale = Global.CurrentLocale == "ru" ? "en" : "ru";
        TranslationServer.SetLocale(Global.CurrentLocale);
    }
}
