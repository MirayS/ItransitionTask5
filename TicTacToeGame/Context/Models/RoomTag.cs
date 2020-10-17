namespace TicTacToeGame.Context.Models
{
    public class RoomTag
    {
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}