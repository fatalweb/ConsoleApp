using System;
using System.IO;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            bool isPlaying = true;

            int playerDirectionX = 0;
            int playerDirectionY = 0;

            int allDots = 0;
            int collectedDots = 0;

            char[,] map = ReadMap("Map", out int playerAxisX, out int playerAxisY, ref allDots, ref collectedDots);

            DrawMap(map);

            while (isPlaying)
            {
                Console.SetCursorPosition(0, 15);
                Console.WriteLine($"Собрано {collectedDots}/{allDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    ChangeDirection(key, ref playerDirectionX, ref playerDirectionY);
                }

                if (map[playerAxisX + playerDirectionX, playerAxisY + playerDirectionY] != ('|'))
                {
                    Move(ref playerAxisX, ref playerAxisY, playerDirectionX, playerDirectionY);
                    CollectDots(map, playerAxisX, playerAxisY, ref collectedDots);
                }

                if (collectedDots == allDots)
                {
                    isPlaying = false;
                }

                System.Threading.Thread.Sleep(150);
            }

            if (collectedDots == allDots)
            {
                Console.SetCursorPosition(0, 16);
                Console.WriteLine("Игра окончена. Вы победили!");
            }
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int playerDirectionX, ref int playerDirectionY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    playerDirectionX = -1; playerDirectionY = 0;
                    break;

                case ConsoleKey.DownArrow:
                    playerDirectionX = 1; playerDirectionY = 0;
                    break;

                case ConsoleKey.LeftArrow:
                    playerDirectionX = 0; playerDirectionY = -1;
                    break;

                case ConsoleKey.RightArrow:
                    playerDirectionX = 0; playerDirectionY = 1;
                    break;
            }
        }

        static void Move(ref int playerAxisX, ref int playerAxisY, int playerDirectionX, int playerDirectionY)
        {
            char space = ' ';
            char player = '@';

            DisplaySumbolByCoordinates(ref playerAxisX, ref playerAxisY, space);

            playerAxisX += playerDirectionX;
            playerAxisY += playerDirectionY;

            DisplaySumbolByCoordinates(ref playerAxisX, ref playerAxisY, player);
        }

        static void DisplaySumbolByCoordinates(ref int playerAxisX, ref int playerAxisY, char symbol)
        {
            Console.SetCursorPosition(playerAxisY, playerAxisX);
            Console.Write(symbol);
        }

        static void CollectDots(char[,] map, int playerAxisX, int playerAxisY, ref int collectedDots)
        {
            char space = ' ';
            char dot = '.';

            if (map[playerAxisX, playerAxisY] == dot)
            {
                collectedDots++;
                map[playerAxisX, playerAxisY] = space;
            }
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }

        static char[,] ReadMap(string mapName, out int playerAxisX, out int playerAxisY, ref int allDots, ref int collectedDots)
        {
            playerAxisX = 0;
            playerAxisY = 0;
            
            char player = '@';
            char space = ' ';
            char dot = '.';

            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == player)
                    {
                        playerAxisX = i;
                        playerAxisY = j;
                    }
                    else if (map[i, j] == space)
                    {
                        map[i, j] = dot;
                        allDots++;
                    }
                }
            }

            return map;
        }
    }
}