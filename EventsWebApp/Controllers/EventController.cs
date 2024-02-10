using EventsWebApp.Data;
using EventsWebApp.Interfaces;
using EventsWebApp.Models;
using EventsWebApp.Services;
using EventsWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EventController(IEventRepository eventRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _eventRepository = eventRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Event> events = await _eventRepository.GetAll();
            return View(events);
        }

        public async Task<IActionResult> DetailAsync(int id)
        {
            Event events = await _eventRepository.GetByIdAsync(id);
            return View(events);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var eventDetails = await _eventRepository.GetByIdAsync(id);
            if (eventDetails == null) return View("Error");
            return View(eventDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var eventDetails = await _eventRepository.GetByIdAsync(id);
            if (eventDetails == null) return View("Error");

            _eventRepository.Delete(eventDetails);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createEventViewModel = new CreateEventViewModel { AppUserId = curUserId };
            return View(createEventViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEventViewModel eventVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(eventVM.Image);

                var uevent = new Event
                {
                    AppUserId = eventVM.AppUserId,
                    Title = eventVM.Title,
                    Description = eventVM.Description,
                    Image = result.Url.ToString(),
                    EventCategory = eventVM.EventCategory,
                    Address = new Address
                    {
                        City = eventVM.Address.City,
                        State = eventVM.Address.State,
                        Street = eventVM.Address.Street
                    }
                };
                _eventRepository.Add(uevent);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }
            return View(eventVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var uevent = await _eventRepository.GetByIdAsync(id);
            if (uevent == null) return View("Error");
            var eventVM = new EditEventViewModel
            {
                Title = uevent.Title,
                Description = uevent.Description,
                AddressId = uevent.AddressId,
                Address = uevent.Address,
                URL = uevent.Image,
                EventCategory = uevent.EventCategory,
            };
            return View(eventVM);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditEventViewModel eventVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Club");
                return View("Edit", eventVM);
            }

            var userClub = await _eventRepository.GetByIdAsyncNoTracks(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(eventVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(eventVM.Image);

                var uevent = new Event
                {
                    Id = id,
                    Title = eventVM.Title,
                    Description = eventVM.Description,
                    Image = photoResult.Url.ToString(),
                    EventCategory = eventVM.EventCategory,
                    AddressId = eventVM.AddressId,
                    Address = eventVM.Address,
                };

                _eventRepository.Update(uevent);

                return RedirectToAction("Index");
            }
            else
            {
                return View(eventVM);
            }
        }

    }
}
