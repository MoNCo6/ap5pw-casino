using Microsoft.AspNetCore.Mvc;
using Casino.Web.Areas.Security.Controllers;
using Casino.Web.Controllers;
using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Tests.Security.Account
{
    // Class containing unit tests for AccountController's Login method
    public class AccountControllerLoginTests
    {
        [Fact]
        public async Task Login_ValidSuccess()
        {
            // Arrange
            // Create a mock IAccountService and setup the Login method to always return true
            var mockISecurityApplicationService = new Mock<IAccountService>();
            mockISecurityApplicationService.Setup(security => security.Login(It.IsAny<LoginViewModel>()))
                .Returns(() => Task.FromResult(true));

            // Create a sample valid LoginViewModel
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "a",
                Password = "a"
            };

            // Instantiate the AccountController with the mock service
            AccountController controller = new AccountController(mockISecurityApplicationService.Object);

            // Act
            // Call the Login method and capture the result
            IActionResult iActionResult = await controller.Login(loginViewModel);

            // Assert
            // Verify that the method redirects to the correct action and controller
            RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(iActionResult);
            Assert.Matches(nameof(HomeController.Index), redirect.ActionName);
            Assert.Matches(nameof(HomeController).Replace(nameof(Controller), string.Empty), redirect.ControllerName);
            Assert.Matches(string.Empty, redirect.RouteValues.Single(pair => pair.Key == "area").Value.ToString());
        }

        [Fact]
        public async Task Login_InvalidFailure()
        {
            // Arrange
            // Similar setup as above but with a condition to return false for invalid credentials
            var mockISecurityApplicationService = new Mock<IAccountService>();
            // ... setup mock service

            // Create an invalid LoginViewModel (empty password)
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "a",
                Password = ""
            };

            // Instantiate the AccountController with the mock service
            AccountController controller = new AccountController(mockISecurityApplicationService.Object);

            // Manually add a model state error to simulate invalid model state
            controller.ModelState.AddModelError(nameof(LoginViewModel.Password),
                $"{nameof(LoginViewModel.Password)} was not set");

            // Act
            // Call the Login method and capture the result
            IActionResult iActionResult = await controller.Login(loginViewModel);

            // Assert
            // Verify that the method returns a ViewResult with the original model
            ViewResult viewResult = Assert.IsType<ViewResult>(iActionResult);
            Assert.NotNull(viewResult.Model);
            LoginViewModel? loggingVM = viewResult.Model as LoginViewModel;
            Assert.NotNull(loggingVM);
            Assert.NotNull(loggingVM.Username);
            Assert.Matches(loginViewModel.Username, loggingVM.Username);
            Assert.NotNull(loggingVM.Password);
            Assert.Matches(loginViewModel.Password, loggingVM.Password);
        }

        [Fact]
        public async Task LoginValidation_InvalidFailure()
        {
            // Arrange
            // Setup mock service and create a LoginViewModel with invalid data (empty password)
            var mockISecurityApplicationService = new Mock<IAccountService>();
            // ... setup mock service

            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "a",
                Password = ""
            };

            AccountController controller = new AccountController(mockISecurityApplicationService.Object);

            // Set ObjectValidator for testing model validation
            controller.ObjectValidator = new ObjectValidator(false);

            // Act
            // Manually trigger model validation and call the Login method
            controller.TryValidateModel(loginViewModel);
            IActionResult iActionResult = await controller.Login(loginViewModel);

            // Assert
            // Verify that the method returns a ViewResult with the original model
            ViewResult viewResult = Assert.IsType<ViewResult>(iActionResult);
            Assert.NotNull(viewResult.Model);
            LoginViewModel? loggingVM = viewResult.Model as LoginViewModel;
            Assert.NotNull(loggingVM);
            Assert.NotNull(loggingVM.Username);
            Assert.Matches(loginViewModel.Username, loggingVM.Username);
            Assert.NotNull(loggingVM.Password);
            Assert.Matches(loginViewModel.Password, loggingVM.Password);
        }
    }
}