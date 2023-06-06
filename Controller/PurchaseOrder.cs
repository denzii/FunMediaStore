using FluentValidation;
using FunMediaStore.Interface;
using FunMediaStore.Model;
using FunMediaStore.Model.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FunMediaStore.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PurchaseOrder : ControllerBase
    {
        private readonly IPointOfSale _pos;
        private readonly IAuth _auth;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PurchaseOrder(ILogger<PurchaseOrder> logger, IPointOfSale pos, IAuth auth, IHttpContextAccessor httpContextAccessor)
        {
            _pos = pos;
            _auth = auth;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PurchaseRequest request)
        {
            try
            {
                // requests assume a logged in user is making them so a userId is not included in the request but taken from context
                if (_httpContextAccessor.HttpContext!.User.Claims != null)
                {
                    var claims = _httpContextAccessor.HttpContext!.User.Claims;
                    var user = _auth.GetClaimedUser(claims);

                    return Ok(await _pos.ProcessAsync(request.Products, user));
                }

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }
    }
}