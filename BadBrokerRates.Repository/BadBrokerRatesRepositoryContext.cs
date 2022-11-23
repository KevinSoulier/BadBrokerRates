using BadBrokerRates.Repository.DBModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BadBrokerRates.Repository
{
    public class BadBrokerRatesRepositoryContext: DbContext
    {        
        public DbSet<CurrencyRates> CurrencyRates { get; set; }

        /*public string DbPath { get; private set; }

        public BadBrokerRatesRepositoryContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // or: Directory.GetCurrentDirectory() gives the same result

            System.IO.Path.Join(Environment.CurrentDirectory, "BadBrokerRates.db");

            DbPath = System.IO.Path.Join(workingDirectory, "BadBrokerRates.db");
        }    */   

        public BadBrokerRatesRepositoryContext(DbContextOptions<BadBrokerRatesRepositoryContext> options)
        : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) { }

     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
