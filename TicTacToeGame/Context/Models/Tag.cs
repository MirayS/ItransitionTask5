using System.Collections.Generic;

namespace TicTacToeGame.Context.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Value { get; set; }
        
        public List<RoomTag> RoomTags { get; set; }

        public Tag()
        {
            RoomTags = new List<RoomTag>();
        }
    }
}