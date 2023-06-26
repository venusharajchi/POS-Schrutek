﻿using System;
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


        public void Create(Kunde newEntity)
        {
            try
            {
                DbSet<Kunde> dbSet = _db.Set<Kunde>();
                dbSet.Add(newEntity);
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Create nicht möglich!", ex);
            }
        }


        public void Update(Kunde entity)
        {
            try
            {
                Kunde? existingEntity = _db.Set<Kunde>().First(k => k.Guid.Equals(entity.Guid));

                if (existingEntity == null)
                    throw new RepositoryException($"Tierheimhaus with id {entity.Guid} not found!");

                // If the entity exists in the database, update its properties.
                _db.Entry(existingEntity).CurrentValues.SetValues(entity);
                _db.SaveChanges();

            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Update nicht möglich!", ex);
            }
        }


        public IQueryable<Kunde> GetAll()
        {
            return _db.Set<Kunde>();
        }
    }
}

