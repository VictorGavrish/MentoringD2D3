namespace GameOfLife
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    internal class Grid
    {
        private readonly int sizeX;

        private readonly int sizeY;

        private readonly Cell[,] cells;

        private readonly Cell[,] nextGenerationCells;

        private static Random rnd;

        private readonly Canvas drawCanvas;

        private readonly Ellipse[,] cellsVisuals;

        public Grid(Canvas c)
        {
            this.drawCanvas = c;
            rnd = new Random();
            this.sizeX = (int)(c.Width / 5);
            this.sizeY = (int)(c.Height / 5);
            this.cells = new Cell[this.sizeX, this.sizeY];
            this.nextGenerationCells = new Cell[this.sizeX, this.sizeY];
            this.cellsVisuals = new Ellipse[this.sizeX, this.sizeY];

            for (var i = 0; i < this.sizeX; i++)
            {
                for (var j = 0; j < this.sizeY; j++)
                {
                    this.cells[i, j] = new Cell(i, j, 0, false);
                    this.nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                }
            }

            this.SetRandomPattern();
            this.InitCellsVisuals();
            this.UpdateGraphics();
        }

        public void Clear()
        {
            for (var i = 0; i < this.sizeX; i++)
            {
                for (var j = 0; j < this.sizeY; j++)
                {
                    this.cells[i, j] = new Cell(i, j, 0, false);
                    this.nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                    this.cellsVisuals[i, j].Fill = Brushes.Gray;
                }
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            var cellVisual = sender as Ellipse;

            var i = (int)cellVisual.Margin.Left / 5;
            var j = (int)cellVisual.Margin.Top / 5;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.cells[i, j].IsAlive)
                {
                    this.cells[i, j].IsAlive = true;
                    this.cells[i, j].Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        public void UpdateGraphics()
        {
            for (var i = 0; i < this.sizeX; i++)
            {
                for (var j = 0; j < this.sizeY; j++)
                {
                    this.cellsVisuals[i, j].Fill = this.cells[i, j].IsAlive
                                                       ? (this.cells[i, j].Age < 2 ? Brushes.White : Brushes.DarkGray)
                                                       : Brushes.Gray;
                }
            }
        }

        public void InitCellsVisuals()
        {
            for (var i = 0; i < this.sizeX; i++)
            {
                for (var j = 0; j < this.sizeY; j++)
                {
                    this.cellsVisuals[i, j] = new Ellipse();
                    this.cellsVisuals[i, j].Width = this.cellsVisuals[i, j].Height = 5;
                    double left = this.cells[i, j].PositionX;
                    double top = this.cells[i, j].PositionY;
                    this.cellsVisuals[i, j].Margin = new Thickness(left, top, 0, 0);
                    this.cellsVisuals[i, j].Fill = Brushes.Gray;
                    this.drawCanvas.Children.Add(this.cellsVisuals[i, j]);

                    this.cellsVisuals[i, j].MouseMove += this.MouseMove;
                    this.cellsVisuals[i, j].MouseLeftButtonDown += this.MouseMove;
                }
            }

            this.UpdateGraphics();
        }

        public static bool GetRandomBoolean()
        {
            return rnd.NextDouble() > 0.8;
        }

        public void SetRandomPattern()
        {
            for (var i = 0; i < this.sizeX; i++)
            {
                for (var j = 0; j < this.sizeY; j++)
                {
                    this.cells[i, j].IsAlive = GetRandomBoolean();
                }
            }
        }

        public void UpdateToNextGeneration()
        {
            for (var i = 0; i < this.sizeX; i++)
            {
                for (var j = 0; j < this.sizeY; j++)
                {
                    this.cells[i, j].IsAlive = this.nextGenerationCells[i, j].IsAlive;
                    this.cells[i, j].Age = this.nextGenerationCells[i, j].Age;
                }
            }

            this.UpdateGraphics();
        }

        public void Update()
        {
            var alive = false;
            var age = 0;

            for (var i = 0; i < this.sizeX; i++)
            {
                for (var j = 0; j < this.sizeY; j++)
                {
                    // nextGenerationCells[i, j] = CalculateNextGeneration(i,j);          // UNOPTIMIZED
                    this.CalculateNextGeneration(i, j, ref alive, ref age); // OPTIMIZED
                    this.nextGenerationCells[i, j].IsAlive = alive; // OPTIMIZED
                    this.nextGenerationCells[i, j].Age = age; // OPTIMIZED
                }
            }

            this.UpdateToNextGeneration();
        }

        public Cell CalculateNextGeneration(int row, int column)
        {
            // UNOPTIMIZED
            bool alive;
            int count, age;

            alive = this.cells[row, column].IsAlive;
            age = this.cells[row, column].Age;
            count = this.CountNeighbors(row, column);

            if (alive && count < 2)
            {
                return new Cell(row, column, 0, false);
            }

            if (alive && (count == 2 || count == 3))
            {
                this.cells[row, column].Age++;
                return new Cell(row, column, this.cells[row, column].Age, true);
            }

            if (alive && count > 3)
            {
                return new Cell(row, column, 0, false);
            }

            if (!alive && count == 3)
            {
                return new Cell(row, column, 0, true);
            }

            return new Cell(row, column, 0, false);
        }

        public void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)
        {
            // OPTIMIZED
            isAlive = this.cells[row, column].IsAlive;
            age = this.cells[row, column].Age;

            var count = this.CountNeighbors(row, column);

            if (isAlive && count < 2)
            {
                isAlive = false;
                age = 0;
            }

            if (isAlive && (count == 2 || count == 3))
            {
                this.cells[row, column].Age++;
                isAlive = true;
                age = this.cells[row, column].Age;
            }

            if (isAlive && count > 3)
            {
                isAlive = false;
                age = 0;
            }

            if (!isAlive && count == 3)
            {
                isAlive = true;
                age = 0;
            }
        }

        public int CountNeighbors(int i, int j)
        {
            var count = 0;

            if (i != this.sizeX - 1 && this.cells[i + 1, j].IsAlive)
            {
                count++;
            }

            if (i != this.sizeX - 1 && j != this.sizeY - 1 && this.cells[i + 1, j + 1].IsAlive)
            {
                count++;
            }

            if (j != this.sizeY - 1 && this.cells[i, j + 1].IsAlive)
            {
                count++;
            }

            if (i != 0 && j != this.sizeY - 1 && this.cells[i - 1, j + 1].IsAlive)
            {
                count++;
            }

            if (i != 0 && this.cells[i - 1, j].IsAlive)
            {
                count++;
            }

            if (i != 0 && j != 0 && this.cells[i - 1, j - 1].IsAlive)
            {
                count++;
            }

            if (j != 0 && this.cells[i, j - 1].IsAlive)
            {
                count++;
            }

            if (i != this.sizeX - 1 && j != 0 && this.cells[i + 1, j - 1].IsAlive)
            {
                count++;
            }

            return count;
        }
    }
}