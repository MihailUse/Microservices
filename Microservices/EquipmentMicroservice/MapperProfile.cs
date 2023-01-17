using AutoMapper;
using EquipmentMicroservice.DAL.Entities;
using EquipmentMicroservice.Models;

namespace EquipmentMicroservice;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateEquipmentModel, Equipment>().ReverseMap();
    }
}
