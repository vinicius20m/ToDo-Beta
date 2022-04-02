using System.Globalization;
using ToDo.Areas.Identity.Data;
using ToDo.Models ;
using Microsoft.EntityFrameworkCore;

// namespace Middleware.Example;

public class Auth
{

    public static string connectionString = "" ;

    private readonly RequestDelegate _next;

    public Auth(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        var optionsBuilder = new DbContextOptionsBuilder<Context>()
            .UseSqlServer(Auth.connectionString)
        ;
        Context dbCtx = new Context(optionsBuilder.Options) ;

        
        context.Request
            .Headers.Add(
                "Date-Time",
                DateTime.Now.ToString()
            )
        ;

        var user = context.User ;
        var request = context.Request ;
        var method = context.Request.Method  ;

        //get the user
        var User = await dbCtx.Users.FirstOrDefaultAsync(m => m.UserName == user.Identity.Name) ;

        request.RouteValues.TryGetValue("action", out var action) ;
        request.RouteValues.TryGetValue("controller", out var controller) ;
        var route = controller?.ToString() +"/"+ action?.ToString() ;
        var path = request.Path.Value ;

        if(User == null){

            context.Request
                .Headers.Add(
                    "User-Auth",
                    "false"
                ) 
            ;

            if(
                path != "/Identity/Account/Login" 
                && path != "/Identity/Account/RegisterConfirmation" 
                && path != "/Identity/Account/Register" 
                && path != "/Identity/Account/ConfirmEmail"
            )
                context.Response.Redirect("/Identity/Account/Login")
            ;
        }
        else{

            var headers = context.Request.Headers ;
            headers.Add(
                "user-auth",
                "true"
            );

            headers.Add(
                "user-id",
                User.Id
            );

            headers.Add(
                "user-name",
                user?.Identity?.Name
            );

            if(context.Session.GetString("token") == null){

                string token = Guid.NewGuid().ToString() ;
                context.Session.SetString("token", token) ;

                User.ApiToken = token ;
                dbCtx.Users.Attach(User) ;
                dbCtx.Entry(User).Property(p => p.ApiToken).IsModified = true ;
                dbCtx.SaveChanges() ;
            }


            if(route == "Show/Index"){

                request.Query.TryGetValue("id", out var query) ;
                if(query.Count > 0){

                    string queryValue = query.Single() ;

                    var ambienteId = Int32.Parse(queryValue) ;
                    if(
                        dbCtx.Ambientes.Single(m => 
                            m.Id == ambienteId
                        ).UserId != User.Id
                    )
                        context.Response.Redirect("/Ambientes/Index") 
                    ;
                }
            }
        }
        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}

public static class AuthExtensions
{
    public static IApplicationBuilder UseAuth(
        this IApplicationBuilder builder, string connectionString)
    {

        Auth.connectionString = connectionString ;
        return builder.UseMiddleware<Auth>();
    }
}