using Carden.Api.Dtos;
using Carden.Api.Utils;

namespace Carden.Api.Services;

public interface IUserService
{
    public Task<Result<User>> GetUser(Guid userId);
    public Task<Result<object>> UploadProfileImage(Guid userId, IFormFile formFile);
    public Task<Result<User>> DeleteUser(Guid userId);

}

public class UserService(IUserRepository userRepository, ICloudinaryService cloudinaryService): IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICloudinaryService _cloudinaryService = cloudinaryService;

    public async Task<Result<User>> GetUser(Guid userId)
    {
        try
        {
            var user = await _userRepository.FindById(userId);
            if (user is null) return Result.Failure<User>(Error.BadRequest("User not found!"));

            return Result.Success(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }
    
    public async Task<Result<object>> UploadProfileImage(Guid userId, IFormFile formFile)
    {
        try
        {
            var user = await _userRepository.FindById(userId);
            if (user is null) return Result.Failure(Error.BadRequest("User not found"));

            if (user.ProfileImageUrl is not null) throw new NotImplementedException();

            var cloudinaryUploadRequest = new CloudinaryUploadRequest(formFile, "profile_images", null);

            var uploadResult = await _cloudinaryService.UploadImageAsync(cloudinaryUploadRequest);

            if (!uploadResult.Success) return Result.Failure(Error.BadRequest(uploadResult.ErrorMessage!));

            return Result.Success(uploadResult);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }

    public async Task<Result<User>> DeleteUser(Guid userId)
    {
        try
        {
            var user = await _userRepository.FindById(userId);
            if (user is null) return Result.Failure<User>(Error.BadRequest("User not found!"));
            
            user.DeletedAt = DateTime.UtcNow;
            
            var deletedUser = await _userRepository.Update(user);
            return (deletedUser is null)
                ? Result.Success(deletedUser)
                : Result.Failure<User>(Error.BadRequest("Error deleting user"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }
}