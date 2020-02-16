using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;

namespace ElasticSearchDotNet
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        public readonly IElasticRepository<User> _userElasticRepository;
        public readonly ILogger<UserController> _logger;

        public UserController(IElasticRepository<User> userElasticRepository,
                              ILogger<UserController> logger)
        {
            _userElasticRepository = userElasticRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all users");

            return Ok(Api.Created(await _userElasticRepository.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            _logger.LogInformation($"Getting the User by Id {id}");

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

            _logger.LogInformation("New user was added successfully.");

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User user)
        {
            await _userElasticRepository.Update(id.ToString(), user);

            _logger.LogInformation("User has been updated successfully.");

            return Ok(Api.Created(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userElasticRepository.Delete(id.ToString());

            _logger.LogInformation("User has been successfully deleted.");

            return NoContent();
        }
    }
}