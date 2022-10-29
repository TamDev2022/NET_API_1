using AutoMapper;
using AsyncAwaitBestPractices;
using NET_API_1.Configurations.Extensions;
using NET_API_1.Interfaces.IRepositories;
using NET_API_1.Interfaces.IServices;
using NET_API_1.Models.DTO;
using NET_API_1.Models.Entity;
using NET_API_1.Models.Request;
using NET_API_1.Utils;
using System.Security.Claims;

namespace NET_API_1.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordEncoderService _passwordEncoderService;
        private readonly ITokenService _tokenService;
        private readonly IFileService _fileService;
        private readonly IAzureStorageService _azureStorageService;
        private readonly ISendMailService _sendMailService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            IPasswordEncoderService passwordEncoderService, ITokenService tokenService,
            IFileService fileService, IAzureStorageService azureStorageService, ISendMailService sendMailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordEncoderService = passwordEncoderService;
            _tokenService = tokenService;
            _fileService = fileService;
            _azureStorageService = azureStorageService;
            _sendMailService = sendMailService;
        }
        public async Task<PaginatedList<UserDTO>> GetListAsync(int PageNumber, int PageSize)
        {
            var users = await _unitOfWork.UserRepository.GetListAsync(PageNumber, PageSize).ConfigureAwait(false);
            if (users == null) throw new ArgumentException($"Không có user nào.");
            return _mapper.Map<PaginatedList<UserDTO>>(users);
        }
        public async Task<User?> GetUserByIdAsync(int UserId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(UserId).ConfigureAwait(false);
            //if (user == null) throw new ArgumentException($"User id {UserId} không tồn tại");
            return user;
        }

        public async Task<User?> GetUserAsync(string Email)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(Email).ConfigureAwait(false);
            //if (user == null) throw new ArgumentException($"User UserName {Email} không tồn tại");
            return user;
        }
        public async Task<UserDTO?> GetUserDTOByIdAsync(int UserId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(UserId).ConfigureAwait(false);
            //if (user == null) throw new ArgumentException($"User id {UserId} không tồn tại");
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<UserDTO?> GetUserDTOAsync(string Email)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(Email).ConfigureAwait(false);
            //if (user == null) throw new ArgumentException($"User UserName {Email} không tồn tại");
            return _mapper.Map<UserDTO>(user);

        }
        public async Task<object> SignInAsync(UserSignInModel model)
        {
            var user = await GetUserAsync(model.Email).ConfigureAwait(false);
            if (user == null) throw new ArgumentException($"Không có user nào.");

            var checkPassword = _passwordEncoderService.Verify(model.Password, user.Password);
            if (!checkPassword) throw new ArgumentException($"mật khẩu không đúng");

            var userDTO = _mapper.Map<UserDTO>(user);
            var AccessToken = _tokenService.GenerateAccessToken(user);

            var resRefreshToken = await _tokenService.GetRefreshByUserIdAsync(user.UserId);
            if (resRefreshToken.ExpiryDate < DateTime.UtcNow) throw new ArgumentException($"RefreshToken expired");

            return new { User = userDTO, AccessToken, RefreshToken = resRefreshToken.TokenHash };
        }

        public Tuple<bool, string> ValidSignUp(UserSignUpModel model)
        {
            if (string.IsNullOrEmpty(model.UserName)) return Tuple.Create(false, "UserName is not null");
            if (string.IsNullOrEmpty(model.Password)) return Tuple.Create(false, "Password is not null");
            if (string.IsNullOrEmpty(model.Email)) return Tuple.Create(false, "Email is not null");
            if (string.IsNullOrEmpty(model.ConfirmPassword)) return Tuple.Create(false, "ConfirmPassword is not null");
            if (model.Password != model.ConfirmPassword) return Tuple.Create(false, "Password is not equal to ConfirmPassword");

            var isValidEmail = model.Email.ValidateEmailAddress();
            if (!isValidEmail) return Tuple.Create(false, $"Email {model.Email} không hợp lệ");

            if (model.Avatar != null)
            {
                var isFileTuple = _fileService.IsImageFile(model.Avatar);
                if (!isFileTuple.Item1) return Tuple.Create(false, isFileTuple.Item2 ?? "");
            }

            // Find exist 
            var exist = GetUserDTOAsync(model.Email).GetAwaiter().GetResult();
            if (exist != null)
            {
                if (exist.UserName == model.UserName)
                    return Tuple.Create(false, $"UserName {model.UserName} đã tồn tại");
                else if (exist.Email == model.Email)
                    return Tuple.Create(false, $"Email {model.Email} đã tồn tại");
            }

            return Tuple.Create(true, "");
        }
        public async Task<UserDTO> SignUpAsync(UserSignUpModel model)
        {
            try
            {
                var valid = ValidSignUp(model);
                if (!valid.Item1) throw new ArgumentException(valid.Item2);

                Random r = new();
                int randNum = r.Next(1000000);
                string sixDigitNumber = randNum.ToString("D6");

                var user = _mapper.Map<User>(model);
                user.Password = _passwordEncoderService.Encode(model.Password);
                user.RoleId = 1; // default role USER
                user.Status = 0;
                user.Code = sixDigitNumber;
                user.ExpiryCode = DateTime.UtcNow.AddMinutes(10);

                // TODO: upload file 
                if (model.Avatar != null)
                {
                    var resBlob = await _azureStorageService.UploadAsync(model.Avatar);
                    if (resBlob == null) throw new ArgumentException("Up load file fail");
                    user.Avatar = resBlob.Blob.Uri;
                }

                _unitOfWork.UserRepository.Insert(user);
                await _unitOfWork.SaveAsync().ConfigureAwait(false);

                var UserAdded = await GetUserDTOByIdAsync(user.UserId);
                if (UserAdded == null) throw new ArgumentException("Can't find any users");

                var refreshTokenDto = _tokenService.GenerateRefreshToken(UserAdded.UserId);
                RefreshToken refreshToken = _mapper.Map<RefreshToken>(refreshTokenDto);

                _unitOfWork.RefreshTokenRepository.Insert(refreshToken);
                await _unitOfWork.SaveAsync().ConfigureAwait(false);

                var resRefreshToken = await _tokenService.GetRefreshByUserIdAsync(UserAdded.UserId);
                MailContent mail = new()
                {
                    To = model.Email,
                    Subject = "Confirm account",
                    Body = ""
                };

                await _sendMailService.SendMailAsync(mail);

                return UserAdded;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<string> ConfirmAccountAsync(string code, UserDTO userDTO)
        {
            var exist = await GetUserAsync(userDTO.Email);
            if (exist == null) throw new ArgumentException("Can't find any users");
            if (exist.Status != 0) return "Account confirmed";
            if (exist.ExpiryCode < DateTime.UtcNow) return "Code expired";

            exist.Status = 1;
            _unitOfWork.UserRepository.Update(exist);

            return "Confirmed";
        }
    }
}
