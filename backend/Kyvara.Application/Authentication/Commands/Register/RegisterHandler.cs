using MediatR;
using Kyvara.Application.Authentication.Interfaces;
using Kyvara.Domain.Users;
using Kyvara.Domain.Users.Entities;
using Kyvara.Domain.Users.Repositories;
using Kyvara.Domain.Users.ValueObjects;

namespace Kyvara.Application.Authentication.Commands.Register;

public sealed class RegisterHandler
    : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _repository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterHandler(
        IUserRepository repository,
        IUserProfileRepository profileRepository,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var email = new UserEmail(request.Email);

        var exists = await _repository.GetByEmailAsync(
            email,
            cancellationToken);

        if (exists is not null)
            throw new Exception("Email already exists.");

        var password = new UserPassword(
            _passwordHasher.Hash(request.Password));

        var user = User.Register(
            email,
            password,
            DateTime.UtcNow);

        await _repository.AddAsync(
            user,
            cancellationToken);

        var username = new Username(
            request.Email.Split('@')[0]);

        var profile = UserProfile.Create(
            user.Id.Value,
            username,
            request.FullName);

        await _profileRepository.AddAsync(
            profile,
            cancellationToken);

        await _profileRepository.UpdateAsync(
            profile,
            cancellationToken);

        return user.Id.Value;
    }
}