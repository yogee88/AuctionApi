using Identity.Model;
using Identity.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Identity.Repository;

namespace Identity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _token;

        private readonly IAccountRepository _repo;

        public AccountController(ITokenService token, IAccountRepository repo)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromBody]User user)
        {
            try
            {
                var users = await _repo.GetAsync(user.EmailId);
                if (users != null && users.Count == 1)
                {
                    user.Id = users.FirstOrDefault().Id;
                    user.Token = _token.Generate(user);
                    user.IsAuthenticated = true;
                    return Ok(user);
                }
                return StatusCode(401, "User Unauthorized");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "User Unauthorized");
            }
            
        }
    }
}
