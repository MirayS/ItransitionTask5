using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TicTacToeGame.Context.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
        
        public List<RoomTag> RoomTags { get; set; }
        
        public string PlayingFieldSerialize { get; set; }
        
        [NotMapped]
        public List<int> PlayingField { 
            get => !string.IsNullOrEmpty(PlayingFieldSerialize) ? JsonConvert.DeserializeObject<List<int>>(PlayingFieldSerialize) : null; 
            set => PlayingFieldSerialize = JsonConvert.SerializeObject(value); 
        }

        public Room()
        {
            RoomTags = new List<RoomTag>();
        }
    }
}