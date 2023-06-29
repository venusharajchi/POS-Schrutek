using System;
using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Infrastructure;

namespace SPG.Venus.Tierheim.Repository
{
    public class KundeRepository : IKundeRepository, IReadOnlyKundeRepository
    {
        private readonly TierheimContext _db;


        public KundeRepository(TierheimContext db)
        {
            _db = db;
        }


        public DbContext GetContext()
        {
            return _db;
        }


        public IQueryable<Kunde> GetAll()
        {

            

            return _db.Kunden.Include(k => k.Tiere);


        }


        public IQueryable<Kunde> GetById(int id)
        {
            return _db.Kunden.Include(k => k.Tiere).Where(k => k.Id == id);
        }



        public IQueryable<Kunde> GetByGuid(Guid guid)
        {
            return _db.Set<Kunde>().Where(k => k.Guid == guid).Include(k => k.Tiere);
        }



        public void Create(Kunde newEntity)
        {
            DbSet<Kunde> dbSet = _db.Set<Kunde>();
            dbSet.Add(newEntity);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new KundeRepositoryException("Save not possible!", ex);
            }
        }



        public void Update(Kunde entity)
        {
            DbSet<Kunde> dbSet = _db.Set<Kunde>();
            dbSet.Update(entity);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new KundeRepositoryException("Update not possible!", ex);
            }
        }



        public void Delete(int id)
        {
            Kunde? entity = GetById(id).SingleOrDefault();
            if (entity != null)
            {
                DbSet<Kunde> dbSet = _db.Set<Kunde>();
                dbSet.Remove((Kunde)entity);
                _db.SaveChanges();
            }
            else
            {
                throw new KundeRepositoryException($"Kunde with ID {id} not found!");
            }
        }

    }
}
