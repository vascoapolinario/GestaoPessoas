using Microsoft.EntityFrameworkCore;
using GestaoPessoas.Entities;

namespace GestaoPessoas.DataSources
{
    public class GestaoPessoasDbContext : DbContext
    {
        public virtual DbSet<Worker> Workers => Set<Worker>();

        public GestaoPessoasDbContext(DbContextOptions<GestaoPessoasDbContext> options) : base(options)
        {

        }
    }
}
