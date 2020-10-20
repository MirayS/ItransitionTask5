using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToeGame.Models;

namespace TicTacToeGame.Services
{
    public class GameService
    {
        public List<RoomDto> Rooms { get; }
        public SortedSet<string> Tags { get; set; }

        public GameService()
        {
            Rooms = new List<RoomDto>();
            Tags = new SortedSet<string>();
        }

        public RoomDto CreateNewRoom(string roomName, List<string> tags, string creatorId)
        {
            var room = new RoomDto()
            {
                Id = Guid.NewGuid(),
                Name = roomName,
                IsFinished = false,
                IsStarted = false,
                IsFirstPlayerStep = true,
                PlayerOne = creatorId,
                PlayerTwo = null,
                PlayingField = new[] {new[] {0, 0, 0}, new[] {0, 0, 0}, new[] {0, 0, 0}},
                IsFirstPlayerWin = null,
                Tags = tags
            };

            Rooms.Add(room);

            return room;
        }

        public bool IsUserInGame(string userId)
        {
            return Rooms.Any(x => x.PlayerOne == userId || x.PlayerTwo == userId);
        }

        public RoomDto GetRoomWithUser(string userId)
        {
            return Rooms.FirstOrDefault(x => x.PlayerOne == userId || x.PlayerTwo == userId);
        }

        public RoomDto GetRoom(Guid roomId)
        {
            return Rooms.FirstOrDefault(x => x.Id == roomId);
        }

        public bool IsGameEnd(RoomDto room)
        {
            if (IsDraw(room))
                return true;
            for (var i = 0; i < 3; i++)
            {
                if (room.PlayingField[i][0] != 0 &&
                    room.PlayingField[i][0] == room.PlayingField[i][1] &&
                    room.PlayingField[i][0] == room.PlayingField[i][2])
                    return true;
                if (room.PlayingField[0][i] != 0 &&
                    room.PlayingField[0][i] == room.PlayingField[1][i] &&
                    room.PlayingField[0][i] == room.PlayingField[2][i])
                    return true;
            }
            if (room.PlayingField[0][0] != 0 &&
                room.PlayingField[0][0] == room.PlayingField[1][1] &&
                room.PlayingField[0][0] == room.PlayingField[2][2])
                return true;
            
            return room.PlayingField[0][2] != 0 &&
                   room.PlayingField[0][2] == room.PlayingField[1][1] &&
                   room.PlayingField[0][2] == room.PlayingField[2][0];
        }

        public bool IsDraw(RoomDto room)
        {
            return room.PlayingField.All(row => row.All(column => column != 0));
        }

        public void LeaveFromRoom(string userId)
        {
            var room = GetRoomWithUser(userId);
            if (room == null)
                return;
            if (room.PlayerOne == userId)
            {
                room.PlayerOne = null;
            }
            if (room.PlayerTwo == userId)
            {
                room.PlayerTwo = null;
            }
            if (room.PlayerOne == null && room.PlayerTwo == null)
            {
                Rooms.Remove(room);
            }
        }
    }
}