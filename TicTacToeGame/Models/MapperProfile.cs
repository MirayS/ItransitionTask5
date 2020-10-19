using AutoMapper;
using TicTacToeGame.Context.Models;

namespace TicTacToeGame.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RoomDto, Room>().ForMember(x => x.Id, r => r.Ignore());
            CreateMap<Room, RoomDto>();
        }
    }
}