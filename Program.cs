using System.Globalization;

namespace Sorter
{
    //Класс с данными заказа
    class InputData
    {
        public int Id { get; set; }
        public double Weight { get; set; }

        public string? District { get; set; }

        public DateTime dateTime { get; set; }

    
    }

    
    //Класс с вводимыми пользователем данными
    class Program {
       static string? _deliveryLog { get; set; }
       static string? _deliveryOrder { get; set; }
       static string? _cityDistrict { get; set; }
       static DateTime _firstDeliveryDateTime { get; set; }
       
        static void Main(string[] args) {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture; //Разделитель в типе дабл - точка

            if (args.Length > 1) { 
                Console.WriteLine($"Too much arguments"); 
                Environment.Exit(-1); 
            }

            string[] arr = args[0].Split(",");
            if (ConsoleInputValidator(arr))//валидация ввода с комадной строки
            {
                Reader(@"C:\Users\gkras\source\repos\DeliverySorter\input.txt");//запуск ридера для чтения данных из файла с заказами

                Console.WriteLine($"Check the result by path {_deliveryOrder}");
            } else
            {
                Environment.Exit(-1);//в случае ошибки - выход
            }

        }

        public static bool ConsoleInputValidator(string[] args)//валидация ввода с комадной строки
        {
            DateTime curTime = DateTime.Now;
            //начинается чтение параметров
     
            if (args.Length != 4)//нужно 4 аргумента
            {
                Console.WriteLine("Error: Not 4 parameters!");
                return false;
            }

            try //попытка парсинга аргументов
            {
                _cityDistrict = args[0];
                _firstDeliveryDateTime = Convert.ToDateTime(args[1]);
                _deliveryLog = args[2];
                _deliveryOrder = args[3];

                File.WriteAllText(_deliveryLog, string.Empty);//предварительная очистка файла логов
                Logger(curTime, $"Info: received new data {args[0]} {args[1]} {args[2]} {args[3]}");//в случае успеха пишем лог
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return false; 
            }
            curTime = DateTime.Now;
            //Логгируем, что параметры успешно прочитаны
            Logger(curTime, $"Info: All parameters are read");
            return true;
        }

        public static void Reader(string filePath)
        {
            DateTime curTime = DateTime.Now;
            Logger(curTime, "Info: File is reading...");//начат процесс чтения файла

            List<InputData> list = new List<InputData>();

            foreach (string line in File.ReadLines(filePath))
            {
                if (FileDataValidator(line))//проверка файла
                {
                    string[] words = line.Split(", ");//разбиваем файл и пишем его в список

                    list.Add(new InputData
                    {
                        Id = Convert.ToInt32(words[0]),
                        Weight = Convert.ToDouble(words[1]),
                        District = words[2],
                        dateTime = Convert.ToDateTime(words[3])
                    });
                } 
            }
            File.WriteAllText(_deliveryOrder, string.Empty);//очищаем файл с результатом
            Writer(list);//пишем в него
        }


        public static bool FileDataValidator(string line)//проверка файла
        {

            DateTime curTime = DateTime.Now;
            string[] words = new string[4];
            words = line.Split(", ");

            if (words.Length != 4)//число параметров строки
            {
                curTime = DateTime.Now;
                Logger(curTime, "Error: Not enough parameters" + " in " + line);
                return false;
            }

            int id;
            
            DateTime dateTime;

            if (!int.TryParse(words[0], out id))//парсинг айдишника
            {
                curTime = DateTime.Now;
                Logger(curTime, "Error: Invalid ID format - " + line);
                return false;
            }

            double weight;

            if (!double.TryParse(words[1], out weight))//парсинг веса
            {
                curTime = DateTime.Now;
                Logger(curTime, "Error: Invalid Weight format - " + line);
                return false;
            }

            string district = words[2];

            if (!DateTime.TryParse(words[3], out dateTime))//парсинг времени
            {
                curTime = DateTime.Now;
                Logger(curTime, "Error: Invalid Date format - " + line);
                return false;
            }
            curTime = DateTime.Now;
            Logger(curTime, "Info: Formatting is OK");//если все ок, то пишем об этом в лог =)
            return true;
        }

  


        public static void Writer(List<InputData> list)
        {
            DateTime curTime = DateTime.Now;
            Logger(curTime, "Info: Filtrating and writing...");
            DateTime deltaTime = _firstDeliveryDateTime.AddMinutes(30);//граница в полчаса
            var ordered = list.Where(x => x.District == _cityDistrict && (x.dateTime >= _firstDeliveryDateTime && x.dateTime <= deltaTime)).ToList();
            //Линкью для определения необходимых значений
            if (!File.Exists(_deliveryOrder))//создаем файл, если его нет
            {
                using (File.Create(_deliveryOrder)) { }
            }
            
            using (var sw = new StreamWriter(_deliveryOrder, true))
            {
    
                for (int i = 0; i < ordered.Count; i++)
                {
                    sw.WriteLine($"Id: {ordered[i].Id} Date : {ordered[i].dateTime} District: {ordered[i].District} Weight {ordered[i].Weight}");//пишем в него
                }
            }
            curTime = DateTime.Now;
            Logger(curTime, "Info: All done!");
        }

        public static void Logger(DateTime curTime, string str)
        {
            if (!File.Exists(_deliveryLog))
            {
                using (File.Create(_deliveryLog)) { }//создаем файл, если его нет
            } 

            using (var sw = new StreamWriter(_deliveryLog, true))//логируем
            {
               
                sw.WriteLine($"|{curTime}| {str}");
            }
        }
    }

}