using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _6._6_Homework
{
    internal class Program
    {
        const string PATH_TO_FILE = "Notebook.txt";

        static void Main()
        {
            InitDb();
            Menu();
        }

        private static void Menu()
        {
            ConsoleKeyInfo consoleKeyInfo;
            do
            {
                Console.WriteLine("\nВведите 1 - вывод данных на экран.");
                Console.WriteLine("Введите 2 - добавить нового сотрудника.");
                Console.WriteLine("Введите 3 - сбросить базу.");
                Console.WriteLine("Введите 0 - чтобы выйти из программы.");
                consoleKeyInfo = Console.ReadKey();

                Console.WriteLine();

                switch (consoleKeyInfo.KeyChar)
                {
                    case '0':
                        break;
                    case '1':
                        Print();
                        break;
                    case '2':
                        Input();
                        break;
                    case '3':
                        ResetDb();
                        break;
                    default:
                        Console.WriteLine("\nВы допустили ошибку! Выберите пункт меню из предложенных!");
                        break;
                }
            } while (consoleKeyInfo.Key != ConsoleKey.D0);
        }

        private static void ResetDb()
        {
            if (CheckFileExists())
            {
                File.Delete(PATH_TO_FILE);
                InitDb();
                Console.WriteLine("База данных бы успешно очищена");
            }
        }

        private static void Input()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int nextId = GetNextIdFromDb();

            DateTime inputDate = DateTime.Now;
            string inputDateString = inputDate.ToString();

            Console.WriteLine($"Id {nextId}: Дата и время добавления записи: {inputDateString}");
            Console.WriteLine();

            stringBuilder.Append($"{nextId}#");
            stringBuilder.Append($"{inputDateString}#");

            Console.WriteLine("Введите Ф.И.О: ");
            stringBuilder.Append($"{Console.ReadLine()}#");

            Console.WriteLine("Введите возраст: ");
            RecordAge(stringBuilder);

            Console.WriteLine("Введите рост: ");
            RecordHeight(stringBuilder);

            Console.WriteLine("Введите дату рождения: ");
            RecordDate(stringBuilder);

            Console.WriteLine("Введите место рождения: ");
            stringBuilder.Append($"{Console.ReadLine()}");

            using (StreamWriter list = new StreamWriter(PATH_TO_FILE, true, Encoding.Unicode))
            {
                list.WriteLine(stringBuilder.ToString());
            }
        }

        private static void RecordDate(StringBuilder stringBuilder)
        {
            string s = Console.ReadLine();
            DateTime date;
            bool isValid = DateTime.TryParse(s, out date);
            if (isValid)
            {
                stringBuilder.Append($"{date.ToShortDateString()}#");
            }
            else
            {
                PrintInvalidMessage();
                RecordDate(stringBuilder);
            }
        }

        private static void RecordAge(StringBuilder stringBuilder)
        {
            RecordNumber(stringBuilder);
        }

        private static void RecordHeight(StringBuilder stringBuilder)
        {
            RecordNumber(stringBuilder);
        }

        private static void RecordNumber(StringBuilder stringBuilder)
        {
            string s = Console.ReadLine();
            int n;
            bool isNumber = int.TryParse(s, out n);
            if (isNumber)
            {
                stringBuilder.Append($"{n}#");
            }
            else
            {
                PrintInvalidMessage();
                RecordAge(stringBuilder);
            }
        }

        private static void PrintInvalidMessage()
        {
            Console.WriteLine("Вы ввели неверный формат. Попробуйте снова.");
        }

        private static int GetNextIdFromDb()
        {
            return File.ReadAllLines(PATH_TO_FILE).Length + 1;
        }

        private static void InitDb()
        {
            if (CheckIsNotDbInitialized())
            {
                File.Create(PATH_TO_FILE).Close();
                Console.WriteLine("Файл создан");
            }
        }

        private static bool CheckIsNotDbInitialized()
        {
            return !CheckFileExists();
        }

        private static bool CheckFileExists()
        {
            return File.Exists(PATH_TO_FILE);
        }

        private static void ValidateFileExistance()
        {
            if (!File.Exists(PATH_TO_FILE))
            {
                File.Create(PATH_TO_FILE).Close();
                Console.WriteLine("Файл создан");
            }
        }

        static void Print()
        {
            using (StreamReader db = new StreamReader(PATH_TO_FILE, Encoding.Unicode))
            {
                string line;
                PrintHeader();
                while ((line = db.ReadLine()) != null)
                {
                    PrintRow(line);
                }
            }
        }

        private static void PrintRow(string line)
        {
            string[] data = line.Split('#');
            Console.WriteLine(
                $"{data[0],5}{data[1],20} {data[2],14} {data[3],15} {data[4],15} {data[5],15} {data[6],20}");
        }

        private static void PrintHeader()
        {
            Console.WriteLine(
                $"{"Id",5}{"Дата и время",20}{"Ф.И.О",15} {"Возраст",25} {"Рост",15} {"Дата рождения",15} {"Место рождения",20}");
        }
    }
}