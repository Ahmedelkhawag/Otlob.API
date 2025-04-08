using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Otlob.API.DTOs;
using Otlob.API.Errors;
using Otlob.API.ExtensionMethods;
using Otlob.Core.Models;
using Otlob.Core.Services;
using System.Security.Claims;

namespace Otlob.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ITokenServices tokenServices, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices; // Add This Service in IdentityServicesExtension Class
            _mapper = mapper;
        }

        #region EndPoints

        #region POST: BaseUrl/api/Account/Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(new ErrorResponse(400));

            var registeredUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };

            return Ok(registeredUser);
        }
        #endregion

        #region POST: BaseUrl/api/Account/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return Unauthorized(new ErrorResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ErrorResponse(401));

            var loginUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };

            return Ok(loginUser);
        }
        #endregion

        #region  GET: BaseUrl/api/Account/CurrentUser
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null)
                return Unauthorized(new ErrorResponse(401));
            var returnedUser = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };
            return Ok(returnedUser);
        }
        #endregion

        #region  GET: BaseUrl/api/Account/CurrentUserAddress

        [Authorize]
        [HttpGet("currentUserAddress")]
        public async Task<IActionResult> GetcurrentUSerWithAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user is null)
                return Unauthorized(new ErrorResponse(401));
            var mappedUserAddress = _mapper.Map<AddressDto>(user.Address);
            return Ok(mappedUserAddress);

        }

        #endregion
        #region PUT: BaseUrl/api/Account/Address
        [Authorize]
        [HttpPut("Address")]
        public async Task<IActionResult> UpdateAddress(AddressDto addressDto)
        {
          var user = await _userManager.FindUserWithAddressAsync(User);
            if (user is null)
                return Unauthorized(new ErrorResponse(401));
            var address = _mapper.Map<AddressDto , Address>(addressDto);
            address.Id = user.Address.Id;
            user.Address = address;
            var updatedAddress = await _userManager.UpdateAsync(user);
            if (!updatedAddress.Succeeded)
            {
                return BadRequest(new ErrorResponse(400));
            }
            else
                return Ok(updatedAddress);

        }
        #endregion

        #region  GET: BaseUrl/api/Account/EmailExists

       // [Authorize]
        [HttpGet("emailExists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
           var returnedUser = new UserDto
           {
               DisplayName = user.DisplayName,
               Email = user.Email,
               Token = await _tokenServices.CreateTokenAsync(user, _userManager)
           };
            return user != null
                ? Ok(new { emailExists = true, returnedUser })
                : Ok(new { emailExists = false });

        }

        #endregion
        #endregion
    }
}
