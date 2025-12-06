using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Core.Entities;

namespace Lingafon.Application.Mappings;

public class LingafonMappingProfile : Profile
{
    public LingafonMappingProfile()
    {
        // Assignment mappings
        CreateMap<Assignment, AssignmentReadDto>();
        CreateMap<AssignmentCreateDto, Assignment>();
        CreateMap<AssignmentUpdateDto, Assignment>();
        
        // User mappings
        CreateMap<User, UserReadDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();
        
        // Dialog mappings
        CreateMap<Dialog, DialogReadDto>();
        CreateMap<DialogCreateDto, Dialog>();
        
        // Message mappings
        CreateMap<Message, MessageReadDto>();
        CreateMap<MessageCreateDto, Message>();
        
        // AssignmentResult mappings
        CreateMap<AssignmentResult, AssignmentResultReadDto>();
        CreateMap<AssignmentResultCreateDto, AssignmentResult>();
    }
}