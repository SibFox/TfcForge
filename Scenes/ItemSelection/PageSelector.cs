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
            byte maxPages = SelectionWindow.GetMaxItemPages();
            _selectedPage = (byte)Mathf.Clamp(value, 1, maxPages);

            SelectionWindow.LoadItemsFromCache();

            PreviousButton.Visible = _selectedPage > 1;
            NextButton.Visible = _selectedPage < maxPages;
        }
    }

    void PreviousPage() => SelectedPage--;

    void NextPage() => SelectedPage++;

    void OnVisibilityChanged()
    {
        SelectedPage = 1;
    }
}
