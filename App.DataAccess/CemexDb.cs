using System.Configuration;
using System.Data.Entity;
using App.Domain;
using System.Data.SqlClient;

namespace App.DataAccess
{

    public class CemexDb : DbContext
    {
      
        public CemexDb() : base(ConfigurationManager.ConnectionStrings["CemexDb"].ConnectionString)
        {

        }
        public SqlConnection GetConnection()
        {
             return new SqlConnection(ConfigurationManager.ConnectionStrings["CemexDb"].ConnectionString);

        }

        public CemexDb(string connectionString) : base(connectionString)
        {
        }

        public DbSet<User> User { get; set; }
    

        public virtual IDbSet<T> DbSet<T>() where T : class
        {
            return Set<T>();
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CemexDb>(null);
        }
    }

    internal class Initialiser : DropCreateDatabaseAlways<CemexDb>
    {
        protected override void Seed(CemexDb context)
        {
            context.SaveChanges();
        }
    }

}