using API_Agenda.Models;
using AutoMapper;

namespace API_Agenda.DTOs.Mappings;

public class ContatoDTOMappingProfile : Profile
{
    public ContatoDTOMappingProfile()
    {
        CreateMap<Contato, ContatoDTO>().ReverseMap();  
        CreateMap<ContatoDTO, Contato>().ReverseMap();
    }
}
