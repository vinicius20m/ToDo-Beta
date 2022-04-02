using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// Microsoft.AspNet.Identity
using ToDo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class UserAmbientes : ViewComponent
    {
        private readonly Context db;
        private readonly HttpContext _http ;
        UserManager<User> userManager ;

        public UserAmbientes(Context context, IHttpContextAccessor acessor)
        {
            db = context;
            _http = acessor.HttpContext ;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var items =db.Ambientes.Where(m => m.UserId == _http.User.FindFirstValue(ClaimTypes.NameIdentifier) )
                .ToList();
            return View("_UserAmbientes",items);
        }
    }