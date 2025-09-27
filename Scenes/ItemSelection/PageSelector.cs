using Godot;
using System;

public partial class PageSelector : HBoxContainer
{
    private byte _selectedPage = 1;

    private ItemSelection SelectionWindow => GetParent().GetParent<ItemSelection>();

    private TextureButton PreviousButton => GetNode<TextureButton>("PreviousPageButton");
    private TextureButton NextButton => GetNode<TextureButton>("NextPageButton");

    public byte SelectedPage
    {
        get => _selectedPage;
        set
        {
            _selectedPage = (byte)Mathf.Clamp(value, 1, SelectionWindow.GetMaxItemPages());

            SelectionWindow.LoadItemsFromCache();

            SetVisibilityForButtons();
        }
    }

    void PreviousPage() => SelectedPage--;

    void NextPage() => SelectedPage++;

    void SetVisibilityForButtons()
    {
        PreviousButton.Visible = _selectedPage > 1;
        NextButton.Visible = _selectedPage < SelectionWindow.GetMaxItemPages();;
    }

    void OnVisibilityChanged()
    {
        SelectedPage = 1;
        SetVisibilityForButtons();
    }
}
