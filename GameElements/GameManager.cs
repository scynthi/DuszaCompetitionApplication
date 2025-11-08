using DuszaCompetitionApplication.Enums;
using DuszaCompetitionApplication.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static DuszaCompetitionApplication.Utility;

namespace DuszaCompetitionApplication.GameElements;

public class GameManager
{
    private string path;
    private List<Card> cards = new List<Card>();
    private List<Card> nonInCollectionCards = new List<Card>();
    private List<Kazamata> kazamatas = new List<Kazamata>();
    private Player player = new Player();

    public GameManager(string path)
    {
        this.path = path;
    }
    public void StartGame()
    {
        string[] contents = ReadIn.ReadFile(path);
        foreach (string line in contents)
        {
            TranslateLine(line);
        }
    }
    private void TranslateLine(string line)
    {
        string[] command;
        string[] header;

        if (line.Contains(';'))
        {
            command = line.Substring(line.IndexOf(';') + 1).Split(';');
            header = line.Split(';')[0].Split(' ');
        }
        else
        {
            command = new string[] { };
            header = line.Split(' ');
        }

        switch (header[0])
        {
            case "uj":
                NewGameElement(header[1], command);
                break;
            case "felvetel":
                Felvetel(command[0]);
                break;
            case "export":
                Export(header[1], command[0]);
                break;
            case "harc":
                BattleLoop(command[0], command[1]);
                break;
            default:
                Console.WriteLine("Something went wrong");
                break;
        }
    }
    private void BattleLoop(string kazamataName, string outName)
    {
        Kazamata currKazamata = ReturnKazamataFromName(kazamataName, kazamatas.ToArray());
        List<Card> kazamataPakli = currKazamata.kazamataCards.ToList<Card>();
        Card? currentKazamataCard = null;

        List<Card> playerPakli = player.pakli.ToList<Card>();
        Card? currentPlayerCard = null;

        List<string> output = new List<string>();
        int round = 1;

        output.Add($"harc kezdodik;{kazamataName}");
        output.Add("");

        while (playerPakli.Count > 0 && kazamataPakli.Count > 0)
        {
            if (currentKazamataCard == null)
            {
                currentKazamataCard = new Card(kazamataPakli[0]);
                output.Add($"{round}.kor;kazamata;kijatszik;{currentKazamataCard.Name};{currentKazamataCard.Attack};{currentKazamataCard.Health};{currentKazamataCard.Element.ToString()}");
            }
            else
            {
                currentPlayerCard?.Damage(currentKazamataCard.Attack, currentKazamataCard.Element);
                output.Add($"{round}.kor;kazamata;tamad;{currentKazamataCard.Name};{currentKazamataCard.Health};{currentPlayerCard?.Name};{currentPlayerCard?.Health}");
            }

            if (currentPlayerCard?.Health <= 0)
            {
                playerPakli.RemoveAt(0);
                currentPlayerCard = null;
            }

            if (currentPlayerCard == null)
            {
                currentPlayerCard = new Card(playerPakli[0]);
                output.Add($"{round}.kor;jatekos;kijatszik;{currentPlayerCard.Name};{currentPlayerCard.Attack};{currentPlayerCard.Health};{currentPlayerCard.Element.ToString()}");
            }
            else
            {
                currentKazamataCard.Damage(currentPlayerCard.Attack, currentPlayerCard.Element);
                output.Add($"{round}.kor;jatekos;tamad;{currentPlayerCard.Name};{currentPlayerCard.Health};{currentKazamataCard.Name};{currentKazamataCard.Health}");
            }

            if (currentKazamataCard?.Health <= 0)
            {
                kazamataPakli.RemoveAt(0);
                currentKazamataCard = null;
            }

            output.Add("");
            round++;
        }

        if (playerPakli.Count > 0) // Player won
        {
            if (currKazamata.type == KazamataTypes.nagy)
            {
                
                output.Add($"jatekos nyert;{nonInCollectionCards[0].Name}");
                player.collection.Add(nonInCollectionCards[0]);
                nonInCollectionCards.RemoveAt(0);
            }
            else if (currentPlayerCard != null) // It's always gonna be true but the compiler didn't like it without this
            {
                if (currKazamata.reward == RewardType.eletero) player.IncreaseHealth(GetIndexOfElement(player.pakli.ToArray(), currentPlayerCard.Name));
                else player.IncreaseAttack(GetIndexOfElement(player.pakli.ToArray(), currentPlayerCard.Name));
                output.Add($"jatekos nyert;{currKazamata.reward.ToString()};{currentPlayerCard.Name}");
            }
        }
        else
        {
            output.Add($"jatekos vesztett;");
        }
        WriteOut.Battle(output.ToArray(), path, outName);

    }
    private void Export(string type, string name)
    {
        switch (type)
        {
            case "vilag":
                WriteOut.Vilag(cards.ToArray(), kazamatas.ToArray(), path, name);
                break;
            case "jatekos":
                WriteOut.Player(player, path, name);
                break;
        }
    }
    private void Felvetel(string cardName)
    {
        Card newCard = ReturnCardFromName(cardName, nonInCollectionCards.ToArray());
        nonInCollectionCards.Remove(newCard);
        player.AddToCollection(newCard);
    }
    private void NewGameElement(string type, string[] command)
    {

        switch (type)
        {
            case "kartya":
                cards.Add(new Card(
                    command[(int)CardArrayParts.name],
                    Convert.ToInt32(command[(int)CardArrayParts.attack]),
                    Convert.ToInt32(command[(int)CardArrayParts.health]),
                    (CardElement)Enum.Parse(typeof(CardElement), command[(int)CardArrayParts.element], true),
                    CardType.sima));
                break;

            case "vezer":
                bool contains = false;
                Card ogCard = cards[0]; // just need to asign it for the compiler
                foreach (Card card in cards)
                {
                    if (card.Name == command[(int)VezerArrayParts.ogName] && card.Type != CardType.vezer)
                    {
                        contains = true;
                        ogCard = card;
                    }
                }
                if (contains)
                {
                    string attrToDouble = command[(int)VezerArrayParts.doubleAttr];

                    if (attrToDouble == "sebzes") cards.Add(new Card(
                    command[(int)VezerArrayParts.newName],
                    ogCard.Attack * 2,
                    ogCard.Health,
                    ogCard.Element,
                    CardType.vezer));

                    else cards.Add(new Card(
                    command[(int)VezerArrayParts.newName],
                    ogCard.Attack,
                    ogCard.Health * 2,
                    ogCard.Element,
                    CardType.vezer));
                }
                else
                {
                    Console.WriteLine("Card was not in the cards");
                }
                break;

            case "kazamata":
                if ((KazamataTypes)Enum.Parse(typeof(KazamataTypes), command[(int)KazamataArrayParts.type], true) == KazamataTypes.nagy)
                {
                    Card[] kazamataCards = MatchNameArrayToCardArray(command.Skip((int)KazamataArrayParts.cards).Take(command.Length - (int)KazamataArrayParts.cards).ToArray(), cards.ToArray());
                    Console.Write(command[(int)KazamataArrayParts.name] + " ");
                    Console.WriteLine();
                    Utility.PrintArray(kazamataCards);
                    kazamatas.Add(new Kazamata(
                        (KazamataTypes)Enum.Parse(typeof(KazamataTypes), command[(int)KazamataArrayParts.type], true),
                        command[(int)KazamataArrayParts.name],
                        kazamataCards.ToArray(),
                        RewardType.kartya
                        ));
                }
                else
                {
                    Card[] kazamataCards = MatchNameArrayToCardArray(command.Skip((int)KazamataArrayParts.cards).Take(command.Length - 1 - (int)KazamataArrayParts.cards).ToArray(), cards.ToArray());
                    Console.Write(command[(int)KazamataArrayParts.name] + " ");
                    Utility.PrintArray(kazamataCards);
                    Console.WriteLine();
                    kazamatas.Add(new Kazamata(
                        (KazamataTypes)Enum.Parse(typeof(KazamataTypes), command[(int)KazamataArrayParts.type], true),
                        command[(int)KazamataArrayParts.name],
                        kazamataCards.ToArray(),
                        (RewardType)Enum.Parse(typeof(RewardType), command[command.Length - 1], true)
                        ));
                }
                    
                break;

            case "jatekos":
                player = new Player();
                nonInCollectionCards = new List<Card>(cards);
                break;

            case "pakli":
                player.ClearPakli();
                player.AddToPakli(MatchNameArrayToCardArray(command[0].Split(','), player.collection.ToArray()));
                break;
        }
    }
    public Kazamata ReturnKazamataFromName(string name, Kazamata[] kazamatas)
    {
        foreach (Kazamata kazamata in kazamatas)
        {
            if (name == kazamata.name) return kazamata;
        }
        return kazamatas[0];
    }
    public Card ReturnCardFromName(string name, Card[] cards)
    {
        foreach (Card card in cards)
        {
            if (name == card.Name) return card;
        }
        return cards[0];
    }

    public Card[] MatchNameArrayToCardArray(string[] names, Card[] cards)
    {
        List<Card> returnCardList = new List<Card>();
        foreach (string name in names)
        {
            returnCardList.Add(ReturnCardFromName(name, cards));
        }
        return returnCardList.ToArray();
    }
    public int GetIndexOfElement(Card[] cards, string cardName)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].Name == cardName) return i;
        }
        return -1;
    }
}
