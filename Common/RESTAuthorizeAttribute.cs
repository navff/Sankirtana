using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Filters;
using Sankirtana.Web.Business.PortalUsers;

namespace Sankirtana.Web.Common;

/// <summary>
    /// Аффтаризовывает пользователя по токену в http-заголовке «Authorization»
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RESTAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private string[] _roles;


        /// <summary>
        /// Позволяет заходить всем зарегистрированным пользователям
        /// </summary>
        //public RESTAuthorizeAttribute() : this(Role.PortalAdmin, Role.PortalManager, Role.RegisteredUser)
        //{
        //}

        /// <summary>
        /// Аффтаризация. Принимает массив ролей в строках
        /// </summary>
        /// <param name="roles"></param>
        public RESTAuthorizeAttribute(params  string[] roles)
        {
            this._roles = roles;
        }

        /// <summary>
        /// /// Аффтаризация. Принимает массив ролей в енумах
        /// </summary>
        /// <param name="ro/les"></param>
       // public RESTAuthorizeAttribute(params Role[] roles)
        // {
            // var rolesList = new List<string>();
            // foreach (Role role in roles)
            // {
                // rolesList.Add(role.ToString());
            // }
            // this._roles = rolesList.ToArray();
        // }


        /// <summary>
        /// Вызывается каждый раз при поступлении запроса
        /// </summary>
        /// <param name="actionContext"></param>
        /*public override void OnAuthorization(HttpActionContext actionContext)
        {
            var token = actionContext.Request.Headers.Authorization?.Parameter;
            if (token == null)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("Token is null")
                };
                return;
            }
            using (var context = new HobbyContext())
            {
                var userOperations = new UserOperations(context);
                var user = userOperations.GetUserByToken(token);

                if (user == null)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("User with this token is not found")
                    };
                    return;
                }

                if (_roles != null)
                {
                    if (!_roles.Contains(user.Role.ToString()))
                    {
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                        {
                            Content = new StringContent("Your role is too small")
                        };
                        return;
                    }
                }

                var identity = new Identity { Name = user.Email, IsAuthenticated = true };
                actionContext.RequestContext.Principal = new GenericPrincipal(identity, new[] { user.Role.ToString() });
            }
            
        }*/

        public void Dispose()
        {
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userService = context.HttpContext.RequestServices.GetService<PortalUserService>();
            var user = await userService.GetById("63005e8fbe776597bb4b48b8");
            context.HttpContext.User = new GenericPrincipal(new Identity() {Name = user.Name, IsAuthenticated = true, Id = user.Id.ToString()}, new []{user.Role});
        }
    }