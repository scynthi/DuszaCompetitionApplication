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
                break;
            default:
                Console.WriteLine("Something went wrong");
                break;
        }
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
    private void Felvetel(string cardNames)
    {
        player.AddToCollection(MatchNameArrayToCardArray(cardNames.Split(','), cards.ToArray()));
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
                    (CardElement)Enum.Parse(typeof(CardElement), command[(int)CardArrayParts.attack], true),
                    CardType.sima));
                break;

            case "vezer":
                bool contains = false;
                Card ogCard = cards[0]; // just need to asign it for the compiler
                foreach (Card card in cards)
                {
                    if (card.name == command[(int)VezerArrayParts.ogName] && card.type != CardType.vezer)
                    {
                        contains = true;
                        ogCard = card;
                    }
                }
                if (contains)
                {
                    string attrToDouble = command[(int)VezerArrayParts.doubleAttr];

                    if (attrToDouble == "health") cards.Add(new Card(
                    command[(int)VezerArrayParts.newName],
                    ogCard.attack * 2,
                    ogCard.health,
                    ogCard.element,
                    CardType.vezer));

                    else cards.Add(new Card(
                    command[(int)VezerArrayParts.newName],
                    ogCard.attack,
                    ogCard.health * 2,
                    ogCard.element,
                    CardType.vezer));
                }
                else
                {
                    Console.WriteLine("Card was not in the cards");
                }
                break;

            case "kazamata":
                Card[] kazamataCards = MatchNameArrayToCardArray(command.Skip((int)KazamataArrayParts.cards).Take(command.Length - 1 - (int)KazamataArrayParts.cards).ToArray(), cards.ToArray());

                kazamatas.Add(new Kazamata(
                    (KazamataTypes)Enum.Parse(typeof(KazamataTypes), command[(int)KazamataArrayParts.type], true),
                    command[(int)KazamataArrayParts.name],
                    kazamataCards.ToArray(),
                    (RewardType)Enum.Parse(typeof(RewardType), command[command.Length - 1], true)
                    ));
                break;

            case "jatekos":
                player = new Player();
                break;

            case "pakli":
                player.ClearPakli();
                player.AddToPakli(MatchNameArrayToCardArray(command[0].Split(','), player.pakli.ToArray()));
                break;
        }
    }
    public Card ReturnCardFromName(string name, Card[] cards)
    {
        foreach (Card card in cards)
        {
            if (name == card.name) return card;
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
}
