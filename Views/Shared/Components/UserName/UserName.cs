using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// Microsoft.AspNet.Identity
using ToDo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class UserName : ViewComponent
    {
        private readonly Context db;
        private readonly HttpContext _http ;
        UserManager<User> userManager ;

        public UserName(Context context, IHttpContextAccessor acessor)
        {
            db = context;
            _http = acessor.HttpContext ;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = db.Users.Single(m => m.Id == _http.User.FindFirstValue(ClaimTypes.NameIdentifier)) ;
            string name = user.FirstName + " " + user.LastName ;
            return View("_UserName", name);
        }
    }