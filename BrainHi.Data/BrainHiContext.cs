using BrainHi.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainHi.Data
{
    /// <summary>
    /// Database context of the BrainHi application
    /// </summary>
    public class BrainHiContext : DbContext
    {
        /// <summary>
        /// Initilizes DB context with the provided options
        /// </summary>
        /// <param name="options">DB Context Options</param>
        public BrainHiContext(DbContextOptions<BrainHiContext> options) : base(options) { }

        /// <summary>
        /// Table of <see cref="Provider"/> objects
        /// </summary>
        public DbSet<Provider> Providers { get; set; }

        /// <summary>
        /// Table of <see cref="Appointment"/> objects
        /// </summary>
        public DbSet<Appointment> Appointments { get; set; }
    }
}
