using Godot;
using System;

public partial class EndFightvisuals : Control
{
    [Export] FightLogic fightLogic;
    [Export] AnimationPlayer endFightAnimationPlayer;
    [Export] Control winScreen;
    [Export] Control loseScreen;

    [ExportGroup("Win Stuff")] 
    [Export] RichTextLabel goldLabel;
    [Export] RichTextLabel xpLabel;
    [Export] Control winNormal;
    [Export] Control winBig;

    [ExportGroup("WinNormal")]
    [Export] Control winNormalCardContainer;
    [Export] Control winNormalHp;
    [Export] Control winNormalDamage;

    [ExportGroup("WinBig")]
    [Export] Control winBigCardContainer;

    public override void _Ready()
    {
        fightLogic.FightEnded += _ShowFightEndVisuals;
    }

    public void _ShowFightEndVisuals()
    {
        if (fightLogic.PlayerCard != null)
        {
            loseScreen.Visible = false;
            winScreen.Visible = true;

            if (fightLogic.type == DungeonTypes.simple)
            { 
                winNormal.Visible = true;
                winBig.Visible = false;
                
                    
                goldLabel.Text = Convert.ToString(5);
                xpLabel.Text = Convert.ToString(5);

                Card card = fightLogic.player.ReturnCardFromCollection(fightLogic.PlayerCard.Name);
                Utility.AddUiCardUnderContainer(card, winNormalCardContainer);

                if (fightLogic.reward == DungeonRewardTypes.health)
                {
                    winNormalHp.Visible = true;
                    winNormalDamage.Visible = false;

                }
                else if (fightLogic.reward == DungeonRewardTypes.attack)
                {
                    winNormalHp.Visible = false;
                    winNormalDamage.Visible = true;
                }
            }
            else if (fightLogic.type == DungeonTypes.small)
            {
                winNormal.Visible = true;
                winBig.Visible = false;

                goldLabel.Text = Convert.ToString(10);
                xpLabel.Text = Convert.ToString(10);

                Card card = fightLogic.player.ReturnCardFromCollection(fightLogic.PlayerCard.Name);
                Utility.AddUiCardUnderContainer(card, winNormalCardContainer);

                if (fightLogic.reward == DungeonRewardTypes.health)
                {
                    winNormalHp.Visible = true;
                    winNormalDamage.Visible = false;

                }
                else if (fightLogic.reward == DungeonRewardTypes.attack)
                {
                    winNormalHp.Visible = false;
                    winNormalDamage.Visible = true;
                }
            }
            
            else
            {
                winNormal.Visible = false;
                winBig.Visible = true;

                goldLabel.Text = Convert.ToString(20);
                xpLabel.Text = Convert.ToString(20);

                

            }

        }

        
        else
        {
            loseScreen.Visible = true;
            winScreen.Visible = false;
        }

        endFightAnimationPlayer.Play("EndFight");
    }






}
