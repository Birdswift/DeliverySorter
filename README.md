Инструкция

1)Клонировать данный репозиторий к себе на компьютер

2)Открыть решение в командной строке

3)Собрать решение командой dotnet build

4)После сборки экзешник будет лежать на пути cd bin\Debug\net6.0\

5)Запустить экзешник DeliverySorter.exe "<Район>,<Дата и время>,<Путь для логов>,<Путь для результата работы программы>"
Пример - 

"District A,2023-10-01 10:00:00,C:\Users\gkras\source\repos\DeliverySorter\bin\Debug\net6.0\logs.txt,C:\Users\gkras\source\repos\DeliverySorter\bin\Debug\net6.0\res.txt

6)Программа выведет местонахождение файла с итогом работы программы

7)Тестовый вариант с файлом ввода заказов прикреплен(input.txt), как и остальные файлы(log.txt & res.txt), демонстрирующие работу приложения.

