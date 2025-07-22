using Carden.Api.Dtos;
using Carden.Api.Utils;

namespace Carden.Api.Services;

public interface IUserService
{
    public Task<Result<User>> GetUser(Guid userId);
    public Task<Result<object>> UploadProfileImage(Guid userId, CloudinaryUploadRequest uploadRequest);
    
}

public class UserService(IUserRepository userRepository, ICloudinaryService cloudinaryService): IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICloudinaryService _cloudinaryService = cloudinaryService;

    public async Task<Result<User>> GetUser(Guid userId)
    {
        var user = await _userRepository.FindById(userId);
        if (user is null) return Result.Failure<User>(Error.BadRequest("User not found!"));

        return Result.Success(user);
    }
    
    public async Task<Result<object>> UploadProfileImage(Guid userId, CloudinaryUploadRequest uploadRequest)
    {
        var user = await _userRepository.FindById(userId);
        if (user is null) return Result.Failure(Error.BadRequest("User not found"));

        if (user.ProfileImageUrl is not null) throw new NotImplementedException();

        var uploadResult = await _cloudinaryService.UploadImageAsync(uploadRequest);

        if (!uploadResult.Success) return Result.Failure(Error.BadRequest(uploadResult.ErrorMessage!));

        return Result.Success(uploadResult);
    }
}