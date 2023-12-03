using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static void Main()
    {
        // Создаем игру
        Game game = new Game();

        // Добавляем игроков
        game.AddPlayer(new Player("Игрок 1"));
        game.AddPlayer(new Player("Игрок 2"));

        // Начинаем игру
        game.StartGame();

        // Выводим результат игры
        game.DisplayWinner();
    }
}

// Класс для представления игрока
class Player
{
    public string Name { get; }
    public List<Card> Hand { get; } = new List<Card>();

    public Player(string name)
    {
        Name = name;
    }

    public void DisplayHand()
    {
        Console.WriteLine($"{Name}'s hand:");
        foreach (var card in Hand)
        {
            Console.WriteLine($"{card.Rank} of {card.Suit}");
        }
        Console.WriteLine();
    }
}

// Класс для представления карты
class Card
{
    public string Suit { get; }
    public string Rank { get; }

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
    }
}

// Класс для представления карточной игры
class Game
{
    private List<Player> players = new List<Player>();
    private List<Card> deck = new List<Card>();

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void StartGame()
    {
        // Создаем колоду карт
        CreateDeck();

        // Перетасовываем карты
        ShuffleDeck();

        // Раздаем карты игрокам
        DealCards();

        // Игровой процесс
        PlayGame();
    }

    private void CreateDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                deck.Add(new Card(suit, rank));
            }
        }
    }

    private void ShuffleDeck()
    {
        Random random = new Random();
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Card value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }

    private void DealCards()
    {
        int numPlayers = players.Count;
        int cardsPerPlayer = deck.Count / numPlayers;

        for (int i = 0; i < numPlayers; i++)
        {
            players[i].Hand.AddRange(deck.GetRange(i * cardsPerPlayer, cardsPerPlayer));
        }
    }

    private void PlayGame()
    {
        while (deck.Count > 0)
        {
            foreach (var player in players)
            {
                // Игрок кладет одну карту
                Card playedCard = player.Hand[0];
                Console.WriteLine($"{player.Name} plays {playedCard.Rank} of {playedCard.Suit}");

                // Удаляем карту из руки игрока
                player.Hand.RemoveAt(0);

                // Добавляем карту в конец руки победителя
                players[0].Hand.Add(playedCard);

                // Выводим руки игроков
                foreach (var p in players)
                {
                    p.DisplayHand();
                }

                // Ожидаем нажатия Enter перед следующим ходом
                Console.ReadLine();
            }

            // Победитель забирает все карты
            players[0].Hand.AddRange(deck);
            deck.Clear();
        }
    }

    public void DisplayWinner()
    {
        // Определяем победителя по количеству карт в руке
        Player winner = players[0];
        foreach (var player in players)
        {
            if (player.Hand.Count > winner.Hand.Count)
            {
                winner = player;
            }
        }

        Console.WriteLine($"Победитель: {winner.Name}");
    }
}
