using DuszaCompetitionApplication.Enums;
using DuszaCompetitionApplication.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using static DuszaCompetitionApplication.Utility;

namespace DuszaCompetitionApplication.GameElements;

public class GameManager
{
    private GameModes gameMode;
    private string path;
    private List<Card> cards = new List<Card>();
    private List<Card> nonInCollectionCards = new List<Card>();
    private List<Kazamata> kazamatas = new List<Kazamata>();
    private Player player = new Player();

    public GameManager(string path, GameModes gameMode)
    {
        this.path = path;
        this.gameMode = gameMode;
    }
    public void StartGameMode()
    {
        path = Directory.GetCurrentDirectory() + @"\GameMode\";
        string[] contents = ReadIn.ReadFile(path);
        foreach (string line in contents)
        {
            TranslateLine(line);
        }
    }
    public void StartTestMode()
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
    public void BattleLoop(string kazamataName, string outName)
    {
        Kazamata currKazamata;
        if (!TryReturnKazamataFromName(kazamataName, kazamatas.ToArray(), out currKazamata)) return;
        List<Card> kazamataPakli = currKazamata.KazamataCards
        .Select(c => c.Clone())
        .ToList();
        Card? currentKazamataCard = null;

        List<Card> playerPakli = player.pakli
        .Select(c => c.Clone())
        .ToList();
        Card? currentPlayerCard = null;

        List<string> output = new List<string>();
        int round = 1;

        output.Add($"harc kezdodik;{kazamataName}");
        output.Add("");

        int damageNum;

        while (playerPakli.Count > 0 && kazamataPakli.Count > 0)
        {
            if (currentKazamataCard == null)
            {
                currentKazamataCard = new Card(kazamataPakli[0]);
                output.Add($"{round}.kor;kazamata;kijatszik;{currentKazamataCard.Name};{currentKazamataCard.Attack};{currentKazamataCard.Health};{currentKazamataCard.Element.ToString()}");
            }
            else
            {
                if (currentPlayerCard != null)
                {
                    damageNum = currentPlayerCard.Damage(currentKazamataCard.Attack, currentKazamataCard.Element);
                    output.Add($"{round}.kor;kazamata;tamad;{currentKazamataCard.Name};{damageNum};{currentPlayerCard?.Name};{currentPlayerCard?.Health}");
                }
            }

            if (currentPlayerCard?.Health <= 0)
            {
                playerPakli.RemoveAt(0);
                currentPlayerCard = null;
            }

            if (playerPakli.Count == 0) // Check if the player ran out of cards
            {
                output.Add("");
                break;
            }

            if (currentPlayerCard == null)
            {
                currentPlayerCard = new Card(playerPakli[0]);
                output.Add($"{round}.kor;jatekos;kijatszik;{currentPlayerCard.Name};{currentPlayerCard.Attack};{currentPlayerCard.Health};{currentPlayerCard.Element.ToString()}");
            }
            else
            {
                if (currentKazamataCard != null)
                {
                    damageNum = currentKazamataCard.Damage(currentPlayerCard.Attack, currentPlayerCard.Element);
                    output.Add($"{round}.kor;jatekos;tamad;{currentPlayerCard.Name};{damageNum};{currentKazamataCard.Name};{currentKazamataCard.Health}");
                }
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
                if (nonInCollectionCards.Count > 0)
                {
                    output.Add($"jatekos nyert;{nonInCollectionCards[0].Name}");
                    player.collection.Add(nonInCollectionCards[0]);
                    nonInCollectionCards.RemoveAt(0);
                }
                else
                {
                    output.Add($"jatekos nyert");
                }
            }
            else if (currentPlayerCard != null) // It's always gonna be true but the compiler didn't like it without this
            {
                if (currKazamata.reward == RewardType.eletero) player.IncreaseHealth(GetIndexOfElement(player.collection.ToArray(), currentPlayerCard.Name));
                else player.IncreaseAttack(GetIndexOfElement(player.collection.ToArray(), currentPlayerCard.Name));
                output.Add($"jatekos nyert;{currKazamata.reward.ToString()};{currentPlayerCard.Name}");
            }
        }
        else
        {
            output.Add($"jatekos vesztett;");
        }
        if (gameMode == GameModes.Test) WriteOut.Battle(output.ToArray(), path, outName);
        else
        {
            PrintBattle(output);
        }
        PrintArray(playerPakli);
        PrintArray(player.collection);
        PrintArray(player.pakli);
    }

    public void BattleLoopUI(string kazamataName, string outName, out List<string> output)
    {

        Kazamata currKazamata;
        output = new List<string>();
        if (!TryReturnKazamataFromName(kazamataName, kazamatas.ToArray(), out currKazamata)) return;
        List<Card> kazamataPakli = currKazamata.KazamataCards
        .Select(c => c.Clone())
        .ToList();

        Card? currentKazamataCard = null;

        List<Card> playerPakli = player.pakli
        .Select(c => c.Clone())
        .ToList();
        Card? currentPlayerCard = null;

        int round = 1;

        output.Add($"harc kezdodik;{kazamataName}");

        int damageNum;

        while (playerPakli.Count > 0 && kazamataPakli.Count > 0)
        {
            if (currentKazamataCard == null)
            {
                currentKazamataCard = new Card(kazamataPakli[0]);
                output.Add($"{round}.kor;kazamata;kijatszik;{currentKazamataCard.Name};{currentKazamataCard.Attack};{currentKazamataCard.Health};{currentKazamataCard.Element.ToString()}");
            }
            else
            {
                if (currentPlayerCard != null)
                {
                    damageNum = currentPlayerCard.Damage(currentKazamataCard.Attack, currentKazamataCard.Element);
                    output.Add($"{round}.kor;kazamata;tamad;{currentKazamataCard.Name};{damageNum};{currentPlayerCard?.Name};{currentPlayerCard?.Health}");
                }
            }

            if (currentPlayerCard?.Health <= 0)
            {
                playerPakli.RemoveAt(0);
                currentPlayerCard = null;
            }

            if (playerPakli.Count == 0) // Check if the player ran out of cards
            {
                break;
            }

            if (currentPlayerCard == null)
            {
                currentPlayerCard = new Card(playerPakli[0]);
                output.Add($"{round}.kor;jatekos;kijatszik;{currentPlayerCard.Name};{currentPlayerCard.Attack};{currentPlayerCard.Health};{currentPlayerCard.Element.ToString()}");
            }
            else
            {
                if (currentKazamataCard != null)
                {
                    damageNum = currentKazamataCard.Damage(currentPlayerCard.Attack, currentPlayerCard.Element);
                    output.Add($"{round}.kor;jatekos;tamad;{currentPlayerCard.Name};{damageNum};{currentKazamataCard.Name};{currentKazamataCard.Health}");
                }
            }

            if (currentKazamataCard?.Health <= 0)
            {
                kazamataPakli.RemoveAt(0);
                currentKazamataCard = null;
            }

            round++;
            
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
    private void Felvetel(string cardName)
    {
        Card newCard;
        if (TryReturnCardFromName(cardName, nonInCollectionCards.ToArray(), out newCard))
        {
            nonInCollectionCards.RemoveAll(c => c.Name == newCard.Name);
            player.AddToCollection(newCard);
        }
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
                Card ogCard = cards[0]; // just need to assign it for the compiler
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

                    else if (attrToDouble == "eletero") cards.Add(new Card(
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
                    List<string> names = command[(int)KazamataArrayParts.cards].Split(',').ToList();
                    names.Add(command[(int)KazamataArrayParts.vezerCard]);
                    Card[] kazamataCards = TryMatchNameArrayToCardArray(names.ToArray(), cards.ToArray());

                    kazamatas.Add(new Kazamata(
                        (KazamataTypes)Enum.Parse(typeof(KazamataTypes), command[(int)KazamataArrayParts.type], true),
                        command[(int)KazamataArrayParts.name],
                        kazamataCards.ToArray(),
                        RewardType.kartya
                        ));
                }
                else if ((KazamataTypes)Enum.Parse(typeof(KazamataTypes), command[(int)KazamataArrayParts.type], true) == KazamataTypes.kis)
                {
                    List<string> names = command[(int)KazamataArrayParts.cards].Split(',').ToList();
                    names.Add(command[(int)KazamataArrayParts.vezerCard]);
                    Card[] kazamataCards = TryMatchNameArrayToCardArray(names.ToArray(), cards.ToArray());

                    kazamatas.Add(new Kazamata(
                        (KazamataTypes)Enum.Parse(typeof(KazamataTypes), command[(int)KazamataArrayParts.type], true),
                        command[(int)KazamataArrayParts.name],
                        kazamataCards.ToArray(),
                        (RewardType)Enum.Parse(typeof(RewardType), command[command.Length - 1], true)
                        ));
                }
                else
                {
                    List<string> names = command[(int)KazamataArrayParts.cards].Split(',').ToList();
                    Card[] kazamataCards = TryMatchNameArrayToCardArray(names.ToArray(), cards.ToArray());

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
                nonInCollectionCards = cards
                .Where(card => card.Type == CardType.sima)
                .Select(card => card.Clone())
                .ToList();
                break;

            case "pakli":
                player.ClearPakli();
                player.AddToPakli(MatchNameArrayToCardArray(command[0].Split(','), player.collection.ToArray()));
                break;
        }
    }
    public bool TryReturnKazamataFromName(string name, Kazamata[] kazamatas, out Kazamata rKazamata)
    {
        rKazamata = new Kazamata();
        foreach (Kazamata kazamata in kazamatas)
        {
            if (name == kazamata.Name) { rKazamata = kazamata; return true; }
        }
        return false;
    }
    public bool TryReturnCardFromName(string name, Card[] cards, out Card rCard)
    {
        rCard = new Card();
        foreach (Card card in cards)
        {
            if (name == card.Name) { rCard = new Card(card); return true; }
        }
        return false;
    }

    public Card ReturnCardFromName(string name, Card[] cards)
    {
        foreach (Card card in cards)
        {
            if (name == card.Name) return card;
        }
        Console.WriteLine("buh");
        return cards[0];
    }

    public Card[] TryMatchNameArrayToCardArray(string[] names, Card[] cards)
    {
        List<Card> returnCardList = new List<Card>();
        Card tempCard;
        foreach (string name in names)
        {
            if (TryReturnCardFromName(name, cards, out tempCard)) returnCardList.Add(tempCard);
        }
        return returnCardList.ToArray();
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
        Console.WriteLine("APPLE");
        return -1;
    }
    public List<string> GetKazamatas()
    {
        List<string> names = new List<string>();
        foreach (Kazamata kazamata in kazamatas) names.Add(kazamata.Name);
        return names;
    }
    public List<string> GetCardsCollection()
    {
        List<string> names = new List<string>();
        foreach (Card card in player.collection) names.Add(card.Name);
        return names;
    }
    public bool TryAddCardToPakli(Card card)
    {
        if (player.pakli.Count < Math.Ceiling((double)player.collection.Count / 2))
        {
            player.AddToPakli(card);
            return true;
        }
        return false;
    }
    public bool TryAddCardToPakli(Card[] cards)
    {
        foreach (Card card in cards)
        {
            if (player.pakli.Count < Math.Ceiling((double)player.collection.Count / 2))
            {
                player.AddToPakli(card);
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public bool RemoveFromPakli(Card card)
    {
        for (int i = 0; i < player.pakli.Count; i++)
        {
            if (player.pakli[i].Name == card.Name)
            {
                player.RemoveFromPakli(i);
                return true;
            }
        }
        return false;
    }
    public List<Card> GetCollection()
    {
        return player.collection;
    }
    public List<Card> GetAllCards()
    {
        return cards;
    }
    public List<Card> GetPakli()
    {
        return player.pakli;
    }
    public void ClearPakli()
    {
        player.ClearPakli();
    }
    public Kazamata[] GetKazataObjects()
    {
        return kazamatas.ToArray();
    }
}
