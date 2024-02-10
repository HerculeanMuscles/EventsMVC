using CloudinaryDotNet.Actions;
using EventsWebApp.Data;
using EventsWebApp.Interfaces;
using EventsWebApp.Models;
using EventsWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.Description = editVM.Description;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.State = editVM.State;
            user.City = editVM.City;
            user.Street = editVM.Street;
        }

        public async Task<IActionResult> Index()
        {
            var userEvents = await _dashboardRepository.GetAllUserEvents();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Events = userEvents,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Description = user.Description,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State,
                Street = user.Street,
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);
            }

            AppUser user = await _dashboardRepository.GetByIdNoTracking(editVM.Id);

            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashboardRepository.Update(user);

                return RedirectToAction("Index", "Dashboard");
            } 
            else
            {
                try 
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                } 
                catch (Exception ex) 
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashboardRepository.Update(user);

                return RedirectToAction("Index", "Dashboard");
            }
        }

    }
}
