using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AgentyTask.Repository;

namespace AgentyTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentyJobApplicationController : ControllerBase
    {
        private MyResumeRepository _repository { get; }
        private readonly ILogger _logger;
        public AgentyJobApplicationController(MyResumeRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Post([FromBody] MyResume resume)
        {
            _logger.LogInformation("User Service Post method getting started...");
            if (String.IsNullOrEmpty(resume.Name))
                throw new ArgumentException("Name cannot be empty");
            if (String.IsNullOrEmpty(resume.Email))
                throw new ArgumentException("Email cannot be empty");
            if (String.IsNullOrEmpty(resume.Phone))
                throw new ArgumentException("Phone cannot be empty");

            var isEmailExist = await _repository.GetByEmail(resume.Email);
            if (isEmailExist == null)
            {
                await _repository.Create(resume);
                return Ok("Resume Id: " + resume.Id);
            }
            else throw new ArgumentException("Resume already exist");
        }

        [HttpGet("{id}")]
        public async Task<MyResume> GetById(string id)
        {
            var user = await _repository.GetById(id);
            if (user != null)
                return user;
            else
                throw new ArgumentException("Please input correct Id.");
        }
    }
}