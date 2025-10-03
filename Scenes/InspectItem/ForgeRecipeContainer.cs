using Godot;
using System;
using System.Text;
using static Godot.TranslationServer;

public partial class ForgeRecipeContainer : VBoxContainer
{
    private InspectItem InspectItem => GetParent().GetParent().GetParent<InspectItem>();

    private Item CurrentItem
    {
        get => InspectItem.CurrentItem;
        set => InspectItem.CurrentItem = value;
    }

    private int CurrentForgeWork
    {
        get => InspectItem.CurrentForgeWork;
        set => InspectItem.CurrentForgeWork = value;
    }

    public Label ForgeWorkResultLabel => GetNode<Label>("LabelHBoxContainer/MarginContainer/ForgeWorkResultLabel");
    public Label ActionWeightLabel => GetNode<Label>("ActionWeightMarginContainer/ActionWeightLabel");
    public Label MathLabel => GetNode<Label>("MathMarginContainer/MathLabel");
    public Label ActionsLabel => GetNode<Label>("ActionsMarginContainer/ActionsLabel");

    

    public void SetForgeRecipe()
    {
        Visible = true;

        StringBuilder actionWeightString = new();
        StringBuilder mathString = new();
        StringBuilder actionsString = new();

        ForgeRecipe recipe = CurrentItem.ForgeRecipe;

        ForgeWorkResultLabel.Text = recipe.RequiredWork.ToString();

        //Shrink / Ужать
        for (int i = 0; i < recipe.Shrink; i++)
        {
            CurrentForgeWork += (int)ForgeDatabase.Action.Shrink;
            if (i == 0)
            {
                actionWeightString.Append((int)ForgeDatabase.Action.Shrink);
                mathString.Append(CurrentForgeWork);
                actionsString.Append(Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Shrink]) + " x" + recipe.Shrink);
            }
            else
            {
                actionWeightString.Append(" + " + (int)ForgeDatabase.Action.Shrink);
                mathString.Append(" > " + CurrentForgeWork);
            }
        }
        
        // Upset / Обжать
        for (int i = 0; i < recipe.Upset; i++)
        {
            CurrentForgeWork += (int)ForgeDatabase.Action.Upset;

            if (i == 0)
                actionsString.Append(" + " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Upset]) + " x" + recipe.Upset);

            actionWeightString.Append(" + " + (int)ForgeDatabase.Action.Upset);
            mathString.Append(" > " + CurrentForgeWork);
        }

        // Bend / Изогнуть
        for (int i = 0; i < recipe.Bend; i++)
        {
            CurrentForgeWork += (int)ForgeDatabase.Action.Bend;

            if (i == 0)
                actionsString.Append(" + " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Bend]) + " x" + recipe.Bend);

            actionWeightString.Append(" + " + (int)ForgeDatabase.Action.Bend);
            mathString.Append(" > " + CurrentForgeWork);
        }

        // Punch / Штамп
        for (int i = 0; i < recipe.Punch; i++)
        {
            CurrentForgeWork += (int)ForgeDatabase.Action.Punch;

            if (i == 0)
                actionsString.Append(" + " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Punch]) + " x" + recipe.Punch);

            actionWeightString.Append(" + " + (int)ForgeDatabase.Action.Punch);
            mathString.Append(" > " + CurrentForgeWork);
        }

        // Weak hit / Слабый удар
        for (int i = 0; i < recipe.WeakHit; i++)
        {
            CurrentForgeWork += (int)ForgeDatabase.Action.WeakHit;

            if (i == 0)
                actionsString.Append(" - " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.WeakHit]) + " x" + recipe.WeakHit);

            actionWeightString.Append(" - " + Math.Abs((int)ForgeDatabase.Action.WeakHit));
            mathString.Append(" > " + CurrentForgeWork);
        }


        // Medium hit / Средний удар
        for (int i = 0; i < recipe.MediumHit; i++)
        {
            CurrentForgeWork += (int)ForgeDatabase.Action.MediumHit;

            if (i == 0)
                actionsString.Append(" - " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.MediumHit]) + " x" + recipe.MediumHit);

            actionWeightString.Append(" - " + Math.Abs((int)ForgeDatabase.Action.MediumHit));
            mathString.Append(" > " + CurrentForgeWork);
        }

        // Strong hit / Сильный удар
        for (int i = 0; i < recipe.StrongHit; i++)
        {
            CurrentForgeWork += (int)ForgeDatabase.Action.StrongHit;

            if (i == 0)
                actionsString.Append(" - " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.StrongHit]) + " x" + recipe.StrongHit);

            actionWeightString.Append(" - " + Math.Abs((int)ForgeDatabase.Action.StrongHit));
            mathString.Append(" > " + CurrentForgeWork);
        }

        // Draw / Протянуть
        for (int i = 0; i < recipe.Draw; i++)
        {
            CurrentForgeWork += (int)ForgeDatabase.Action.Draw;

            if (i == 0)
                actionsString.Append(" - " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Draw]) + " x" + recipe.Draw);

            actionWeightString.Append(" - " + Math.Abs((int)ForgeDatabase.Action.Draw));
            mathString.Append(" > " + CurrentForgeWork);
        }

        mathString.Append(" >>[ ");
        actionWeightString.Append(" >>[ ");
        actionsString.Append(" >>[ ");

        byte actionAmount = 3;
        if (recipe.LastActions.SecondAction == ForgeDatabase.Action.None)
            actionAmount--;
        if (recipe.LastActions.ThirdAction == ForgeDatabase.Action.None)
            actionAmount--;
        for (int i = 1; i <= actionAmount; i++)
        {
            ForgeDatabase.Action action = ForgeDatabase.Action.None;
            switch (i)
            {
                case 1:
                    action = recipe.LastActions.FirstAction;
                    break;
                case 2:
                    action = recipe.LastActions.SecondAction;
                    break;
                case 3:
                    action = recipe.LastActions.ThirdAction;
                    break;
            }

            if (action < 0)
            {
                actionsString.Append("- ");
                actionWeightString.Append("- ");
            }
            else
            {
                actionsString.Append("+ ");
                actionWeightString.Append("+ ");
            }

            CurrentForgeWork += (int)action;

            mathString.Append("-> " + CurrentForgeWork + " ");
            actionWeightString.Append(Math.Abs((int)action) + " ");
            actionsString.Append(Translate(ForgeDatabase.ActionTRCodes[action]) + " ");
        }

        actionsString.Replace(" + ", "", 0, 3);
        actionWeightString.Replace(" + ", "", 0, 3);
        mathString.Replace(" > ", "", 0, 3);
        ActionWeightLabel.Text = actionWeightString.Append("[!]").ToString();
        MathLabel.Text = mathString.Append("[!]").ToString();
        ActionsLabel.Text = actionsString.Append("[!]").ToString();
    }
}
