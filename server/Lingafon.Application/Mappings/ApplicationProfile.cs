using AutoMapper;
using Lingafon.Application.DTOs;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Core.Entities;

namespace Lingafon.Application.Mappings;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<User, UserReadDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>().ForAllMembers(opts 
            => opts.Condition((src, dest, srcMember) 
                => srcMember != null));

        CreateMap<Assignment, AssignmentReadDto>();
        CreateMap<AssignmentCreateDto, Assignment>();
        CreateMap<AssignmentUpdateDto, Assignment>().ForAllMembers(opts
            => opts.Condition((src, dest, srcMember) 
                => srcMember != null));

        CreateMap<AssignmentResult, AssignmentResultReadDto>();
        CreateMap<AssignmentResultCreateDto, AssignmentResult>();

        CreateMap<Dialog, DialogReadDto>();
        CreateMap<DialogCreateDto, Dialog>();

        CreateMap<Message, MessageReadDto>();
        CreateMap<MessageCreateDto, Message>();
    }
}

