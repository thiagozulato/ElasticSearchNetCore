using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchDotNet
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        public readonly IElasticRepository<User> _userElasticRepository;
        public UserController(IElasticRepository<User> userElasticRepository)
        {
            _userElasticRepository = userElasticRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(Api.Created(await _userElasticRepository.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return Ok(await _userElasticRepository.GetById(id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody]User user)
        {
            user.CreateId();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userElasticRepository.Add(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User user)
        {
            await _userElasticRepository.Update(id.ToString(), user);

            return Ok(Api.Created(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deletedUser = await _userElasticRepository.Delete(id.ToString());

            return Ok(Api.Created(deletedUser));
        }
    }

    public class User
    {
        [Required(ErrorMessage = "Invalid UserID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User birthdate is required")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "User document is required")]
        public string Document { get; set; }

        [Required(ErrorMessage = "User country is required")]
        public string Country { get; set; }

        public void CreateId()
        {
            Id = Guid.NewGuid();
        }
    }

    public class Api
    {
        public static ApiResponse<T> Created<T>(T value)
        {
            return new ApiResponse<T>(value);
        }

        public static ApiResponse<Object> CreateInvalidResource(int code, string message)
        {
            return new ApiResponse<Object>(code, message);
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; private set; }
        public int Code { get; private set; } = 0;
        public string Message { get; private set; } = string.Empty;

        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}