using System;
using AutoMapper;
using server.DTO.User;
using server.Repository.IRepository;
using server.Services.IServices;

namespace server.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<UserDTO>> GetTopReputationUsersAsync()
    {
        var users = await _userRepository.GetTopReputationUsersAsync();
        return _mapper.Map<List<UserDTO>>(users);
    }
}
