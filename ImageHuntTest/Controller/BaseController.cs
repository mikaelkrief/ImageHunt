using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ImageHunt.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class BaseControllerTest
    {
        private DummyController _target;

        public BaseControllerTest()
        {
            _target = new DummyController();
            
        }

        [Fact]
        public void GetUserId()
        {
            // Arrange
            _target.ControllerContext = new ControllerContext(){HttpContext = new DefaultHttpContext()};
            _target.User.AddIdentity(new ClaimsIdentity(new []{new Claim("userId", "15"), }));
            // Act
            var result = _target.UserId;
            // Assert
            Check.That(result).Equals(15);
        }
    }

    public class DummyController : BaseController
    {
    }

}
