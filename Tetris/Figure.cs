using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace Tetris
{
    class Figure
    {
        Random rand = new Random((int)DateTime.Now.Millisecond);
        public int[,] termino;  //падающая фигура
        public int[,] newTermino;   //следующая фигура 
        const int height = 20, width = 10;  //размеры игрового поля
        int x = 3, y = 0;   //начальные координаты фигуры
        public int[,] table = new int[height, width];  //игровое поле 
        public int score = 0;   //кол-во очков
        public int lvl = 1;    //уровень игры 
        public int line = 0;    //кол-во убраных рядов
        public int speed = 600;  //промежуток времени
        public int record = 0;  //рекорд очков
        public Figure()
        {

        }

        public void Create() //создание новой фигуры 
        {
            switch (rand.Next(7))
            {
                case 0:
                    newTermino = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
                    break;
                case 1:
                    newTermino = new int[1, 4] { { 2, 2, 2, 2 } };
                    break;
                case 2:
                    newTermino = new int[2, 3] { { 0, 3, 3 }, { 3, 3, 0 } };
                    break;
                case 3:
                    newTermino = new int[2, 3] { { 0, 4, 0 }, { 4, 4, 4 } };
                    break;
                case 4:
                    newTermino = new int[2, 2] { { 5, 5 }, { 5, 5 } };
                    break;
                case 5:
                    newTermino = new int[2, 3] { { 6, 0, 0 }, { 6, 6, 6 } };
                    break;
                case 6:
                    newTermino = new int[2, 3] { { 7, 7, 0 }, { 0, 7, 7 } };
                    break;
            }
        }
        public void Next()  // переход к следующей фигуре
        {
            Clear_table();
            Level();
            Timer();
            for (int i = 0; i < width; i++)  //проверка, есть ли место для новой фигуры 
            {
                if (table[0, i] != 0 || table[1, i] != 0)
                {
                    speed = 1001;
                    break;
                }
            }
            
            x = 3; y = 0;       
            termino = newTermino;
            Create();   //создать новую фигуру 
        }

        public void Down(bool cycle)  //падение фигуры
        {
            if ((y + termino.GetLength(0)) == height) //если упала до конца
            {
                Next(); //переход к следующей фигуре 
                return;
            }
           
            for (int i = 0; i < termino.GetLength(0); i++)  //обнуляем предыдущие координаты
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    if (termino[i, j] != 0)
                        table[y + i, j + x] = 0;
                }
            }

            int count = 0;
            bool flag;
            if (cycle) //если падение до конца 
            {
                while (true)
                {
                    flag = Fall();
                    if ((y + termino.GetLength(0)) == height - 1)
                    {
                        break;
                    }
                    if (flag)
                    {
                        if (count != 0)
                        {
                            y++;
                        }
                        count++;
                    }
                    else
                    {
                        break;
                    }

                }
            }
            else
            {
                flag = Fall();   //падение на 1 клетку 
            }

            if (flag)
            {
                for (int i = 0; i < termino.GetLength(0); i++)  //назначение новых координат 
                {
                    for (int j = 0; j < termino.GetLength(1); j++)
                    {
                        if (termino[i, j] != 0)
                            table[y + i + 1, j + x] = termino[i, j];
                    }
                }
                y++;
            }
            else
            {
                for (int i = 0; i < termino.GetLength(0); i++)  //назначение старых координат 
                {
                    for (int j = 0; j < termino.GetLength(1); j++)
                    {
                        if (termino[i, j] != 0)
                            table[y + i, j + x] = termino[i, j];
                    }
                }
                Next();      //переход к следующей фигуре
            }
        }

        bool Fall()  //падение на 1 клетку
        {
            for (int i = 0; i < termino.GetLength(0); i++)    //проверяем, есть ли место для падениия
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    if (table[y + i + 1, x + j] != 0 && termino[i, j] != 0)
                    {
                        return false;   
                    }
                }
            }
            return true;    
        }
        public void Left()  //сдвиг фигуры влево
        {
            if (x == 0)   //проверка, находится ли фигура у границы
            {
                return;
            }

            for (int i = 0; i < termino.GetLength(0); i++)   //проверка, есть ли слева свободные клетки
            {
                if (table[y + i, x] != 0 && termino[i, 0] == 0)
                {
                    return;
                }
                if (table[y + i, x - 1] != 0 && termino[i, 0] != 0)
                {
                    return;
                }
            }

            for (int i = 0; i < termino.GetLength(0); i++)  //обнуляем предыдущие координаты
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    if (termino[i, j] != 0)
                        table[y + i, j + x] = 0;
                }
            }
            for (int i = 0; i < termino.GetLength(0); i++)  //назначение новых координат 
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    if (termino[i, j] != 0)
                        table[y + i, j + x - 1] = termino[i, j];
                }
            }
            x--;

        }
        public void Right()  //сдвиг фигуры вправо
        {
            if (x + termino.GetLength(1) == width)  //проверка, находится ли фигура у правой границы
            {
                return;
            }

            for (int i = 0; i < termino.GetLength(0); i++)  //проверка, есть ли справа свободные клетки
            {
                if (table[y + i, x + termino.GetLength(1) - 1] != 0 && termino[i, termino.GetLength(1) - 1] == 0)
                {
                    return;
                }
                if (table[y + i, x + termino.GetLength(1)] != 0 && termino[i, termino.GetLength(1) - 1] != 0)
                {
                    return;
                }
            }

            for (int i = 0; i < termino.GetLength(0); i++)  //обнуляем предыдущие координаты
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    if (termino[i, j] != 0)
                        table[y + i, j + x] = 0;
                }
            }
            for (int i = 0; i < termino.GetLength(0); i++)  //назначение новых координат 
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    if (termino[i, j] != 0)
                        table[y + i, j + x + 1] = termino[i, j];
                }
            }
            x++;
        }

        public void Rotation()
        {
            int[,] mas = new int[termino.GetLength(1), termino.GetLength(0)];
            for (int i = 0; i < termino.GetLength(0); i++)      //делаем поворот
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    mas[j, i] = termino[termino.GetLength(0) - i - 1, j];
                }
            }


            if (y + mas.GetLength(0) > height || x + mas.GetLength(1) > width) //проверка на выход за границы экрана
            {
                return;
            }

            for (int i = 0; i < termino.GetLength(0); i++)  //обнуляем предыдущие координаты
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    if (termino[i, j] != 0)
                        table[y + i, j + x] = 0;
                }
            }

            bool flag = true;
            for (int i = 0; i < mas.GetLength(0); i++)    //проверяем, есть ли место для перевёрнутой фигуры
            {
                for (int j = 0; j < mas.GetLength(1); j++)
                {
                    if (table[y + i, x + j] != 0 && mas[i, j] != 0)
                    {
                        flag = false;
                    }
                }
            }

            if (flag)            //если место есть, то перезначаем значения
                termino = mas;

            for (int i = 0; i < termino.GetLength(0); i++)  //назначение новых координат 
            {
                for (int j = 0; j < termino.GetLength(1); j++)
                {
                    if (termino[i, j] != 0)
                        table[y + i, j + x] = termino[i, j];
                }
            }
        }
        public void Clear_table() //ищем заполненные строки и удаляем их
        {
            int count = 0;
            int flag = 1;
            for (int i = 0; i < table.GetLength(0); i++)
            {
                int j = 0;
                while (table[i, j] != 0)   //перебираем все эллементы строки
                {
                    if (j + 1 == width)  //если все элементы != 0
                    {
                        Delete_line(i);
                        count++;
                        break;
                    }
                    j++;
                }
                if (flag == count)
                {
                    score += 100 * count * lvl;
                    flag++;
                }
            }
            line += count;
        }

        void Delete_line(int x)  //сдвигаем строки вниз
        {
            while (x > 1)
            {
                for (int i = 0; i < width; i++)
                {
                    table[x, i] = table[x - 1, i];
                }
                x--;
            }
        }

        void Level()  //уровень игры
        {
            lvl = line / 5 + 1;
        }
        void Timer()  //скорость игры
        {
            speed = 525 - 25 * lvl;
        }

        public void Read_record()  //чтение рекорда очков
        {
            try
            {
                using (StreamReader stream = new StreamReader(@"Record.txt"))
                {
                    record = Convert.ToInt32(stream.ReadLine());
                }
            }
            catch
            {

            }
        }
        public void Write_record()  //запись рекорда
        {
            if (score > record)
            using (StreamWriter stream = new StreamWriter(@"Record.txt", false)) 
            {
                stream.WriteLine(score);
            }
        }
        
    } 

}