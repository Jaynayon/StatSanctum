using AutoMapper;
using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>();
            CreateMap<Rarity, RarityDto>();
            CreateMap<RarityDto, Rarity>();
            CreateMap<ItemType, ItemTypeDto>();
            CreateMap<ItemTypeDto, ItemType>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
