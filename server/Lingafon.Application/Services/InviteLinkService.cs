using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;

namespace Lingafon.Application.Services;

public class InviteLinkService : IInviteLinkService
{
    private readonly IInviteLinkRepository _inviteLinkRepository;
    private readonly ITeacherStudentRepository _teacherStudentRepository;
    private readonly IMapper _mapper;

    public InviteLinkService(IInviteLinkRepository inviteLinkRepository, IMapper mapper, ITeacherStudentRepository teacherStudentRepository)
    {
        _inviteLinkRepository = inviteLinkRepository;
        _mapper = mapper;
        _teacherStudentRepository = teacherStudentRepository;
    }

    public async Task<InviteLinkReadDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var inviteLink = await _inviteLinkRepository.GetByIdAsync(id);
        if (inviteLink is null)
            return null;

        return _mapper.Map<InviteLinkReadDto>(inviteLink);
    }

    public async Task<IEnumerable<InviteLinkReadDto>> GetAllAsync()
    {
        var inviteLinks = await _inviteLinkRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<InviteLinkReadDto>>(inviteLinks);
    }

    public async Task<InviteLinkReadDto> CreateAsync(InviteLinkCreateDto dto)
    {
        ValidateCreateDto(dto);

        var inviteLink = new InviteLink
        {
            TeacherId = dto.TeacherId,
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = dto.ExpiresAt,
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _inviteLinkRepository.AddAsync(inviteLink);
        return _mapper.Map<InviteLinkReadDto>(inviteLink);
    }

    public async Task UpdateAsync(InviteLinkCreateDto dto)
    {
        ValidateCreateDto(dto);

        var inviteLink = _mapper.Map<InviteLink>(dto);
        await _inviteLinkRepository.UpdateAsync(inviteLink);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        return await _inviteLinkRepository.DeleteAsync(id);
    }

    public async Task<bool> AcceptInviteLink(Guid userId, string token)
    {
        if(userId == Guid.Empty || string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("UserId and InviteId cannot be empty", nameof(userId));
        
        var inviteLink = await _inviteLinkRepository.GetByTokenAsync(token);
        if(inviteLink is null)
            throw new ArgumentException("Invite doesn't found");

        var teacherStudent = new TeacherStudent()
        {
            TeacherId = inviteLink.TeacherId,
            StudentId = userId,
            CreatedAt = DateTime.UtcNow,
        };
        await _teacherStudentRepository.AddAsync(teacherStudent);
        
        inviteLink.IsUsed = true;
        await _inviteLinkRepository.UpdateAsync(inviteLink);
        
        return true;
    }

    public async Task<IEnumerable<InviteLink>?> GetExpiredInviteLinksAsync()
    {
        var expiredLinks = await _inviteLinkRepository.GetExpiredInviteLinksAsync();
        return expiredLinks;
    }

    public async Task<IEnumerable<InviteLink>?> GetUsedInviteLinksAsync()
    {
        var usedLinks = await _inviteLinkRepository.GetUsedInviteLinksAsync();
        return usedLinks;
    }

    public async Task<IEnumerable<InviteLink>?> GetByTeacherIdAsync(Guid teacherId)
    {
        if (teacherId == Guid.Empty)
            throw new ArgumentException("TeacherId cannot be empty", nameof(teacherId));

        var links = await _inviteLinkRepository.GetByTeacherIdAsync(teacherId);
        return links;
    }

    private void ValidateCreateDto(InviteLinkCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.TeacherId == Guid.Empty)
            throw new ArgumentException("TeacherId cannot be empty", nameof(dto.TeacherId));

        if (dto.ExpiresAt <= DateTime.UtcNow)
            throw new ArgumentException("ExpiresAt must be in the future", nameof(dto.ExpiresAt));
    }
}