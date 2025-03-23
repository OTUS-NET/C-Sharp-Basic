
namespace Otus.Console.Interactive
{
    using System;
    using System.Globalization;

    public class SimpleClass
    {
        private int _a;
        public SimpleClass(int a)
        {
            _a = a;
        }


        ///// <summary>
        ///// Когда объекта класс SimpleClass выводим в Console.WriteLine
        ///// Вызывается этот метод
        ///// </summary>
        ///// <returns></returns>
        public override string ToString()
        {
            return $"{{ \"a\": {_a} }}";
        }
    }

    /// <summary>
    /// Класс для демонстрации вывода данных
    /// </summary>
    public static class WriteDemo
    {
        public static void Demo()
        {
            //Simple();
            Formatted();
        }


        static int Div(int a, int b)
        {
            return a / b;
        }

        /// <summary>
        /// Простой вывод данных
        /// </summary>
        private static void Simple()
        {
            Console.WriteLine("Выводим обычный текст с переводом строки");

            Console.WriteLine(1);
            Console.WriteLine(true);
            Console.WriteLine(DateTime.Now);


            Console.Write("Привет,");
            Console.Write(" Otus!");

            Console.WriteLine();


            var obj = new SimpleClass(4);

            Console.WriteLine(
                "1. Я пишу текст '{0}'  и число {1} и даже объект {2}",
                                "Привет", 2, obj.ToString());


            var hello = "Привет";


            Console.WriteLine($"\t2. Я пишу текст {hello} и число {2}\nи даже объект {obj} div={Div(100, 25)}");
        }

        /// <summary>
        /// Форматируемый вывод
        /// </summary>
        private static void Formatted()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            //Console.OutputEncoding = System.Text.Encoding.UTF8;

            var s = $"Вот дата обычная {DateTime.Now:dd.MM}";

            var c = DateTime.Now;

            Console.WriteLine($"Вот дата обычная {DateTime.Now}");
            Console.WriteLine($"Вот дата форматируемая {DateTime.Now:MM,dd'/'yyyy HH:mm}");
            var rubn = $"Выводим с валютой согласно языку операционной системы {100:C}";
            Console.WriteLine($"Выводим 4 знака после запятой {222212.345677890:n4}");
            Console.WriteLine($"Выводим с валютой согласно языку операционной системы {100:C}");
            Console.WriteLine($"Шестнадцатиричная система {243:X}");
            Console.WriteLine($"День недели {DateTime.Now:dddd}");

            Console.ReadKey();
        }
    }
}
