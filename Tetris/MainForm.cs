using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Tetris
{
    //private enum FigTypes : byte { I, Z, S, L, J, T, D };
    //private enum FigState : byte { S0, S90, S180, S270 };
    //private enum SqrColors : byte { White, Blue, Red, Green, Brown, Purple };
    
    public struct Records
    {
        public string Gamer;
        public long Score;
        public Records(string GamerName, long Scores)
        {
            Gamer = GamerName;
            Score = Scores;
        }
    }

    public partial class MainForm : Form
    {
        private static byte FieldWidth = 12;
        private static byte FieldHeight = 20;

        private int MaxRecords = 15;
        private int LineToNextLevel = 20;


        private int Speed = 500;    // м.с. ожидания 
        private enum SqrColors: byte {White, Blue, Red, Green, Brown, Purple};
        private struct Position         // стуктура позиция в идексах поля
        {
            public sbyte X;
            public sbyte Y;
            public Position(sbyte x, sbyte y)
            {
                X = x;
                Y = y;
            }
        }
        //Position MainPos = new Position((byte)(FieldWidth / 2), 0);     // позиция центра фигуры

        private enum FigTypes : byte {I, Z, S, L, J, T, D};
        private enum FigState : byte { S0, S90, S180, S270 };

        private sbyte[,] Fig_I_0 = new sbyte[3, 2] { { 0, -2 }, { 0, -1 }, { 0, 1 } };
        private sbyte[,] Fig_I_90 = new sbyte[3, 2] { { -1, 0 }, { 1, 0 }, { 2, 0 } };
        private sbyte[,] Fig_I_180 = new sbyte[3, 2] { { 0, -1 }, { 0, 1 }, { 0, 2 } };
        private sbyte[,] Fig_I_270 = new sbyte[3, 2] { { -2, 0 }, { -1, 0 }, { 1, 0 } };

        private sbyte[,] Fig_J_0 = new sbyte[3, 2] { { -1, 0 }, { 1, 0 }, { 1, 1 } };
        private sbyte[,] Fig_J_90 = new sbyte[3, 2] { { 0, -1 }, { 0, 1 }, { -1, 1 } };
        private sbyte[,] Fig_J_180 = new sbyte[3, 2] { { -1, -1 }, { -1, 0 }, { 1, 0 } };
        private sbyte[,] Fig_J_270 = new sbyte[3, 2] { { 1, -1 }, { 0, -1 }, { 0, 1 } };

        private sbyte[,] Fig_L_0 = new sbyte[3, 2] { { -1, 1 }, { -1, 0 }, { 1, 0 } };
        private sbyte[,] Fig_L_90 = new sbyte[3, 2] { { -1, -1 }, { 0, -1 }, { 0, 1 } };
        private sbyte[,] Fig_L_180 = new sbyte[3, 2] { { -1, 0 }, { 1, 0 }, { 1, -1 } };
        private sbyte[,] Fig_L_270 = new sbyte[3, 2] { { 0, -1 }, { 0, 1 }, { 1, 1 } };

        private sbyte[,] Fig_T_0 = new sbyte[3, 2] { { -1, 0 }, { 0, 1 }, { 1, 0 } };
        private sbyte[,] Fig_T_90 = new sbyte[3, 2] { { 0, -1 }, { -1, 0 }, { 0, 1 } };
        private sbyte[,] Fig_T_180 = new sbyte[3, 2] { { -1, 0 }, { 0, -1 }, { 1, 0 } };
        private sbyte[,] Fig_T_270 = new sbyte[3, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 } };

        private sbyte[,] Fig_S_0 = new sbyte[3, 2] { { -1, 0 }, { 0, -1 }, { 1, -1 } };
        private sbyte[,] Fig_S_90 = new sbyte[3, 2] { { -1, -1 }, { -1, 0 }, { 0, 1 } };

        private sbyte[,] Fig_Z_0 = new sbyte[3, 2] { { -1, -1 }, { 0, -1 }, { 1, 0 } };
        private sbyte[,] Fig_Z_90 = new sbyte[3, 2] { { -1, 1 }, { -1, 0 }, { 0, -1 } };
        
        private sbyte[,] Fig_D_0 = new sbyte[3, 2] { { -1, 0 }, { -1, -1 }, { 0, -1 } };


        private struct FigureStruct     // структура положения/состояния фигуры
        {
            public Position MainPos;
            public FigTypes Type;
            public SqrColors Color;
            public FigState State;
            public FigureStruct(Position CentrPos, FigTypes TypeOfFigure, SqrColors FigureColor, FigState FigureState)
            {
                //MainPos = new Position(CentrPos.X, CentrPos.Y);
                MainPos = CentrPos;
                Type = TypeOfFigure;
                Color = FigureColor;
                State = FigureState;
            }
        }
        private FigureStruct CurrentFigure;    // текущая фигура
        private FigureStruct NextFigure;    // следующая фигура

        private Random rnd;
        SqrColors[,] Field = new SqrColors[FieldWidth, FieldHeight];    // поле

        private enum Directs : byte { Left, Right };
        private Directs RotateDirect;

        private long Score = 0;
        private long Lines = 0;

        private bool IsRunned = false;
        private bool needActivate = false;

        private string RecordsFile = string.Empty;


        


        // сортировщик рекордов
        private class RecordComparer : IComparer<Records>
        {
            public int Compare(Records x, Records y)
            {
                if (x.Score > y.Score)
                    return -1;
                else if (x.Score < y.Score)
                    return 1;
                else
                {
                    // очки равны, сравниваем по именам
                    return x.Gamer.CompareTo(y.Gamer);
                }
            }
        }




        // ----------------------------------------------------------------
        public MainForm()
        {
            InitializeComponent();
            
            for (int x = 0; x < FieldWidth; x++)
                for (int y = 0; y < FieldHeight; y++)
                    Field[x, y] = SqrColors.White;
            
            RotateDirect = Directs.Right;

            RecordsFile = System.IO.Directory.GetCurrentDirectory() + "\\" + "recordes.dat";
        }


        // нажатие кнопки
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:    // ----- Вправо
                    StepRight();
                    break;

                case Keys.Left:     // ----- Влево
                    StepLeft();
                    break;

                case Keys.Up:       // ----- развернуть
                    Rotation();
                    break;

                case Keys.Down:     // ----- Вниз
                    StepDown();
                    break;

                default:
                    break;
            }
            
            return base.ProcessDialogKey(keyData);
        }


        // старт
        private void Start()
        {
            // очистить поле
            for (int x = 0; x < FieldWidth; x++)
                for (int y = 0; y < FieldHeight; y++)
                    Field[x, y] = SqrColors.White;
            pnlField.Refresh();

            Score = 0;
            lblScore.Text = "Очки: " + Score.ToString();

            Lines = 0;
            lblLineCount.Text = "Линий: " + Lines.ToString();
            
            // создать генератор
            rnd = new Random();
            
            // запустить прорисовку бокса
            pnlField.Paint += new PaintEventHandler(pnlField_Paint);

            // создать следующую фигуру
            MakeNextFigure();

            // копировать в текущую
            CurrentFigure = NextFigure;

            // прорисовать текущую
            PaintCurrentFigure(false);

            // создать следующую фигуру
            MakeNextFigure();
            
            // прорисовать следующую
            PaintNextFigure();

            // запустить таймер
            MoveDownTimer.Interval = Speed;
            MoveDownTimer.Start();

            IsRunned = true;
            lblStart.Text = "Pause";
        }



        // создать следующую фигуру
        private void MakeNextFigure()
        {
            /*
            int chislo = rnd.Next();
            FigTypes ft = (FigTypes)(chislo % 7);   // тип фигуры
            FigState st = (FigState)(chislo % 4);   // состояние
             */

            FigTypes ft;
            FigState st;
            int chislo = rnd.Next(21);
            switch (chislo)
            {
                case 0:
                    ft = FigTypes.I;
                    st = FigState.S0;
                    break;
                case 1:
                    ft = FigTypes.I;
                    st = FigState.S90;
                    break;
                case 2:
                    ft = FigTypes.I;
                    st = FigState.S180;
                    break;
                case 3:
                    ft = FigTypes.I;
                    st = FigState.S270;
                    break;

                case 4:
                    ft = FigTypes.J;
                    st = FigState.S0;
                    break;
                case 5:
                    ft = FigTypes.J;
                    st = FigState.S90;
                    break;
                case 6:
                    ft = FigTypes.J;
                    st = FigState.S180;
                    break;
                case 7:
                    ft = FigTypes.J;
                    st = FigState.S270;
                    break;
                
                case 8:
                    ft = FigTypes.L;
                    st = FigState.S0;
                    break;
                case 9:
                    ft = FigTypes.L;
                    st = FigState.S90;
                    break;
                case 10:
                    ft = FigTypes.L;
                    st = FigState.S180;
                    break;
                case 11:
                    ft = FigTypes.L;
                    st = FigState.S270;
                    break;

                case 12:
                    ft = FigTypes.T;
                    st = FigState.S0;
                    break;
                case 13:
                    ft = FigTypes.T;
                    st = FigState.S90;
                    break;
                case 14:
                    ft = FigTypes.T;
                    st = FigState.S180;
                    break;
                case 15:
                    ft = FigTypes.T;
                    st = FigState.S270;
                    break;

                case 16:
                    ft = FigTypes.S;
                    st = FigState.S0;
                    break;
                case 17:
                    ft = FigTypes.S;
                    st = FigState.S90;
                    break;
                
                case 18:
                    ft = FigTypes.Z;
                    st = FigState.S0;
                    break;
                case 19:
                    ft = FigTypes.Z;
                    st = FigState.S90;
                    break;

                default:
                    ft = FigTypes.D;
                    st = FigState.S0;
                    break;
            }

            Position pos = GetCentrPos(ft, st);     // положение центра
            switch (ft)
            {
                case FigTypes.I:
                    NextFigure = new FigureStruct(pos, ft, SqrColors.Purple, st);
                    break;

                case FigTypes.J:
                    NextFigure = new FigureStruct(pos, ft, SqrColors.Blue, st);
                    break;
                case FigTypes.L:
                    NextFigure = new FigureStruct(pos, ft, SqrColors.Blue, st);
                    break;

                case FigTypes.S:
                    NextFigure = new FigureStruct(pos, ft, SqrColors.Brown, st);
                    break;
                case FigTypes.Z:
                    NextFigure = new FigureStruct(pos, ft, SqrColors.Brown, st);
                    break;

                case FigTypes.T:
                    NextFigure = new FigureStruct(pos, ft, SqrColors.Green, st);
                    break;

                default:    // квадрат
                    NextFigure = new FigureStruct(pos, ft, SqrColors.Red, st);
                    break;
            }

            //ShowFig();
        }

        
        
        // стартовая позиция для данной фигуры
        private Position GetCentrPos(FigTypes ft, FigState fst)
        {
            Position rez = new Position((sbyte)(FieldWidth / 2), 0);

            switch (ft)
            {
                case FigTypes.I:
                    switch (fst)
                    {
                        case FigState.S0:
                            //rez.X = (byte)(FieldWidth / 2);
                            rez.Y = 2;
                            break;
                        case FigState.S90:
                            //rez.X = (byte)(FieldWidth / 2 - 1);
                            rez.Y = 0;
                            break;
                        case FigState.S180:
                            //rez.X = (byte)(FieldWidth / 2);
                            rez.Y = 1;
                            break;
                        default:
                            //rez.X = (byte)(FieldWidth / 2);
                            rez.Y = 0;
                            break;
                    }
                    break;
                case FigTypes.J:
                    if (fst == FigState.S0)
                        rez.Y = 0;
                    else
                        rez.Y = 1;
                    break;

                case FigTypes.L:
                    if (fst == FigState.S0)
                        rez.Y = 0;
                    else
                        rez.Y = 1;
                    break;
                case FigTypes.S:
                    rez.Y = 1;
                    break;
                case FigTypes.Z:
                    rez.Y = 1;
                    break;
                case FigTypes.T:
                    if (fst == FigState.S0)
                        rez.Y = 0;
                    else
                        rez.Y = 1;
                    break;
                default:    // квадрат
                    rez.Y = 1;
                    break;
            }
            return rez;
        }

        

        // Все точки фигуры относительно её центра
        private Position[] GetFigurePoints()
        {
            Position[] Pos = new Position[3];
            switch (CurrentFigure.Type)
            {
                case FigTypes.I:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_I_0[i, 0], Fig_I_0[i, 1]);
                            break;
                        case FigState.S90:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_I_90[i, 0], Fig_I_90[i, 1]);
                            break;
                        case FigState.S180:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_I_180[i, 0], Fig_I_180[i, 1]);
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_I_270[i, 0], Fig_I_270[i, 1]);
                            break;
                    }
                    break;
                
                case FigTypes.J:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_J_0[i, 0], Fig_J_0[i, 1]);
                            break;
                        case FigState.S90:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_J_90[i, 0], Fig_J_90[i, 1]);
                            break;
                        case FigState.S180:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_J_180[i, 0], Fig_J_180[i, 1]);
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_J_270[i, 0], Fig_J_270[i, 1]);
                            break;
                    }
                    break;

                case FigTypes.L:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_L_0[i, 0], Fig_L_0[i, 1]);
                            break;
                        case FigState.S90:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_L_90[i, 0], Fig_L_90[i, 1]);
                            break;
                        case FigState.S180:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_L_180[i, 0], Fig_L_180[i, 1]);
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_L_270[i, 0], Fig_L_270[i, 1]);
                            break;
                    }
                    break;

                case FigTypes.T:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_T_0[i, 0], Fig_T_0[i, 1]);
                            break;
                        case FigState.S90:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_T_90[i, 0], Fig_T_90[i, 1]);
                            break;
                        case FigState.S180:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_T_180[i, 0], Fig_T_180[i, 1]);
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_T_270[i, 0], Fig_T_270[i, 1]);
                            break;
                    }
                    break;

                case FigTypes.S:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_S_0[i, 0], Fig_S_0[i, 1]);
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_S_90[i, 0], Fig_S_90[i, 1]);
                            break;
                    }
                    break;

                case FigTypes.Z:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_Z_0[i, 0], Fig_Z_0[i, 1]);
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                Pos[i] = new Position(Fig_Z_90[i, 0], Fig_Z_90[i, 1]);
                            break;
                    }
                    break;

                default:
                    for (int i = 0; i < 3; i++)
                        Pos[i] = new Position(Fig_D_0[i, 0], Fig_D_0[i, 1]);
                    break;
            }

            return Pos;
        }



        // разворот
        private void Rotation()
        {
            if (CurrentFigure.Type == FigTypes.D)
                return;
            
            // точки фигуры
            Position[] AllPoints = GetFigurePoints();

            // точки после разворота
            Position[] PtAfterRotate = GetFigurePointsAfterRotate();

            // не выходят ли точки за границу поля
            for (int i = 0; i < PtAfterRotate.Length; i++)
            {
                int X = CurrentFigure.MainPos.X + PtAfterRotate[i].X;
                int Y = CurrentFigure.MainPos.Y + PtAfterRotate[i].Y;

                if (Y > FieldHeight-1)  // выйдем за низ поля
                    return;

                // подвинуть бы
                if (X < 0 || Y < 0 || X > FieldWidth-1)
                    return;

            }





            // точки которые должны быть свободны после разворота
            // т.е. PtAfterRotate без AllPoints
            Position[] OnlyNewPoints = RemoveOldPoints(AllPoints, PtAfterRotate);

            // возможен ли разворот
            for (int i = 0; i < OnlyNewPoints.Length; i++)
            {
                // для каждой новой точки
                // если точка занята
                if (Field[CurrentFigure.MainPos.X + OnlyNewPoints[i].X, CurrentFigure.MainPos.Y + OnlyNewPoints[i].Y] != SqrColors.White)
                    return;
            }


            // собственно разворот
            // стереть
            PaintCurrentFigure(true);

            // повернуть
            if (CurrentFigure.Type == FigTypes.I ||
                     CurrentFigure.Type == FigTypes.J ||
                     CurrentFigure.Type == FigTypes.L ||
                     CurrentFigure.Type == FigTypes.T)
            {
                switch (RotateDirect)
                {
                    case Directs.Right:
                        if (CurrentFigure.State == FigState.S270)
                            CurrentFigure.State = FigState.S0;
                        else
                            CurrentFigure.State++;
                        break;
                    default:
                        if (CurrentFigure.State == FigState.S0)
                            CurrentFigure.State = FigState.S270;
                        else
                            CurrentFigure.State--;
                        break;
                }
            }
            else
            {

                if (CurrentFigure.State == FigState.S0)
                    CurrentFigure.State = FigState.S90;
                else
                    CurrentFigure.State = FigState.S0;
            }


            // нарисовать
            PaintCurrentFigure(false);
        }



        // исключить точки, которые фигура уже занимет
        private Position[] RemoveOldPoints(Position[] OldPt, Position[] NewPt)
        {
            List<Position> PosList = new List<Position>();
            // перебираем все точки новой фигуры
            for (int i = 0; i < NewPt.Length; i++)
            {
                bool Add = true;
                // сравниваем каждую точку новой с всеми точкам старой
                for (int j = 0; j < OldPt.Length; j++)
                {
                    // если совпало
                    if (NewPt[i].X == OldPt[j].X && NewPt[i].Y == OldPt[j].Y)
                    {
                        // то не добавляем эту точку
                        Add = false;
                        break;
                    }
                }

                // если совпадения не было, добавляем эту точку
                if (Add == true)
                    PosList.Add(NewPt[i]);
            }

            Position[] Rez = new Position[PosList.Count];
            for (int i = 0; i < Rez.Length; i++)
                Rez[i] = PosList[i];

            return Rez;
        }



        // получить точки фигуры после разворота
        private Position[] GetFigurePointsAfterRotate()
        {
            Position[] rez;
            // текущее состояние фигуры
            FigState FSNow = CurrentFigure.State;
    
            // поворачиваем
            if (CurrentFigure.Type == FigTypes.I ||
                     CurrentFigure.Type == FigTypes.J ||
                     CurrentFigure.Type == FigTypes.L ||
                     CurrentFigure.Type == FigTypes.T)
            {
                if (RotateDirect == Directs.Right)
                {
                    if (CurrentFigure.State == FigState.S270)
                        CurrentFigure.State = FigState.S0;
                    else
                        CurrentFigure.State++;
                }
                else
                {
                    if (CurrentFigure.State == FigState.S0)
                        CurrentFigure.State = FigState.S270;
                    else
                        CurrentFigure.State--;

                }
            }
            else
            {
                if (CurrentFigure.State == FigState.S0)
                    CurrentFigure.State = FigState.S90;
                else
                    CurrentFigure.State = FigState.S0;
            }

            // получаем точки новой фигуры
            rez = GetFigurePoints();
            
            // возвращаем начальное состояние фигуры
            CurrentFigure.State = FSNow;
            
            return rez;
        }



        // шаг вниз
        private void StepDown()
        {
            // есть ли куда опускаться
            if (MaybeMoveDown() == true)
            {
                // опустить
                OneStepDown();

                if (needActivate == true)
                {
                    needActivate = false;
                    this.Activate();
                }
            }
            else
            {
                if (IsOver() == false)
                {
                    // остановить таймер
                    MoveDownTimer.Stop();
                    
                    // стереть полные строки
                    ClearFullLines();
                    MoveDownTimer.Start();

                    // копировать новую в текущую
                    CurrentFigure = NextFigure;

                    // прорисовать
                    PaintCurrentFigure(false);
                    
                    // создать новую фигуру
                    MakeNextFigure();

                    // прорисовать
                    PaintNextFigure();
                }
                else
                {
                    GameOver();
                }
            }

            // перерисовать поле
            //pnlField.Refresh();
        }

        
        // Конец?
        private bool IsOver()
        {
            for (int x = 0; x < FieldWidth; x++)
                if (Field[x, 0] != SqrColors.White)
                    return true;

            return false;
        }


        // Конец игры
        private void GameOver()
        {
            MoveDownTimer.Stop();
            IsRunned = false;
            lblStart.Text = "Start";
            pnlField.Paint -= pnlField_Paint;

            // список рекордов
            List<Records> RcList;

            if (File.Exists(RecordsFile) == false)
            {
                try
                {
                    File.Create(RecordsFile);
                }
                catch
                {
                    return;
                }
                RcList = new List<Records>();
            }
            else
            {
                FileStream FS;
                try
                {
                    FS = new FileStream(RecordsFile, FileMode.Open, FileAccess.Read);
                }
                catch
                {
                    return;
                }

                if (FS.Length > 0)
                    RcList = ReadFileRecords(FS);
                else
                    RcList = new List<Records>();

                FS.Close();
            }


            // сортировать
            RecordComparer RC = new RecordComparer();
            RcList.Sort(RC);

            // если набрали больше меньшего или рекордсменов больше 10
            int count = RcList.Count;
            if (count < MaxRecords || Score >= RcList[count - 1].Score)
            {
                YourName newForm = new YourName();
                newForm.ShowDialog();
                if (newForm.DialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    string GamerName = newForm.GamerName;
                    RcList.Add(new Records(GamerName, Score));
                    RcList.Sort(RC);

                    //записать результаты в файл
                    WriteRecordsToFile(RcList);

                    // показать таблицу
                    RecordsTable RcFr = new RecordsTable(MaxRecords, RcList);
                    RcFr.ShowDialog();

                }
                newForm.Dispose();
            }
        }



        // читать записи рекордов из потока
        private List<Records> ReadFileRecords(FileStream FS)
        {
            List<Records> RcList = new List<Records>();
            string Nm;
            long Sc;

            byte Ln;
            byte[] buf;
            byte[] bufScore = new byte[8];

            bool eof = false;
            while (eof == false)
            {
                // если не вышли за пределы файла
                if (FS.Position < FS.Length - 1)
                {
                    // кол-во сиволов следующего геймера
                    Ln = (byte)FS.ReadByte();
                    // если указанное кол-во символов + 8бит очков не выходят за пределы файла
                    if (Ln * 2 + 8 - 1 + FS.Position < FS.Length)
                    {
                        // если длинна не нулевая
                        if (Ln > 0)
                        {
                            // выделяем буффер
                            buf = new byte[Ln * 2];
                            // читаем имя
                            FS.Read(buf, 0, Ln * 2);
                            // буфер в строку
                            Nm = Encoding.Unicode.GetString(buf);
                        }
                        else
                            Nm = "";

                        // Очки
                        FS.Read(bufScore, 0, 8);
                        Sc = BitConverter.ToInt64(bufScore, 0);

                        // добавить рекорд в список
                        RcList.Add(new Records(Nm, Sc));
                    }
                    else
                        eof = true;
                }
                else
                    eof = true;
            }

            return RcList;
        }



        // запись рекордов в поток
        private void WriteRecordsToFile(List<Records> RcList)
        {
            FileStream FS;
            try
            {
                FS = new FileStream(RecordsFile, FileMode.Create, FileAccess.Write);
            }
            catch
            {
                return;
            }
            
            
            string Nm;
            long Sc;

            byte Ln;
            byte[] buf;

            for (int i = 0; i < RcList.Count && i < MaxRecords; i++)
            {
                Nm = RcList[i].Gamer;
                Ln = (byte)Nm.Length;
                Sc = RcList[i].Score;

                // Длинна
                FS.WriteByte(Ln);

                // имя
                buf = Encoding.Unicode.GetBytes(Nm);
                FS.Write(buf, 0, buf.Length);

                // очки
                buf = BitConverter.GetBytes(Sc);
                FS.Write(buf, 0, buf.Length);
            }
        }


        // если есть полные линии
        private void ClearFullLines()
        {
            List<int> fullLinesLst = new List<int>();
            for (int y = FieldHeight - 1; y >= 0; y--)
            {
                bool full = true;
                for (int x = 0; x < FieldWidth; x++)
                {
                    if (Field[x, y] == SqrColors.White)
                    {
                        full = false;
                        break;
                    }
                }
                if (full == true)
                    fullLinesLst.Add(y);
            }

            if (fullLinesLst.Count > 0)
            {
                // удалить линии
                DeleteLines(fullLinesLst);

                // очки
                switch (fullLinesLst.Count)
                {
                    case 1:
                        Score += 100;
                        break;
                    case 2:
                        Score += 300;
                        break;
                    case 3:
                        Score += 500;
                        break;
                    default:
                        Score += 1000;
                        break;
                }
                
                lblScore.Text = "Очки: " + Score.ToString();

                // Линий
                Lines += fullLinesLst.Count;
                lblLineCount.Text = "Линий: " + Lines.ToString();

                // скорость
                if (Lines % LineToNextLevel == 0)
                {
                    if (Speed > 50)
                        Speed -= 50;
                    else if (Speed > 10)
                        Speed -= 10;

                    MoveDownTimer.Interval = Speed;
                    string s = lblSpeed.Text.Substring(lblSpeed.Text.Length - 1);
                    int i = Convert.ToInt32(s);
                    i++;
                    lblSpeed.Text = "Скорость: " + i.ToString();
                }

            }
        }



        // удалить линии
        private void DeleteLines(List<int> Lines)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                // стераем
                for (int x = 0; x < FieldWidth; x++)
                    Field[x, Lines[i]] = SqrColors.White;

                // опускаем
                for (int y = Lines[i]; y > 0; y--)
                    for (int x = 0; x < FieldWidth; x++)
                        Field[x, y] = Field[x, y - 1];
                // 0-я строка
                for (int x = 0; x < FieldWidth; x++)
                    Field[x, 0] = SqrColors.White;

                // инекрементируем остальные индексы
                if (i < Lines.Count - 1)
                    for (int j = i + 1; j < Lines.Count; j++)
                        Lines[j]++;
            }
            
            // обновить поле
            pnlField.Refresh();
        }


        // Шаг влево
        private void StepLeft()
        {
            // есть ли куда сдвигаться
            if (MaybeMoveLeft() == true)
            {
                // сдвинуть
                OneStepLeft();
            }
        }



        // Шаг вправо
        private void StepRight()
        {
            // есть ли куда сдвигаться
            if (MaybeMoveRight() == true)
            {
                // сдвинуть
                OneStepRight();
            }
        }

        

        // один шаг вниз
        private void OneStepDown()
        {
            // стереть 
            PaintCurrentFigure(true);

            // инкрементировать 
            CurrentFigure.MainPos.Y++;

            // прорисовать
            PaintCurrentFigure(false);
        }



        // один шаг влево
        private void OneStepLeft()
        {
            // стереть
            PaintCurrentFigure(true);

            // декрементировать
            CurrentFigure.MainPos.X--;

            // прорисовать
            PaintCurrentFigure(false);
        }



        // один шаг вправо
        private void OneStepRight()
        {
            // стереть
            PaintCurrentFigure(true);

            // декрементировать
            CurrentFigure.MainPos.X++;

            // прорисовать
            PaintCurrentFigure(false);
        }



        // есть ли куда опускаться
        private bool MaybeMoveDown()
        {
            // уперлись ли в пол
            if (IsBottom() == true)
                return false;

            // уперлись ли в другую фигуру
            if (FreeSpaceToDown() == false)
                return false;

            return true;
        }

        

        // Можно ли двигаться влево
        private bool MaybeMoveLeft()
        {
            // уперлись ли в левый край
            if (IsLeftSide() == true)
                return false;

            // уперлись ли в другую фигуру
            if (FreeSpaceToLeft() == false)
                return false;

            return true;
        }

        

        // Можно ли двигаться вправо
        private bool MaybeMoveRight()
        {
            // уперлись ли в левый край
            if (IsRightSide() == true)
                return false;

            // уперлись ли в другую фигуру
            if (FreeSpaceToRight() == false)
                return false;


            return true;
        }



        // есть ли свободное место под фигурой
        private bool FreeSpaceToDown()
        {
            // Массив нижних точек фигуры
            Position[] LowerPoints = GetLowerPoints();  
            for (int i = 0; i < LowerPoints.Length; i++)
            {
                if (Field[CurrentFigure.MainPos.X + LowerPoints[i].X, CurrentFigure.MainPos.Y + LowerPoints[i].Y + 1] 
                    != SqrColors.White)
                    return false;
            }
            return true;
        }



        // есть ли свободное место слева от фигуры
        private bool FreeSpaceToLeft()
        {
            // смещения точек относительно центра
            Position[] AllPoints = GetFigurePoints();

            // Массив левых точек фигуры
            Position[] LeftPoints = GetLeftPoints(AllPoints);
            
            for (int i = 0; i < LeftPoints.Length; i++)
            {
                if (Field[CurrentFigure.MainPos.X + LeftPoints[i].X - 1, CurrentFigure.MainPos.Y + LeftPoints[i].Y]
                    != SqrColors.White)
                    return false;
            }
            return true;
        }



        // есть ли свободное место справа от фигуры
        private bool FreeSpaceToRight()
        {
            // смещения точек относительно центра
            Position[] AllPoints = GetFigurePoints();

            // Массив левых точек фигуры
            Position[] RightPoints = GetRightPoints(AllPoints);

            for (int i = 0; i < RightPoints.Length; i++)
            {
                if (Field[CurrentFigure.MainPos.X + RightPoints[i].X + 1, CurrentFigure.MainPos.Y + RightPoints[i].Y]
                    != SqrColors.White)
                    return false;
            }
            return true;
        }
   


        // нижние точки фигуры
        private Position[] GetLowerPoints()
        {
            Position[] LowerPoints;
            switch (CurrentFigure.Type)
            {
                case FigTypes.I:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            LowerPoints = new Position[1];
                            LowerPoints[0] = new Position(0, 1);
                            break;
                        case FigState.S90:
                            LowerPoints = new Position[4];
                            LowerPoints[0] = new Position(-1, 0);
                            LowerPoints[1] = new Position(0, 0);
                            LowerPoints[2] = new Position(1, 0);
                            LowerPoints[3] = new Position(2, 0);
                            break;
                        case FigState.S180:
                            LowerPoints = new Position[1];
                            LowerPoints[0] = new Position(0, 2);
                            break;
                        default:
                            LowerPoints = new Position[4];
                            LowerPoints[0] = new Position(-2, 0);
                            LowerPoints[1] = new Position(-1, 0);
                            LowerPoints[2] = new Position(0, 0);
                            LowerPoints[3] = new Position(1, 0);
                            break;
                    }
                    break;

                case FigTypes.J:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            LowerPoints = new Position[3];
                            LowerPoints[0] = new Position(-1, 0);
                            LowerPoints[1] = new Position(0, 0);
                            LowerPoints[2] = new Position(1, 1);
                            break;
                        case FigState.S90:
                            LowerPoints = new Position[2];
                            LowerPoints[0] = new Position(-1, 1);
                            LowerPoints[1] = new Position(0, 1);
                            break;
                        case FigState.S180:
                            LowerPoints = new Position[3];
                            LowerPoints[0] = new Position(-1, 0);
                            LowerPoints[1] = new Position(0, 0);
                            LowerPoints[2] = new Position(1, 0);
                            break;
                        default:
                            LowerPoints = new Position[2];
                            LowerPoints[0] = new Position(0, 1);
                            LowerPoints[1] = new Position(1, -1);
                            break;
                    }
                    break;

                case FigTypes.L:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            LowerPoints = new Position[3];
                            LowerPoints[0] = new Position(-1, 1);
                            LowerPoints[1] = new Position(0, 0);
                            LowerPoints[2] = new Position(1, 0);
                            break;
                        case FigState.S90:
                            LowerPoints = new Position[2];
                            LowerPoints[0] = new Position(-1, -1);
                            LowerPoints[1] = new Position(0, 1);
                            break;
                        case FigState.S180:
                            LowerPoints = new Position[3];
                            LowerPoints[0] = new Position(-1, 0);
                            LowerPoints[1] = new Position(0, 0);
                            LowerPoints[2] = new Position(1, 0);
                            break;
                        default:
                            LowerPoints = new Position[2];
                            LowerPoints[0] = new Position(0, 1);
                            LowerPoints[1] = new Position(1, 1);
                            break;
                    }
                    break;

                case FigTypes.T:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            LowerPoints = new Position[3];
                            LowerPoints[0] = new Position(-1, 0);
                            LowerPoints[1] = new Position(0, 1);
                            LowerPoints[2] = new Position(1, 0);
                            break;
                        case FigState.S90:
                            LowerPoints = new Position[2];
                            LowerPoints[0] = new Position(-1, 0);
                            LowerPoints[1] = new Position(0, 1);
                            break;
                        case FigState.S180:
                            LowerPoints = new Position[3];
                            LowerPoints[0] = new Position(-1, 0);
                            LowerPoints[1] = new Position(0, 0);
                            LowerPoints[2] = new Position(1, 0);
                            break;
                        default:
                            LowerPoints = new Position[2];
                            LowerPoints[0] = new Position(0, 1);
                            LowerPoints[1] = new Position(1, 0);
                            break;
                    }
                    break;

                case FigTypes.S:
                    if (CurrentFigure.State == FigState.S0)
                    {
                        LowerPoints = new Position[3];
                        LowerPoints[0] = new Position(-1, 0);
                        LowerPoints[1] = new Position(0, 0);
                        LowerPoints[2] = new Position(1, -1);
                    }
                    else
                    {
                        LowerPoints = new Position[2];
                        LowerPoints[0] = new Position(-1, 0);
                        LowerPoints[1] = new Position(0, 1);
                    }
                    break;

                case FigTypes.Z:
                    if (CurrentFigure.State == FigState.S0)
                    {
                        LowerPoints = new Position[3];
                        LowerPoints[0] = new Position(-1, -1);
                        LowerPoints[1] = new Position(0, 0);
                        LowerPoints[2] = new Position(1, 0);
                    }
                    else
                    {
                        LowerPoints = new Position[2];
                        LowerPoints[0] = new Position(-1, 1);
                        LowerPoints[1] = new Position(0, 0);
                    }
                    break;

                default:
                    LowerPoints = new Position[2];
                    LowerPoints[0] = new Position(-1, 0);
                    LowerPoints[1] = new Position(0, 0);
                    break;
            }

            return LowerPoints;
        }



        // левые точки фигуры
        private Position[] GetLeftPoints(Position[] AllPoints)
        {
            List<Position> PosList = new List<Position>();

            PosList.Add(new Position(0, 0));
            for (int i = 0; i < AllPoints.Length; i++)
            {
                // берем i-ю точку
                Position Pi = AllPoints[i];

                // ищем с таким же Y-ом
                int indx = -1;
                for (int j = 0; j < PosList.Count; j++)
                {
                    if (Pi.Y == PosList[j].Y)
                    {
                        indx = j;
                        break;
                    }
                }

                // если есть 
                if (indx != -1)
                {
                    // сравниваем
                    if (Pi.X < PosList[indx].X)
                        // если X-меньше, заменяем
                        PosList[indx] = Pi;
                }
                else                // если нет добавляем
                    PosList.Add(Pi);
            }



            Position[] LeftPoints = new Position[PosList.Count];
            for (int i = 0; i < LeftPoints.Length; i++)
                LeftPoints[i] = PosList[i];
            return LeftPoints;
        }



        // правые точки фигуры
        private Position[] GetRightPoints(Position[] AllPoints)
        {
            List<Position> PosList = new List<Position>();

            PosList.Add(new Position(0, 0));
            for (int i = 0; i < AllPoints.Length; i++)
            {
                // берем i-ю точку
                Position Pi = AllPoints[i];

                // ищем с таким же Y-ом
                int indx = -1;
                for (int j = 0; j < PosList.Count; j++)
                {
                    if (Pi.Y == PosList[j].Y)
                    {
                        indx = j;
                        break;
                    }
                }

                // если есть 
                if (indx != -1)
                {
                    // сравниваем
                    if (Pi.X > PosList[indx].X)
                        // если X-меньше, заменяем
                        PosList[indx] = Pi;
                }
                else                // если нет добавляем
                    PosList.Add(Pi);
            }



            Position[] LeftPoints = new Position[PosList.Count];
            for (int i = 0; i < LeftPoints.Length; i++)
                LeftPoints[i] = PosList[i];
            return LeftPoints;
        }



        // уперается ли фигура в пол
        private bool IsBottom()
        {
            // кубиков от центра до низа фигуры
            byte PtToDown = BoxToBottom();

            if (CurrentFigure.MainPos.Y == FieldHeight - PtToDown - 1)
                return true;
            else
                return false;
        }
        
        

        // уперлись ли в левый край поля
        private bool IsLeftSide()
        {
            // кубиков от центра до левого края фигуры
            byte PtToLeft = BoxToLeft();

            if (CurrentFigure.MainPos.X == PtToLeft)
                return true;
            else
                return false;
        }

        

        // уперлись ли в правый край поля
        private bool IsRightSide()
        {
            // кубиков от центра до правого края фигуры
            byte PtToRight = BoxToRight();

            if (CurrentFigure.MainPos.X == FieldWidth - PtToRight - 1)
                return true;
            else
                return false;
        }


        
        // кубиков от центра до низа фгуры
        private byte BoxToBottom()
        {
            byte rez = 0;
            switch (CurrentFigure.Type)
            {
                case FigTypes.I:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            rez = 1;
                            break;
                        case FigState.S180:
                            rez = 2;
                            break;
                        default:    // плошмя
                            rez = 0;
                            break;
                    }
                    break;

                case FigTypes.Z:
                    if (CurrentFigure.State == FigState.S0)
                        rez = 0;
                    else
                        rez = 1;
                    break;
                case FigTypes.S:
                    if (CurrentFigure.State == FigState.S0)
                        rez = 0;
                    else
                        rez = 1;
                    break;

                case FigTypes.J:
                    if (CurrentFigure.State == FigState.S180)
                        rez = 0;
                    else
                        rez = 1;
                    break;
                case FigTypes.L:
                    if (CurrentFigure.State == FigState.S180)
                        rez = 0;
                    else
                        rez = 1;
                    break;
                
                case FigTypes.T:
                    if (CurrentFigure.State == FigState.S180)
                        rez = 0;
                    else
                        rez = 1;
                    break;

                default:    // кубик
                    rez = 0;
                    break;
            }

            return rez;
        }



        // кубиков от центра до левого края фгуры
        private byte BoxToLeft()
        {
            byte rez = 0;
            switch (CurrentFigure.Type)
            {
                case FigTypes.I:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S90:
                            rez = 1;
                            break;
                        case FigState.S270:
                            rez = 2;
                            break;
                        default:
                            rez = 0;
                            break;
                    }
                    break;

                case FigTypes.J:
                    if (CurrentFigure.State == FigState.S270)
                        rez = 0;
                    else
                        rez = 1;
                    break;
                case FigTypes.L:
                    if (CurrentFigure.State == FigState.S270)
                        rez = 0;
                    else
                        rez = 1;
                    break;

                case FigTypes.S:
                    rez = 1;
                    break;
                case FigTypes.Z:
                    rez = 1;
                    break;

                case FigTypes.T:
                    if (CurrentFigure.State == FigState.S270)
                        rez = 0;
                    else
                        rez = 1;
                    break;

                default:
                    rez = 1;
                    break;
            }
            return rez;
        }



        // кубиков от центра до правого края фгуры
        private byte BoxToRight()
        {
            byte rez = 0;
            switch (CurrentFigure.Type)
            {
                case FigTypes.I:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S90:
                            rez = 2;
                            break;
                        case FigState.S270:
                            rez = 1;
                            break;
                        default:
                            rez = 0;
                            break;
                    }
                    break;

                case FigTypes.J:
                    if (CurrentFigure.State == FigState.S90)
                        rez = 0;
                    else
                        rez = 1;
                    break;
                case FigTypes.L:
                    if (CurrentFigure.State == FigState.S90)
                        rez = 0;
                    else
                        rez = 1;
                    break;

                case FigTypes.S:
                    if (CurrentFigure.State == FigState.S0)
                        rez = 1;
                    else
                        rez = 0;
                    break;
                case FigTypes.Z:
                    if (CurrentFigure.State == FigState.S0)
                        rez = 1;
                    else
                        rez = 0;
                    break;

                case FigTypes.T:
                    if (CurrentFigure.State == FigState.S90)
                        rez = 0;
                    else
                        rez = 1;
                    break;

                default:
                    rez = 0;
                    break;
            }
            return rez;
        }


             
        // прорисовать текущую фигуру (на поле)
        private void PaintCurrentFigure(bool clear)
        {
            Graphics g = pnlField.CreateGraphics();
            SqrColors cl = SqrColors.White;
            Position[] PosArr= new Position[4];
            PosArr[0] = new Position(CurrentFigure.MainPos.X, CurrentFigure.MainPos.Y);

            // Инексы кубиков фигуры
            switch (CurrentFigure.Type)
            {
                case FigTypes.I:
                    switch (CurrentFigure.State)
                    {
                        
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_I_0[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_I_0[i, 1]));
                            break;
                        case FigState.S90:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_I_90[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_I_90[i, 1]));
                            break;
                        case FigState.S180:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_I_180[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_I_180[i, 1]));
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_I_270[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_I_270[i, 1]));
                            break;
                    }
                    break;

                case FigTypes.J:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_J_0[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_J_0[i, 1]));
                            break;
                        case FigState.S90:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_J_90[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_J_90[i, 1]));
                            break;
                        case FigState.S180:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_J_180[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_J_180[i, 1]));
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_J_270[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_J_270[i, 1]));                            break;
                    }
                    break;

                case FigTypes.L:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_L_0[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_L_0[i, 1]));
                            break;
                        case FigState.S90:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_L_90[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_L_90[i, 1]));
                            break;
                        case FigState.S180:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_L_180[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_L_180[i, 1]));
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_L_270[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_L_270[i, 1]));
                            break;
                    }
                    break;

                case FigTypes.T:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_T_0[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_T_0[i, 1]));
                            break;
                        case FigState.S90:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_T_90[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_T_90[i, 1]));
                            break;
                        case FigState.S180:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_T_180[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_T_180[i, 1]));
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_T_270[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_T_270[i, 1]));
                            break;
                    }
                    break;

                case FigTypes.S:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_S_0[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_S_0[i, 1]));
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_S_90[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_S_90[i, 1]));
                            break;
                    }
                    break;

                case FigTypes.Z:
                    switch (CurrentFigure.State)
                    {
                        case FigState.S0:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_Z_0[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_Z_0[i, 1]));
                            break;
                        default:
                            for (int i = 0; i < 3; i++)
                                PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_Z_90[i, 0]),
                                                             (sbyte)(CurrentFigure.MainPos.Y + Fig_Z_90[i, 1]));
                            break;
                    }
                    break;

                default:
                    for (int i = 0; i < 3; i++)
                        PosArr[i + 1] = new Position((sbyte)(CurrentFigure.MainPos.X + Fig_D_0[i, 0]),
                                                     (sbyte)(CurrentFigure.MainPos.Y + Fig_D_0[i, 1]));
                    break;
            }

            // цвет
            if (clear == true)
                cl = SqrColors.White;
            else
                cl = CurrentFigure.Color;

            // рисуем 4 кубика
            for (int i = 0; i < 4; i++)
                PaintOneBox(g, cl, PosArr[i].X, PosArr[i].Y);

            // освобождаем ресурс
            g.Dispose();
        }

                       
        
        // прорисовать фигуру один кубик
        private void PaintOneBox(Graphics g, SqrColors cl,  sbyte X, sbyte Y)
        {
            switch (cl)
            {
                case SqrColors.Blue:
                    g.DrawImageUnscaled(ImagesRsc.SqrBlue, new Point(X * 24 + 3, Y * 24 + 3));
                    break;
                case SqrColors.Brown:
                    g.DrawImageUnscaled(ImagesRsc.SqrBrown, new Point(X * 24 + 3, Y * 24 + 3));
                    break;
                case SqrColors.Green:
                    g.DrawImageUnscaled(ImagesRsc.SqrGreen, new Point(X * 24 + 3, Y * 24 + 3));
                    break;
                case SqrColors.Purple:
                    g.DrawImageUnscaled(ImagesRsc.SqrPurple, new Point(X * 24 + 3, Y * 24 + 3));
                    break;
                case SqrColors.Red:
                    g.DrawImageUnscaled(ImagesRsc.SqrRed, new Point(X * 24 + 3, Y * 24 + 3));
                    break;
                default:
                    g.DrawImageUnscaled(ImagesRsc.SqrEmpty, new Point(X * 24 + 3, Y * 24 + 3));
                    break;
            }
            Field[X, Y] = cl;
        }

        
        // прорисовать следующую
        private void PaintNextFigure()
        {

            Graphics g = pictBoxNextFigure.CreateGraphics();
            g.Clear(pictBoxNextFigure.BackColor);

            SqrColors cl = NextFigure.Color;
            Position[] PosArr = new Position[4];
            
            //PosArr[0] = new Position(CurrentFigure.MainPos.X, CurrentFigure.MainPos.Y);
            // Инексы кубиков фигуры
            // PoaArr[0] - Координата центра
            // PosArr[1-3] - СМЕЩЕНИЯ относительно центра

            switch (NextFigure.Type)
            {
                case FigTypes.I:
                    switch (NextFigure.State)
                    {
                        case FigState.S0:
                            PosArr[0] = new Position(39, 51);
                            fillMass(PosArr, Fig_I_0);
                            break;
                        case FigState.S90:
                            PosArr[0] = new Position(28, 40);
                            fillMass(PosArr, Fig_I_90);
                            break;
                        case FigState.S180:
                            PosArr[0] = new Position(40, 28);
                            fillMass(PosArr, Fig_I_180);
                            break;
                        default:
                            PosArr[0] = new Position(51, 39);
                            fillMass(PosArr, Fig_I_270);
                            break;
                    }
                    break;


                case FigTypes.J:
                    switch (NextFigure.State)
                    {
                        case FigState.S0:
                            PosArr[0] = new Position(39, 28);
                            fillMass(PosArr, Fig_J_0);
                            break;
                        case FigState.S90:
                            PosArr[0] = new Position(51, 39);
                            fillMass(PosArr, Fig_J_90);
                            break;
                        case FigState.S180:
                            PosArr[0] = new Position(40, 51);
                            fillMass(PosArr, Fig_J_180);
                            break;
                        default:
                            PosArr[0] = new Position(28, 40);
                            fillMass(PosArr, Fig_J_270);
                            break;
                    }
                    break;

                case FigTypes.L:
                    switch (NextFigure.State)
                    {
                        case FigState.S0:
                            PosArr[0] = new Position(41, 28);
                            fillMass(PosArr, Fig_L_0);
                            break;
                        case FigState.S90:
                            PosArr[0] = new Position(51, 40);
                            fillMass(PosArr, Fig_L_90);
                            break;
                        case FigState.S180:
                            PosArr[0] = new Position(39, 51);
                            fillMass(PosArr, Fig_L_180);
                            break;
                        default:
                            PosArr[0] = new Position(28, 39);
                            fillMass(PosArr, Fig_L_270);
                            break;
                    }
                    break;

                case FigTypes.T:
                    switch (NextFigure.State)
                    {
                        case FigState.S0:
                            PosArr[0] = new Position(40, 28);
                            fillMass(PosArr, Fig_T_0);
                            break;
                        case FigState.S90:
                            PosArr[0] = new Position(51, 39);
                            fillMass(PosArr, Fig_T_90);
                            break;
                        case FigState.S180:
                            PosArr[0] = new Position(40, 51);
                            fillMass(PosArr, Fig_T_180);
                            break;
                        default:
                            PosArr[0] = new Position(28, 39);
                            fillMass(PosArr, Fig_T_270);
                            break;
                    }
                    break;

                case FigTypes.S:
                    switch (NextFigure.State)
                    {
                        case FigState.S0:
                            PosArr[0] = new Position(40, 51);
                            fillMass(PosArr, Fig_S_0);
                            break;
                        default:
                            PosArr[0] = new Position(52, 40);
                            fillMass(PosArr, Fig_S_90);
                            break;
                    }
                    break;

                case FigTypes.Z:
                    switch (NextFigure.State)
                    {
                        case FigState.S0:
                            PosArr[0] = new Position(40, 52);
                            fillMass(PosArr, Fig_Z_0);
                            break;
                        default:
                            PosArr[0] = new Position(51, 39);
                            fillMass(PosArr, Fig_Z_90);
                            break;
                    }
                    break;


                default:
                    PosArr[0] = new Position(51, 51);
                    fillMass(PosArr, Fig_D_0);
                    break;

            }
            

            // рисуем 4 кубика
            Image img = SwitchColor(cl);

            g.DrawImageUnscaled(img, new Point(PosArr[0].X, PosArr[0].Y));
            g.DrawImageUnscaled(img, new Point(PosArr[0].X + PosArr[1].X * 24, PosArr[0].Y + PosArr[1].Y * 24));
            g.DrawImageUnscaled(img, new Point(PosArr[0].X + PosArr[2].X * 24, PosArr[0].Y + PosArr[2].Y * 24));
            g.DrawImageUnscaled(img, new Point(PosArr[0].X + PosArr[3].X * 24, PosArr[0].Y + PosArr[3].Y * 24));
            
            //for (int i = 0; i < 4; i++)
              //  PaintOneBox(g, cl, PosArr[i].X, PosArr[i].Y);

            // освобождаем ресурс
            img.Dispose();
            g.Dispose();
        }


        // выбрать изображение для цвета
        private Image SwitchColor(SqrColors cl)
        {
            Image img;
            switch (cl)
            {
                case SqrColors.Blue:
                    img = ImagesRsc.SqrBlue;
                    break;
                case SqrColors.Brown:
                    img = ImagesRsc.SqrBrown;
                    break;
                case SqrColors.Green:
                    img = ImagesRsc.SqrGreen;
                    break;
                case SqrColors.Purple:
                    img = ImagesRsc.SqrPurple;
                    break;
                case SqrColors.Red:
                    img = ImagesRsc.SqrRed;
                    break;
                default:
                    img = ImagesRsc.SqrEmpty;
                    break;
            }
            
            return img;
        }


        // заполнить массив индексов для данной фигуры
        private void fillMass(Position[] dest, sbyte[,] src)
        {
            for (int i = 0; i < 3; i++ )
            {
                dest[i + 1] = new Position(src[i, 0], src[i, 1]);
            }
        }



        // прорисовка поля
        private void pnlField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int x = 0; x < FieldWidth; x++)
            {
                for (int y = 0; y < FieldHeight; y++)
                {
                    SqrColors c = Field[x, y];
                    switch (c)
                    {
                        case SqrColors.Blue:
                            g.DrawImageUnscaled(ImagesRsc.SqrBlue, new Point(x * 24 + 3, y * 24 + 3));
                            break;
                        case SqrColors.Brown:
                            g.DrawImageUnscaled(ImagesRsc.SqrBrown, new Point(x * 24 + 3, y * 24 + 3));
                            break;
                        case SqrColors.Green:
                            g.DrawImageUnscaled(ImagesRsc.SqrGreen, new Point(x * 24 + 3, y * 24 + 3));
                            break;
                        case SqrColors.Purple:
                            g.DrawImageUnscaled(ImagesRsc.SqrPurple, new Point(x * 24 + 3, y * 24 + 3));
                            break;
                        case SqrColors.Red:
                            g.DrawImageUnscaled(ImagesRsc.SqrRed, new Point(x * 24 + 3, y * 24 + 3));
                            break;

                        default:
                            g.DrawImageUnscaled(ImagesRsc.SqrEmpty, new Point(x * 24 + 3, y * 24 + 3));
                            break;
                    }
                }
            }
            
            g.Dispose();

            PaintNextFigure();
        }



        // таймер движения вниз
        private void MoveDownTimer_Tick(object sender, EventArgs e)
        {
            StepDown();
        }



        // деактивация формы
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            //MessageBox.Show("Deactivate");
            if (IsRunned == true)
            {
                MoveDownTimer.Stop();
                IsRunned = false;
                //pnlField.Paint -= pnlField_Paint;

                frPause form = new frPause();
                form.ShowDialog();
                
                IsRunned = true;
                needActivate = true;
                //pnlField.Paint += new PaintEventHandler(pnlField_Paint);
                pnlField.Refresh();

                MoveDownTimer.Start();
                //this.Activate();
                //this.pnlField.Focus();
                //this.Show();
            }

        }



        // вращение влево
        private void picBox_RtLeft_Click(object sender, EventArgs e)
        {
            if (RotateDirect == Directs.Right)
            {
                RotateDirect = Directs.Left;
                picBox_RtLeft.Image = ImagesRsc.RtLeftOn;
                picBox_RtRight.Image = ImagesRsc.RtRightOff;
            }
        }


        // вращение вправо
        private void picBox_RtRight_Click(object sender, EventArgs e)
        {
            if (RotateDirect == Directs.Left)
            {
                RotateDirect = Directs.Right;
                picBox_RtLeft.Image = ImagesRsc.RtLeftOff;
                picBox_RtRight.Image = ImagesRsc.RtRightOn;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsRunned = false;
        }





      

       

        private void MainForm_Leave(object sender, EventArgs e)
        {
            MessageBox.Show("dfd");
        }

        private void lblStart_Click(object sender, EventArgs e)
        {
            if (IsRunned == true)
            {
                MoveDownTimer.Stop();
                IsRunned = false;
                //pnlField.Paint -= pnlField_Paint;

                frPause form = new frPause();
                form.ShowDialog();

                IsRunned = true;
                needActivate = true;
                //pnlField.Paint += new PaintEventHandler(pnlField_Paint);
                pnlField.Refresh();

                MoveDownTimer.Start();
            }
            else
                Start();
        }

        

       

        

        
        

    }
}
