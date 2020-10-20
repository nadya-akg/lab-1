using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookConsole
{
    class Program
    {
        private static DataManager dataManager;

        /// <summary>
        /// Весь список записей
        /// </summary>
        private static List<NotebookItem> notebook = new List<NotebookItem>();

        static void Main(string[] args)
        {
            dataManager = new DataManager();
            notebook = dataManager.LoadNotebookItems();

            ShowMenu();
        }

        /// <summary>
        /// Показать меню с действиями
        /// </summary>
        private static void ShowMenu()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1) Добавить запись");
            Console.WriteLine("2) Удалить запись");
            Console.WriteLine("3) Показать все записи");
            Console.WriteLine("4) Поиск записи");
            Console.WriteLine("5) Выход");

            Console.WriteLine("Введите номер действия:");
            int n = GetIntFromUserInput(1, 5);

            switch (n)
            {
                case 1:
                    AddNotebookItem();
                    break;
                case 2:
                    RemoveNotebookItem();
                    break;
                case 3:
                    ShowData();
                    break;
                case 4:
                    SearchNotebookItem();
                    break;
                case 5:
                    return;
            }
        }

        /// <summary>
        /// Получить целочисленное число из введеного пользователем текста
        /// <param name="min">Минимальное значение, которое может ввести пользователь</param>
        /// <param name="max">Максимальное значение, которое может ввести пользователь</param>
        /// </summary>
        /// <returns>Число</returns>
        private static int GetIntFromUserInput(int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                var textN = Console.ReadLine();
                int value;
                if (!int.TryParse(textN, out value))
                {
                    Console.WriteLine("Введёное значение не удалось привести к числу. Попробуйте ещё раз.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine($"Введёное значение должно быть в диапазоне [{min},{max}]. Попробуйте ещё раз.");
                    continue;
                }

                return value;
            }
        }
        
        /// <summary>
        /// Получить дату из введеного пользователем текста
        /// </summary>
        /// <returns>Дата</returns>
        private static DateTime GetDateTimeFromUserInput()
        {
            while (true)
            {
                var text = Console.ReadLine();

                if (!DateTime.TryParse(text, out DateTime value))
                {
                    Console.WriteLine("Введёное значение не удалось привести к дате. Попробуйте ещё раз.");
                    continue;
                }

                return value;
            }
        }        

        /// <summary>
        /// Получить не пустую строку введеного пользователем текста
        /// </summary>
        /// <returns>Строка</returns>
        private static string GetNotNullStringFromUserInput()
        {
            while (true)
            {
                var text = Console.ReadLine();

                if (string.IsNullOrEmpty(text))
                {
                    Console.WriteLine("Текст не может быть пустым. Попробуйте ещё раз.");
                    continue;
                }

                return text;
            }
        }

        /// <summary>
        /// Добавить новую запись в записную книжку
        /// </summary>
        private static void AddNotebookItem()
        {
            Console.WriteLine("Добавление новой записи.");
            NotebookItem item = new NotebookItem();

            Console.Write("Введите ФИО:");
            var fullName = Console.ReadLine();
            Console.Write("Введите дату рождения в формате (yyyy-MM-dd, например, 1995-02-24):");
            var birthDate = GetDateTimeFromUserInput();
            Console.Write("Введите номер телефона:");
            var phone = Console.ReadLine();
            Console.Write("Введите примечание:");
            var note = Console.ReadLine();

            item.FullName = fullName;
            item.BirthDate = birthDate;
            item.Phone = phone;
            item.Note = note;

            notebook.Add(item);

            Console.WriteLine("Запись успешно добавлена!");

            dataManager.SaveNotebookItems(notebook);

            ShowMenu();
        }

        /// <summary>
        /// Удалить запись из записной книжки
        /// </summary>
        private static void RemoveNotebookItem()
        {
            if (notebook.Count == 0)
            {
                Console.WriteLine("Нет записей для удаления. Сперва добавьте хотя бы одну запись!");
                ShowMenu();
                return;
            }

            Console.WriteLine("Введите номер записи для удаления.");
            Console.WriteLine("0) Отмена удаления");

            PrintData(notebook);

            int numberToRemove = GetIntFromUserInput(0, notebook.Count + 1);

            if (numberToRemove != 0)
            {
                notebook.RemoveAt(numberToRemove - 1);
                Console.WriteLine("Запись успешно удалена!");
                dataManager.SaveNotebookItems(notebook);
            }

            ShowMenu();
        }


        /// <summary>
        /// Показать данные (и отсортировать, если выбрано такое действие)
        /// </summary>
        private static void ShowData()
        {
            if (notebook.Count == 0)
            {
                Console.WriteLine("Нет записей для показа. Сперва добавьте хотя бы одну запись!");
                ShowMenu();
                return;
            }

            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1) Показать данные как есть");
            Console.WriteLine("2) Показать данные отсортированные по ФИО");

            var number = GetIntFromUserInput(1, 2);

            if (number == 1)
            {
                PrintData(notebook);
            }
            else
            {
                var sortedList = notebook.OrderBy(x => x.FullName).ToList();
                PrintData(sortedList);
            }

            ShowMenu();
        }

        /// <summary>
        /// Вывести данные на консоль из переданного списка записей
        /// </summary>
        /// <param name="items">Список записей</param>
        private static void PrintData(List<NotebookItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {items[i].FullName}, {items[i].BirthDate:yyyy-MM-dd}, {items[i].Phone}, {items[i].Note}");
            }
        }

        /// <summary>
        /// Поиск записи в записной книжке
        /// </summary>
        private static void SearchNotebookItem()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1) Поиск по ФИО:");
            Console.WriteLine("2) Поиск по номеру телефона:");
            Console.WriteLine("3) Поиск по дате рождения:");
            Console.WriteLine("4) Поиск по примечанию:");

            var number = GetIntFromUserInput(1, 4);

            string searchText = string.Empty;

            if (number != 3)
            {
                Console.Write("Введите текст поиска:");
                searchText = GetNotNullStringFromUserInput();
            }

            List<NotebookItem> foundItems = new List<NotebookItem>();

            switch (number)
            {
                case 1:
                    foundItems = notebook.Where(x => x.FullName.ToUpper().Contains(searchText.ToUpper())).ToList();
                    break;
                case 2:
                    foundItems = notebook.Where(x => x.Phone.ToUpper().Contains(searchText.ToUpper())).ToList();
                    break;
                case 3:
                    Console.Write("Введите дату рождения в формате (yyyy-MM-dd, например, 1995-02-24):");
                    var birthDate = GetDateTimeFromUserInput();
                    foundItems = notebook.Where(x => x.BirthDate == birthDate).ToList();
                    break;
                case 4:
                    foundItems = notebook.Where(x => x.Note.ToUpper().Contains(searchText.ToUpper())).ToList();
                    break;
            }

            if (foundItems.Count == 0)
            {
                Console.WriteLine("Записей не найдено");
            }
            else
            {
                Console.WriteLine("Найденные записи:");
                PrintData(foundItems);
            }

            ShowMenu();
        } 


    }
}
