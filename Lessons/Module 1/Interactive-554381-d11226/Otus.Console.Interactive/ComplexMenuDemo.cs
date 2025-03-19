namespace Otus.Console.Interactive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    class ComplexMenuDemo
    {

        /// <summary>
        /// Опции меню
        /// </summary>
        private static string[] options = new[]{
            "Новая игра",
            "Загрузить игру",
            "Сохранить Игру",
            "Настройки",
        };

        /// <summary>
        /// На одну строку вниз
        /// </summary>
        private static void SetDown()
        {
            if (selectedValue < options.Length)
            {
                selectedValue++;
            }
            else
            {
                selectedValue = 1;
            }
        }

        /// <summary>
        /// На одну строку вверх
        /// </summary>
        private static void SetUp()
        {
            if (selectedValue > 1)
            {
                selectedValue--;
            }
            else
            {
                selectedValue = 4;
            }
        }

        /// <summary>
        /// Вывести меню
        /// </summary>
        private static void PrintMenu()
        {
            Console.WriteLine("Для выхода нажмите Escape");
            for (var i = 0; i < options.Length; i++)
            {
                Console.WriteLine($" {i + 1}. {options[i]}");
            }
        }

        /// <summary>
        /// Исходное положение стрелки меню
        /// </summary>
        private static int selectedValue = 0;

        private static void WriteCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(">");
            Console.SetCursorPosition(0, pos);
        }

        private static void ClearCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(" ");
            Console.SetCursorPosition(0, pos);
        }

        /// <summary>
        /// Запуск меню
        /// </summary>
        private static void Start()
        {
            ConsoleKeyInfo ki;

            selectedValue = 1;

            PrintMenu();
            WriteCursor(selectedValue);
            do
            {
                ki = Console.ReadKey();
                ClearCursor(selectedValue);
                switch (ki.Key)
                {
                    case ConsoleKey.UpArrow:
                        SetUp();
                        break;
                    case ConsoleKey.DownArrow:
                        SetDown();
                        break;
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                    case ConsoleKey.D4:
                        selectedValue = int.Parse(ki.KeyChar.ToString());
                        break;
                }
                WriteCursor(selectedValue);
            } while (ki.Key != ConsoleKey.Escape);

        }

        public static void Demo()
        {
            //Krutilca();

            Start();
        }

        private static void Clear()
        {
            var i = 0;
            while (i < 10)
            {
                Console.WriteLine($"Строка под номером {i++}");
            }

            Console.Write("Сейчас все очищу, нажмите любую клавишу");
            Console.ReadKey();
            Console.Clear();
            Console.Write("Все чисто");
        }


        /// <summary>
        /// Крутилка в консоли (демонстрация CursorPosition)
        /// </summary>
        private static void Krutilca()
        {
            var a = new char[] { '\\', '|', '/', '-' };

            Console.Write("\\ Начинаем крутиться");
            var i = 0;

            do
            {
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                Console.Write(a[(i++) % 4]);
                Thread.Sleep(300);
                // Прокручиваем 12 раз
            } while (i < 48);
        }
    }
}
