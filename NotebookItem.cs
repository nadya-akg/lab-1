using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookConsole
{
    /// <summary>
    /// Класс хранящий в себе информацию о одной записе в записной книжке
    /// </summary>
    public class NotebookItem
    {   
        /// <summary>
        /// ФИО
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Запись
        /// </summary>
        public string Note { get; set; }
    }
}
