namespace GameOfLife
{
    internal class Cell
    {
        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public int Age { get; set; }

        public bool IsAlive { get; set; }

        public Cell(int row, int column, int age, bool alive)
        {
            this.PositionX = row * 5;
            this.PositionY = column * 5;
            this.Age = age;
            this.IsAlive = alive;
        }
    }
}