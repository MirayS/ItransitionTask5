using System;
using System.Collections.Generic;

namespace TicTacToeGame.Models
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public bool IsFirstPlayerStep { get; set; }
        public bool? IsFirstPlayerWin { get; set; }
        
        public int[][] PlayingField { get; set; }
    }
}