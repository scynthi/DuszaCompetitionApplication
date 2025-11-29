using Godot;
using System;

public partial class EndFightvisuals : Control
{
    [Export] FightLogic fightLogic;
    [Export] AnimationPlayer endFightAnimationPlayer;
    [Export] Control winScreen;
    [Export] Control loseScreen;

    [ExportGroup("Win Stuff")] 
    [Export] Label goldLabel;
    [Export] Label xpLabel;
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
        if (fightLogic.PlayerCard == null)
        {
            loseScreen.Visible = false;
            winScreen.Visible = true;

            goldLabel.Text = Convert.ToString(fightLogic);

            if (fightLogic.type == DungeonTypes.simple)
            {
                fightLogic.player.Xp += 5;
                fightLogic.player.Money += 5;
                
                winNormal.Visible = true;
                winBig.Visible = false;

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
                fightLogic.player.Xp += 10;
                fightLogic.player.Money += 10;
                
                winNormal.Visible = true;
                winBig.Visible = false;

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
                fightLogic.player.Xp += 20;
                fightLogic.player.Money += 20;
                
                winNormal.Visible = false;
                winBig.Visible = true;

                Card card = fightLogic.player.ReturnCardFromCollection(fightLogic.PlayerCard.Name);
                Utility.AddUiCardUnderContainer(card, winNormalCardContainer);
            }

        }

        
        else
        {
            loseScreen.Visible = true;
            winScreen.Visible = false;
        }


    }






}
