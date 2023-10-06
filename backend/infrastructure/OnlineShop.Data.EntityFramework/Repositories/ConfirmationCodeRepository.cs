using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.EntityFramework.Repositories
{
    public class ConfirmationCodeRepositoryEf : EfRepository<ConfirmationCode>, IConfirmationCodeRepository
    {
        public ConfirmationCodeRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
