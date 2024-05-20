using Application.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.WebMVC.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		#region data member
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		#endregion

		public AccountController(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		[AllowAnonymous]
		public IActionResult Login(string ReturnUrl = "")
		{
			_signInManager.SignOutAsync();
			TempData["ReturnUrl"] = ReturnUrl;
			return View();
		}

		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user != null)
			{
				var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
				if (result.Succeeded)
				{
					var returnUrl = TempData["ReturnUrl"] as string;
					if (!string.IsNullOrEmpty(returnUrl))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return Redirect("/account/users");
					}

				}
				ModelState.AddModelError(string.Empty, "password is not correct!");
				return View(model);
			}
			ModelState.AddModelError(string.Empty, "email is not correct");
			return View(model);
		}
		public IActionResult Logout()
		{
			_signInManager.SignOutAsync();
			return Redirect("/account/login");
		}

		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return View();
		}

		#region Role

		[Authorize(Roles = "admin")]
		public IActionResult Roles()
		{
			var roles = _roleManager.Roles.ToList();
			return View(roles);
		}

		[Authorize(Roles = "admin")]
		public IActionResult CreateRole()
		{
			return View();
		}

		[Authorize(Roles = "admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var role = new IdentityRole
			{
				Name = model.RoleName
			};
			var result = await _roleManager.CreateAsync(role);
			if (result.Succeeded)
			{
				return Redirect("/Account/Roles");
			}
			ModelState.AddModelError(string.Empty, GetErrorMessage(result));
			return View(model);
		}
		#endregion


		#region User

		public IActionResult Users()
		{
			var users = _userManager.Users.ToList();
			return View(users);
		}
		public IActionResult CreateUser()
		{
			var roles = _roleManager.Roles.ToList();
			ViewBag.Roles = roles;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateUser(CreateUserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = new IdentityUser
			{
				UserName = model.UserName,
				Email = model.Email,
			};
			var userResult = await _userManager.CreateAsync(user, model.Password);
			if (userResult.Succeeded)
			{
				var role = await _roleManager.FindByIdAsync(model.RoleId);
				var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
				if (roleResult.Succeeded)
				{
					return Redirect("/Account/users");
				}
				else
				{
					await _userManager.DeleteAsync(user);
					ModelState.AddModelError(string.Empty, GetErrorMessage(roleResult));
					return View(model);
				}
			}

			ModelState.AddModelError(string.Empty, GetErrorMessage(userResult));
			return View(model);
		}
		#endregion
		private string GetErrorMessage(IdentityResult result)
		{
			if (result.Errors.Any())
			{
				var errorMessage = string.Join(" ", result.Errors.Select(x => x.Description).ToList());
				return errorMessage;
			}
			return string.Empty;
		}
	}


}



