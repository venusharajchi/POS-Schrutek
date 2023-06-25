using SPG.Venus.Tierheim.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG.Venus.Tierheim.Domain.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        int Create(TEntity newProduct);
        int Update(TEntity newEntity);
    }
}

