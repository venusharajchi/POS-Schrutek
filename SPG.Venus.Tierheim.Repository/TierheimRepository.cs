using System;
using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Infrastructure;

namespace SPG.Venus.Tierheim.Repository
{
    public class TierheimRepository : ITierheimRepository, IReadOnlyTierheimRepository
    {
        private readonly TierheimContext _db;

        public TierheimRepository(TierheimContext db)
        {
            _db = db;
        }



        public IQueryable<Tierheimhaus> GetAll()
        {
            return _db.Tierheimhaeuser.Include(th => th.Tiere);
        }



        public IQueryable<Tierheimhaus> GetById(int id)
        {
            return _db.Tierheimhaeuser.Include(th => th.Tiere).Where(th => th.Id == id);
        }



        public IQueryable<Tierheimhaus> GetByName(String name)
        {
            return _db.Tierheimhaeuser.Include(th => th.Tiere).Where(th => th.Name == name);
        }



        public void Create(Tierheimhaus newEntity)
        {
            DbSet<Tierheimhaus> dbSet = _db.Set<Tierheimhaus>();
            dbSet.Add(newEntity);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new TierheimRepositoryException("Create not possible!", ex);
            }
        }



        public void Update(Tierheimhaus entity)
        {
            DbSet<Tierheimhaus> dbSet = _db.Set<Tierheimhaus>();
            dbSet.Update(entity);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new TierheimRepositoryException("Update not possible!", ex);
            }
        }



        public void Delete(int id)
        {
            Tierheimhaus? entity = GetById(id).SingleOrDefault();
            if (entity != null)
            {
                DbSet<Tierheimhaus> dbSet = _db.Set<Tierheimhaus>();
                dbSet.Remove(entity);
                _db.SaveChanges();
            }
            else
            {
                throw new TierheimRepositoryException($"Tierheimhaus with ID '{id}' not found!");
            }
        }
    }
}
