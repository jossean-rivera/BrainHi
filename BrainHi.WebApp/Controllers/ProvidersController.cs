using BrainHi.Data;
using BrainHi.Models;
using BrainHi.WebApp.ViewModels.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainHi.WebApp.Controllers
{
    [Route("/providers")]
    public class ProvidersController : Controller
    {
        //  Application repository of the application
        private readonly IApplicationRepository _repository;

        //  Repository will be provided by dependency injection
        public ProvidersController(IApplicationRepository repository) => _repository = repository;

        //  Displays view with a list of providers
        [HttpGet]
        public async Task<ViewResult> List(string query = null)
        {
            //  Query all providers from repository
            IEnumerable<Provider> providers = await _repository.GetEntitiesAsync<Provider>();

            //  Filter providers to display depending on the provided parameters
            IEnumerable<Provider> filterProviders = providers;

            if (!string.IsNullOrEmpty(query))
            {
                //  Filter by name or specialty that contains the provided query (use non case sensitive contains operation)
                filterProviders = filterProviders.Where(provider => provider.ProviderFullName.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                    provider.Specialty.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            return View(model: (providers, filterProviders, query));
        }

        //  Displays view with details of a specific provider
        [HttpGet("{id}")]
        public async Task<ViewResult> Display(int? id)
        {
            //  ID is required
            if (!id.HasValue) return View("NotFound");

            //  Get provider from repository
            Provider provider = await _repository.GetQueryBuilder<Provider>()
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.ProviderId == id.Value);

            //  Return view result
            return View(new ProviderDisplayViewModel { Provider = provider });
        }

        //  Post action to save an appointment
        [HttpPost("book-appointment")]
        public async Task<IActionResult> Book(ProviderDisplayViewModel inputModel)
        {
            //  Query provider
            Provider provider = await _repository.GetQueryBuilder<Provider>()
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.ProviderId == inputModel.ProviderId);

            //  The provider is required
            if (provider == null) return View("NotFound");

            //  Display any errors
            if (!ModelState.IsValid) return View(nameof(Display), new ProviderDisplayViewModel { Provider = provider });

            //  Save appointment (assuming that the end datetime is 1 hour after the selected date)
            Appointment appointment = await _repository.AddEntityAsync(new Appointment
            {
                ProviderId = inputModel.ProviderId,
                AppointmentReason = inputModel.AppointmentReason,
                EndTime = new DateTime(inputModel.Year.Value, inputModel.Month.Value, inputModel.Day.Value, inputModel.Hour.Value + 1, inputModel.Minute.Value, 0),
                StartTime = new DateTime(inputModel.Year.Value, inputModel.Month.Value, inputModel.Day.Value, inputModel.Hour.Value, inputModel.Minute.Value, 0),
                PatientDoB = new DateTime(inputModel.PatientDoBYear.Value, inputModel.PatientDoBMonth.Value, inputModel.PatientDoBDay.Value),
                PatientFullName = inputModel.PatientFullName,
                PatientGender = inputModel.PatientGender,
                PatientPhoneNumber = inputModel.PatientPhoneNumber
            });

            //  Temporarily save the Id of appointment
            TempData[nameof(Appointment.AppointmentId)] = appointment.AppointmentId;

            //  Redirect to list page
            return RedirectToAction(nameof(BookConfirm));
        }

        //  Displays page with successful message about the appointment
        [HttpGet("appointment-confirm")]
        public async Task<ViewResult> BookConfirm()
        {
            //  Get Id of new appointment
            int? appointmentId = TempData[nameof(Appointment.AppointmentId)] as int?;

            //  ID is required
            if (!appointmentId.HasValue) return View("NotFound");

            //  Query appointment
            Appointment appointment = await _repository.GetQueryBuilder<Appointment>()
                .Include(ap => ap.Provider)
                .FirstOrDefaultAsync(ap => ap.AppointmentId == appointmentId.Value);

            //  Return view result to display view about the confirmation of the appointment
            return View(appointment);
        }

        //  Displays form to create a new provider
        [HttpGet("new")]
        public ViewResult Create() => View();

        //  Post action to create a new provider
        [HttpPost("new")]
        public async Task<IActionResult> Create([Bind(nameof(Provider.ProviderFullName), nameof(Provider.Specialty))] Provider provider)
        {
            //  Display error messages
            if (!ModelState.IsValid) return View(provider);

            //  Add provider to repository
            await _repository.AddEntityAsync(provider);

            //  Redirect to confirm page
            return RedirectToAction(nameof(CreateConfirm));
        }

        //  Displays page with successful message about creating a new provider
        [HttpGet("confirm")]
        public ViewResult CreateConfirm() => View();
    }
}
