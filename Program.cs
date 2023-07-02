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

            int playerDX = 0;
            int playerDY = 0;

            int allDots = 0;
            int collectDots = 0;

            char[,] map = ReadMap("Map", out int playerX, out int playerY, ref allDots, ref collectDots);

            DrawMap(map);

            while (isPlaying)
            {
                Console.SetCursorPosition(0, 15);
                Console.WriteLine($"Собрано {collectDots}/{allDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    ChangeDirection(key, ref playerDX, ref playerDY);
                }

                if (map[playerX + playerDX, playerY + playerDY] != ('|'))
                {
                    Move(ref playerX, ref playerY, playerDX, playerDY);
                    CollectDots(map, playerX, playerY, ref collectDots);
                }

                if (collectDots == allDots)
                {
                    isPlaying = false;
                }

                System.Threading.Thread.Sleep(150);
            }

            if (collectDots == allDots)
            {
                Console.SetCursorPosition(0, 16);
                Console.WriteLine("Игра окончена. Вы победили!");
            }
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int dx, ref int dy)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    dx = -1; dy = 0;
                    break;

                case ConsoleKey.DownArrow:
                    dx = 1; dy = 0;
                    break;

                case ConsoleKey.LeftArrow:
                    dx = 0; dy = -1;
                    break;

                case ConsoleKey.RightArrow:
                    dx = 0; dy = 1;
                    break;
            }
        }

        static void Move(ref int x, ref int y, int dx, int dy)
        {
            Console.SetCursorPosition(y, x);
            Console.Write(" ");

            x += dx;
            y += dy;

            Console.SetCursorPosition(y, x);
            Console.Write('@');
        }

        static void CollectDots(char[,] map, int playerX, int playerY, ref int collectDots)
        {
            if (map[playerX, playerY] == '.')
            {
                collectDots++;
                map[playerX, playerY] = ' ';
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

        static char[,] ReadMap(string mapName, out int playerX, out int playerY, ref int allDots, ref int collectDots)
        {
            playerX = 0;
            playerY = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '@')
                    {
                        playerX = i;
                        playerY = j;
                    }

                    else if (map[i, j] == ' ')
                    {
                        map[i, j] = '.';
                        allDots++;
                    }
                }
            }

            return map;
        }
    }
}