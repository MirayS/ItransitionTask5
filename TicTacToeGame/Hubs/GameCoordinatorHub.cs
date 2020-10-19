using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TicTacToeGame.Context;
using TicTacToeGame.Context.Models;
using TicTacToeGame.Models;
using TicTacToeGame.Services;

namespace TicTacToeGame.Hubs
{
    public class GameCoordinatorHub : Hub
    {
        private readonly MainContext _dbContext;
        private readonly IMapper _mapper;
        private readonly GameService _game;

        public GameCoordinatorHub(MainContext dbContext, IMapper mapper, GameService game)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _game = game;
        }

        public async Task CreateRoom(string name)
        {
            if (_game.IsUserInGame(Context.ConnectionId))
                return;
            
            var room = _game.CreateNewRoom(name, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
            await Clients.All.SendAsync("newRoom", room);
            await Clients.Caller.SendAsync("moveToRoom", room);
        }

        public async Task ConnectToRoom(Guid roomId)
        {
            if (_game.IsUserInGame(Context.ConnectionId))
                return;
            var room = _game.GetRoom(roomId);
            if (room.PlayerTwo != null) 
                return;
            room.PlayerTwo = Context.ConnectionId;
            room.IsStarted = true;
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
            await Clients.All.SendAsync("removeRoom", room.Id);
            await Clients.Caller.SendAsync("moveToRoom", room);
            await Clients.OthersInGroup(room.Id.ToString()).SendAsync("roomUpdated", room);
        }

        public async Task MakeStep(int row, int column)
        {
            if (!_game.IsUserInGame(Context.ConnectionId))
                return;
            var room = _game.GetRoomWithUser(Context.ConnectionId);
            if (!room.IsStarted)
                return;
            if (row > 3 || column > 3 || room.PlayingField[row][column] != 0)
                return;
            room.PlayingField[row][column] = room.IsFirstPlayerStep ? 1 : 2;
            if (_game.IsGameEnd(room))
            {
                room.IsFinished = true;
                room.IsFirstPlayerWin = !_game.IsDraw(room) ? room.IsFirstPlayerStep : (bool?)null;
            }
            room.IsFirstPlayerStep = !room.IsFirstPlayerStep;
            await Clients.Group(room.Id.ToString()).SendAsync("roomUpdated", room);
        }

        public async Task LeaveFromRoom()
        {
            if (!_game.IsUserInGame(Context.ConnectionId))
                return;
            var room = _game.GetRoomWithUser(Context.ConnectionId);
            if (!room.IsStarted)
                return;
            if (!room.IsFinished)
                return;
            _game.LeaveFromRoom(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id.ToString());
            await Clients.Caller.SendAsync("leaveFromRoom");
        }
        
        public async Task GetConnectionId()
        {
            await Clients.Caller.SendAsync("getConnectionId", Context.ConnectionId);
        }
        
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("rooms", _game.Rooms.Where(x => x.IsStarted == false));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var room = _game.GetRoomWithUser(Context.ConnectionId);
            if (room == null)
                return;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id.ToString());

            if (room.IsStarted)
            {
                room.IsFinished = true;
                room.IsFirstPlayerWin = room.PlayerOne != Context.ConnectionId;
                await Clients.Group(room.Id.ToString()).SendAsync("roomUpdated", room);
            }
            _game.LeaveFromRoom(Context.ConnectionId);

            await Clients.Others.SendAsync("removeRoom", room.Id);
            await base.OnDisconnectedAsync(exception);
        }
    }
}