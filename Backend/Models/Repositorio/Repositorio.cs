using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Interfaces.IRepositorio;

namespace Models.Repositorio
{
    public class Repositorio<T> : IRepositorioGenerico<T> where T : class
    {
        private readonly DogWalkPlusContext _context;
        private DbSet<T> _dbSet;
        public Repositorio(DogWalkPlusContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Agregar(T entidad)
        {
            await _dbSet.AddAsync(entidad);
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro, string incluirPropiedades)
        {
            IQueryable<T> query = _dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            if (incluirPropiedades != null)
            {
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string incluirPropiedades)
        {
            IQueryable<T> query = _dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            if (incluirPropiedades != null)
            {
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad);
                }
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public void Remover(T entidad)
        {
            _dbSet.Remove(entidad);
        }
    }
}
