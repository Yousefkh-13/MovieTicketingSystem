using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MovieTicketingSystem.Areas.Identity.Controllers
{
    [Area(AreaConstants.IDENTITY_AREA)]

    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager; // Service layer => UserStore<ApplicationUser>
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;


        public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Index), ControllerConstants.HOME_CONTROLLER, new { area = AreaConstants.CUSTOMER_AREA });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            ApplicationUser user = registerVM.Adapt<ApplicationUser>(/*config*/);

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

                return View(registerVM);
            }

            {
                // Send confirmation mail
                // generate unique token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(Confirm), ControllerConstants.ACCOUNT_CONTROLLER, new { area = AreaConstants.IDENTITY_AREA, user.Id, token }, Request.Scheme);
                string body = $"<h1>Please confirm your account by clicking <b><a href='{link}'>here</a></b></h1>";

                await _emailSender.SendEmailAsync(user.Email!, "Confirm Your Account", body);
            }

            await _userManager.AddToRoleAsync(user, RoleConstants.CUSTOMER);

            TempData[NotificationConstants.SUCCESS_NOTIFICATION] = "Create Account Successfully, please verify your account";

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Confirm(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                TempData[NotificationConstants.ERROR_NOTIFICATION] = String.Join(", ", result.Errors.Select(e => e.Description));
            else
            {
                TempData[NotificationConstants.SUCCESS_NOTIFICATION] = "Confirm Account successfully, please login";
                //await _signInManager.SignInAsync(user, false);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Index), ControllerConstants.HOME_CONTROLLER, new { area = AreaConstants.CUSTOMER_AREA });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailOrUserName) ??
                                    await _userManager.FindByNameAsync(loginVM.EmailOrUserName);

            if (user is null)
            {
                ModelState.AddModelError(nameof(LoginVM.EmailOrUserName), "Invalid User Name or Email");
                ModelState.AddModelError(nameof(LoginVM.Password), "Invalid Password");

                return View(loginVM);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.Remember, lockoutOnFailure: true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError(nameof(LoginVM.EmailOrUserName), "Too many attempts, please try again later");
            }

            if (signInResult.IsNotAllowed)
            {
                ModelState.AddModelError(nameof(LoginVM.EmailOrUserName), "Please verify your account!");
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(nameof(LoginVM.EmailOrUserName), "Invalid User Name or Email");
                ModelState.AddModelError(nameof(LoginVM.Password), "Invalid Password");

                return View(loginVM);
            }

            TempData[NotificationConstants.SUCCESS_NOTIFICATION] = $"Welcome Back {user.FirstName} {user.LastName}";

            return RedirectToAction(nameof(Index), ControllerConstants.HOME_CONTROLLER, new { area = AreaConstants.CUSTOMER_AREA });
        }

        [HttpGet]
        public IActionResult ResendConfirmation()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Index), ControllerConstants.HOME_CONTROLLER, new { area = AreaConstants.CUSTOMER_AREA });

            // generate view

            return View();
        }

        [HttpPost]
        public IActionResult ResendConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            return View();
        }

        public IActionResult ExternalLogin()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData[NotificationConstants.SUCCESS_NOTIFICATION] = "Logout successfully";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Index), ControllerConstants.HOME_CONTROLLER, new { area = AreaConstants.CUSTOMER_AREA });

            return View();
        }

    }

}

