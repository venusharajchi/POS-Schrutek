using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG.Venus.Tierheim.Repository.Tierheim
{
    public class TierheimRepository : ITierheimRepository, IReadOnlyTierheimRepository
    {
        private readonly TierheimContext _db;

        public TierheimRepository(TierheimContext db)
        {
            _db = db;
        }

        public void Create(Tierheimhaus newEntity)
        {
            try
            {
                DbSet<Tierheimhaus> dbSet = _db.Set<Tierheimhaus>();
                dbSet.Add(newEntity);
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Create nicht möglich!", ex);
            }
        }

        public IQueryable<Tierheimhaus> GetAll()
        {
            return _db.Set<Tierheimhaus>();
        }
    }
}

