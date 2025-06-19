using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.ViewModels;
using OsiguranjeVozila.Repositories;

namespace OsiguranjeVozila.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List() //vraca listu svih zaposlenih
        {
            var users = await userRepository.GetAll();

            var usersViewModel = new UserViewModel();

            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    Email = user.Email
                });
            }

            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)  //kreira novog zaposlenog
        {



            var existingUsername = await userManager.FindByNameAsync(request.Username);
            var existingEmail = await userManager.FindByEmailAsync(request.Email);

            if (existingUsername != null)
            {
                ModelState.AddModelError("Username", "Korisničko ime već postoji");
                return View(request);
            }
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email adresa već postoji");
                return View(request);
            }

            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email

            };



            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult != null)
            {
                if (identityResult.Succeeded)
                {
                    var roles = new List<string> { "Zaposleni" };

                    if (request.Admin)
                    {
                        roles.Add("Administrator");
                    }

                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult != null && identityResult.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Prodaja je ažurirana!";
                        return RedirectToAction("List", "AdminUsers");
                    }
                    


                }
                
            }

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) //brise zaposlenog
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult != null && identityResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "Prodja je obrisana!";
                    return RedirectToAction("List", "AdminUsers");
                    
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) //pronalazi zaposlenog na osnovu id-ja i popunjava formu s njim
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                UserViewModel userViewModel = new UserViewModel
                {
                    Username = user.UserName,
                    Email = user.Email,

                };
                return View(userViewModel);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel userViewModel) //azurira zaposlenog
        {
            var identityUser = await userManager.FindByIdAsync(userViewModel.Id.ToString());

            if (identityUser == null)
            {
                return NotFound();
            }

            identityUser.Email = userViewModel.Email;
            identityUser.UserName = userViewModel.Username;


            var identityResult = await userManager.UpdateAsync(identityUser);

            if (identityResult != null && identityResult.Succeeded)
            {
                return RedirectToAction("List", "AdminUsers");
            }


            return View(userViewModel);

        }
    }
}
