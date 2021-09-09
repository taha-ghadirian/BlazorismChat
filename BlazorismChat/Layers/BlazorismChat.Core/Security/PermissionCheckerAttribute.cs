//using BlazzingExam.Core.Server.ServerServices.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;

//namespace BlazzingExam.Core.Server.Security
//{
//    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
//    {
//        private readonly int _permissionId;
//        private IPermissionService _permissionService;

//        public PermissionCheckerAttribute(int permissionId)
//        {
//            _permissionId = permissionId;
//        }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            _permissionService =
//                (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));

//            if (context.HttpContext.User.Identity is {IsAuthenticated: true})
//            {
//                var userName = context.HttpContext.User.Identity.Name;
//                var result = _permissionService != null && _permissionService.IsUserInPermissionAsync(userName, _permissionId).Result;
//                if (!result)
//                {
//                    context.Result = new UnauthorizedResult();
//                }
//            }
//            else
//            {
//                context.Result = new UnauthorizedResult();
//            }
//        }
//    }
//}