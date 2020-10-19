using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TicTacToeGame.Context;
using TicTacToeGame.Context.Models;

namespace TicTacToeGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly MainContext _dbContext;

        public RoomsController(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Room> Get()
        {
            return _dbContext.Rooms;
        }

        [HttpPost]
        public IActionResult Create(Room room)
        {
            _dbContext.Rooms.Add(room);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}