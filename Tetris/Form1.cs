using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Tetris
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
            holst.InitializeContexts();
        }
        Figure figure = new Figure();
        bool lever = true;
        bool lever_manual = false;
        private void holst_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Gl.glViewport(0, 0, holst.Width, holst.Height);
            Gl.glClearColor(0, 0, 0, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Start();
        }

        private void Start() //начало игры
        {
           figure.Read_record();
           figure.Create();
           figure.Next();
           timer.Start();
        }
        
        private void Help()
        {
            lever_manual = true;
            lever = false;
            timer.Stop();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            Gl.glRasterPos2f(-0.8f, 0.8f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "W - Rotate falling figure");

            Gl.glRasterPos2f(-0.8f, 0.6f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "A - Move falling figure left");

            Gl.glRasterPos2f(-0.8f, 0.4f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "D - Move falling figure right");

            Gl.glRasterPos2f(-0.8f, 0.2f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "S - Soft drop");

            Gl.glRasterPos2f(-0.8f, 0f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "Space - Hard drop");

            Gl.glRasterPos2f(-0.8f, -0.2f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "R - New game");

            holst.Invalidate();
        }
        private void Continue(object sender, EventArgs e)  //падение фигуры 
        {
            if (figure.speed != 1001)
            {
                figure.Down(false);
                Draw();
                timer.Interval = figure.speed;
            }
            else
            {
                lever = false;
                timer.Stop();
                figure.Write_record();
            }

        }
        private void Draw()  //отрисовка
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            for ( int i = 0; i < 20; i++)   //отрисовка поля
            {
                for (int j = 0; j < 10; j++)
                {
                    Switch_color(figure.table, i, j);
                    Square(j + 1, i);
                }
            }
            for (int i = 0; i < figure.newTermino.GetLength(0); i++)  //следующая фигура 
            {
                for (int j = 0; j < figure.newTermino.GetLength(1); j++)
                {
                    Switch_color(figure.newTermino, i, j);
                    Square(j + 14, i + 5);
                }
            }

            for (int i = 0; i < figure.table.GetLength(0); i++)     //белая рамка
            {
                Gl.glColor3f(1f, 1f, 1f);
                Square(0, i);
                Gl.glColor3f(1f, 1f, 1f);
                Square(11, i);
            }

            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            Gl.glRasterPos2f(0.45f, 0.9f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "Pause");

            Gl.glRasterPos2f(0.45f, 0.7f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "Manual");

            Gl.glRasterPos2f(0.5f, -0.2f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "LVL: " + figure.lvl.ToString());

            Gl.glRasterPos2f(0.45f, -0.4f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "Score: " + figure.score.ToString());

            Gl.glRasterPos2f(0.45f, -0.6f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "Record: " + figure.record.ToString());

            Gl.glRasterPos2f(0.45f, -0.9f);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_18, "New Game");
            holst.Invalidate();
        }

       private void Switch_color(int[,] termino, int i, int j) //выбор цвета
        {
            switch (termino[i, j])  
            {
                case 0:
                    Gl.glColor3f(0f, 0f, 0f);  //фон
                    break;
                case 1:
                    Gl.glColor3f(1f, 0.25f, 0.25f);
                    break;
                case 2:
                    Gl.glColor3f(0.36f, 0.8f, 0.79f);
                    break;
                case 3:
                    Gl.glColor3f(1f, 1f, 0.25f);
                    break;
                case 4:
                    Gl.glColor3f(0.42f, 0.24f, 0.8f);
                    break;
                case 5:
                    Gl.glColor3f(0.62f, 0.93f, 0f);
                    break;
                case 6:
                    Gl.glColor3f(0.76f, 0f, 0.53f);
                    break;
                case 7:
                    Gl.glColor3f(1f, 0.60f, 0f);
                    break;

            }
        }
        private void Square(int x, int y)   //отрисовка квадрата 
        {
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex2f(x * 0.1f - 1f,1f - y * 0.1f - 0.1f);
            Gl.glVertex2f(x * 0.1f + 0.1f - 1f, 1f - y * 0.1f - 0.1f);
            Gl.glVertex2f(x * 0.1f + 0.1f - 1f, 1f - y * 0.1f);
            Gl.glVertex2f(x * 0.1f - 1f, 1f - y * 0.1f);
            Gl.glEnd();

            Gl.glColor3f(0f, 0f, 0f);
            Gl.glBegin(Gl.GL_LINE_LOOP);  //черная рамка во круг квадрата 
            Gl.glVertex2f(x * 0.1f - 1f, 1f - y * 0.1f - 0.1f);
            Gl.glVertex2f(x * 0.1f + 0.1f - 1f, 1f - y * 0.1f - 0.1f);
            Gl.glVertex2f(x * 0.1f + 0.1f - 1f, 1f - y * 0.1f);
            Gl.glVertex2f(x * 0.1f - 1f, 1f - y * 0.1f);
            Gl.glEnd();
        }

        private void holst_KeyDown(object sender, KeyEventArgs e)
        {
            if (lever)
            {
                switch (e.KeyCode)
                {
                    case (Keys.A):   //сдвиг влево
                        figure.Left();
                        Draw();
                        break;
                    case (Keys.D):  //сдваиг вправо
                        figure.Right();
                        Draw();
                        break;
                    case (Keys.S):  //сдвинуть вниз
                        figure.Down(false);
                        Draw();
                        break;
                    case (Keys.W):  //повернуть
                        figure.Rotation();
                        Draw();
                        break;
                    case (Keys.Space):  //падение до конца
                        figure.Down(true);
                        Draw();
                        Continue(sender, e);
                        break;
                }
            }

            if (e.KeyCode == Keys.R && !lever_manual)
            {
                figure = new Figure();
                lever = true;
                Start();
            }
        }

        private void holst_MouseClick(object sender, MouseEventArgs e)  //новая игра
        {
            if (lever_manual)
            {
                lever_manual = false;
                lever = true;
                timer.Start();
                return;
            }

           if (e.X > 350 && e.X < 500 && e.Y > 510 && e.Y < 600)
            {
                figure = new Figure();
                lever = true;
                Start();
            }

           if (e.X > 375 && e.X < 450 && e.Y > 10 && e.Y < 50 )
            {
                if (lever)
                {
                    lever = false;
                    timer.Stop();
                }
                else
                {
                    if(figure.speed != 1001)
                    {
                        lever = true;
                        timer.Start();
                    }
                }
            }

           if (e.X > 375 && e.X < 450 && e.Y > 60 && e.Y < 100)
            {
                Help();
            }
        }
    }
}