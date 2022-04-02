using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http ;
using ToDo.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Controllers
{
    [Route("[controller]")]
    public class ShowController : Controller
    {
        private readonly ILogger<ShowController> _logger;
        private readonly HttpContext _http ;
        private readonly Context _context ;

        public ShowController(ILogger<ShowController> logger, IHttpContextAccessor accessor, Context context)
        {
            _logger = logger;
            _http = accessor.HttpContext ;
            _context = context ;
        }


        [HttpGet]
        [Route("Index")]
        public IActionResult Index(int id)
        {

            _http.Request.Headers.TryGetValue("user-id", out var userId) ;
            _http.Request.Headers.TryGetValue("user-auth", out var auth) ;
            if(auth.ToString() == "true"){

                ViewData["AuthId"] = userId.ToString() ;
                ViewData["AmbienteId"] = id ;
                ViewData["AmbienteTitle"] = _context.Ambientes.Single(m => m.Id == id).Title ;
                
                var token = HttpContext.Session.GetString("token") ?? string.Empty ;
                ViewData["ApiToken"] = token ;
            }
        

            return View();
        }

        [HttpGet]
        [Route("Permissoes")]
        public IActionResult Permissoes()
        {

            _http.Request.Headers.TryGetValue("user-id", out var userId) ;
            _http.Request.Headers.TryGetValue("user-auth", out var auth) ;
            if(auth.ToString() == "true"){

            }
                var permissoes = _context.Quadro_Permissions.Include(a => a.PermissionType) ;

            return Ok(permissoes) ;
        }

        [HttpGet]
        [Route("TestReact")]
        public IActionResult TestReact()
        {

            return View() ;
        }


        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}