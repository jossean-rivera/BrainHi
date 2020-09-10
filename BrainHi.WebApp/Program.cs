using System;
using System.Linq;
using System.Threading.Tasks;
using BrainHi.Data;
using BrainHi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrainHi.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //  SEED DATABASE BEFORE RUNNING THE WEB APPLICATION
            //  Create the host using the Startup class
            IHost host = CreateHostBuilder(args).Build();

            //  In order to seed the datase, we need the IApplicationRepository which is a scoped services,
            //  therefore, start by creating a scope
            IServiceScope scope = host.Services.CreateScope();

            //  Get the dependency injection provider from the scope
            IServiceProvider provider = scope.ServiceProvider;

            //  Get the IApplication Repository
            IApplicationRepository repository = provider.GetRequiredService<IApplicationRepository>();

            //  Seed the database
            await SeedDatabaseAsync(repository);

            //  Dispose the scope to release memory
            scope.Dispose();

            //  Run the web application host
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //  Makes sure that the database is populated with testing data
        private static async Task SeedDatabaseAsync(IApplicationRepository repository)
        {
            if (!await repository.GetQueryBuilder<Provider>().AnyAsync())
            {
                //  Add some testing providers
                await repository.AddEntitiesAsync(TestProviders);

                //  Set the ID of the providers
                foreach (Appointment a in TestAppointments)
                {
                    a.ProviderId = TestProviders.First().ProviderId;
                }

                //  Add some testing appointments
                await repository.AddEntitiesAsync(TestAppointments);
            }
        }

        #region Test Data
        //  Test providers to seed the database
        private static Provider[] TestProviders = new[]
        {
            new Provider{ ProviderFullName = "Dr. Martin Martinez", Specialty = "Internal Medicine" },
            new Provider{ ProviderFullName = "Dr. Richard Rodriguez", Specialty = "Internal Medicine" },
            new Provider{ ProviderFullName = "Dra. Maria M. Perez", Specialty = "Cardiology" },
            new Provider{ ProviderFullName = "Dra. Franchesca Cruz", Specialty = "Gastroenterology" },
            new Provider{ ProviderFullName = "Dra. Emma Rivera", Specialty = "Psychiatry" },
            new Provider{ ProviderFullName = "Dr. Hector Rivera", Specialty = "Neurology" },
            new Provider{ ProviderFullName = "Dr. Ricardo Miranda", Specialty = "Paediatric" },
            new Provider{ ProviderFullName = "Dr. Edgar Falcon", Specialty = "Paediatric" },
            new Provider{ ProviderFullName = "Dr. Arturo Delgado", Specialty = "Paediatric" },
            new Provider{ ProviderFullName = "Dra. Kiara Santiago", Specialty = "Cardiology" },
        };

        //  Test data for seeding database
        private static Appointment[] TestAppointments = new[]
        {
            new Appointment
            { 
                AppointmentReason = "Cough syntoms", 
                StartTime = DateTime.Now.AddDays(1), 
                EndTime = DateTime.Now.AddDays(1).AddHours(1), 
                PatientFullName = "Jossean Rivera" ,
                PatientDoB = new DateTime(year: 2000, month: 1, day: 1),
                PatientGender = PatientGender.Male,
                PatientPhoneNumber = "505-555-97410"
            },
            new Appointment
            {
                AppointmentReason = "Fever syntoms",
                StartTime = DateTime.Now.AddDays(2),
                EndTime = DateTime.Now.AddDays(2).AddHours(1),
                PatientFullName = "Yary Miranda" ,
                PatientDoB = new DateTime(year: 1978, month: 1, day: 1),
                PatientGender = PatientGender.Female,
                PatientPhoneNumber = "505-555-1523"
            },
            new Appointment
            {
                AppointmentReason = "Cough syntoms",
                StartTime = DateTime.Now.AddDays(4),
                EndTime = DateTime.Now.AddDays(4).AddHours(1),
                PatientFullName = "Jezziel Rivera" ,
                PatientDoB = new DateTime(year: 2000, month: 1, day: 1),
                PatientGender = PatientGender.Male,
                PatientPhoneNumber = "505-555-7894"
            }
        };
        #endregion
    }
}
