using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHunt;
using ImageHunt.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using NFluent;
using Xunit;

namespace ImageHuntTest
{
    public class TokenAuthorizationHandlerTest : ContextBasedTest
    {
        private TokenAuthorizationHandler _target;

        public TokenAuthorizationHandlerTest()
        {
            _target = new TokenAuthorizationHandler(_context);
        }
        [Fact]
        public async Task NoAuthorizationInHeader()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
            var filterMetaData = A.Fake<IFilterMetadata>();
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new[] { filterMetaData });
            var authorizationRequirement = A.Fake<IAuthorizationRequirement>();

            var user = new ClaimsPrincipal();
            var context = new AuthorizationHandlerContext(new[] { authorizationRequirement }, user, authorizationFilterContext);
            // Act
            await _target.HandleAsync(context);
            // Assert
        }
        [Fact]
        public void HandleAsync_GrantAccess()
        {
            _context.Admins.Add(new Admin() { Token = "toto", ExpirationTokenDate = DateTime.Now.AddHours(1) });
            _context.SaveChanges();
            var target = new TokenAuthorizationHandler(_context);

            var httpContext = new DefaultHttpContext() { Request = { Headers = { { "Authorization", "Bearer toto" } } } };
            var routeData = new RouteData();
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
            var filterMetaData = A.Fake<IFilterMetadata>();
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new[] { filterMetaData });
            var authorizationRequirement = A.Fake<IAuthorizationRequirement>();

            var user = new ClaimsPrincipal();
            var context = new AuthorizationHandlerContext(new[] { authorizationRequirement }, user, authorizationFilterContext);
            var result = target.HandleAsync(context);
            Check.That(context.HasSucceeded).IsTrue();
            //Check.That(context.User).IsNotNull();
        }
        [Fact]
        public void HandleAsync_DenyAccess()
        {
            var target = new TokenAuthorizationHandler(_context);

            var httpContext = new DefaultHttpContext() { Request = { Headers = { { "Authorization", "Bearer toto" } } } };
            var routeData = new RouteData();
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
            var filterMetaData = A.Fake<IFilterMetadata>();
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new[] { filterMetaData });
            var authorizationRequirement = A.Fake<IAuthorizationRequirement>();

            var user = new ClaimsPrincipal();
            var context = new AuthorizationHandlerContext(new[] { authorizationRequirement }, user, authorizationFilterContext);
            target.HandleAsync(context);
            Check.That(context.HasSucceeded).IsFalse();

        }
    }
}
