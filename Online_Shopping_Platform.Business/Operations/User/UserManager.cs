using Online_Shopping_Platform.Business.DataProtection;
using Online_Shopping_Platform.Business.Operations.User.Dtos;
using Online_Shopping_Platform.Business.Types;
using Online_Shopping_Platform.Data.Entities;
using Online_Shopping_Platform.Data.Repositories;
using Online_Shopping_Platform.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;  // Unit of work to manage transactions
        private readonly IRepository<UserEntity> _userRepository;  // Repository to interact with User entities
        private readonly IDataProtection _protector;  // Data protection service for encrypting/decrypting sensitive data

        // Constructor to inject dependencies
        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, IDataProtection protector)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _protector = protector;
        }

        // Method to add a new user
        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            // Check if the email already exists
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email Address already exists!"  // Return error message if email exists
                };
            }

            // Create a new user entity
            var userEntity = new UserEntity
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _protector.Protect(user.Password),  // Protect password before saving
                PhoneNumber = user.PhoneNumber,
                Role = Role.Customer  // Default user role set to Customer
            };

            _userRepository.Add(userEntity);  // Add user entity to the repository

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Commit changes to the database
            }
            catch (Exception)
            {
                throw new Exception("An error occurred during user registration.");  // Handle potential errors during registration
            }

            return new ServiceMessage
            {
                IsSucceed = true,  // Return success if registration is successful
            };
        }

        // Method to log in a user
        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            // Find user by email
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Username or password is incorrect."  // Return error if user not found
                };
            }

            // Unprotect the stored password and compare with input
            var unprotectedPassword = _protector.UnProtect(userEntity.Password);

            if (unprotectedPassword == user.Password)  // If passwords match, return user data
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        Role = userEntity.Role,  // Return user info upon successful login
                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Username or password is incorrect."  // Return error if passwords don't match
                };
            }
        }
    }
}
