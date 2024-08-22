namespace Connect4
{
    internal class Slot
    {
        public static readonly Slot INVALID_SLOT = new Slot
        {
            Player = INVALID_PLAYER,
            Row = INVALID_ROW,
            Column = INVALID_COLUMN
        };

        public int Row { get; set; }
        public int Column { get; set; }
        public int Player { get; set; }
        public Slot() 
        {
            Player = DEFAULT_PLAYER;
            Row = DEFAULT_ROW;
            Column = DEFAULT_COLUMN;
        }
        public bool IsOpenSlot => Player == DEFAULT_PLAYER;
        public bool IsValid() => (Player != INVALID_PLAYER && Row != INVALID_ROW && Column != INVALID_COLUMN);
        

        internal const int DEFAULT_PLAYER = 0;
        internal const int DEFAULT_ROW = 0;
        internal const int DEFAULT_COLUMN = 0;

        public const int INVALID_PLAYER = -1;
        public const int INVALID_ROW = -1;
        public const int INVALID_COLUMN = -1;
    }
}
