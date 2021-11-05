using Dominio.Nucleo;
using System;
using Microsoft.EntityFrameworkCore;
using Dominio.Nucleo.Entidad;
using System.Linq.Expressions;

namespace Infraestructura.Nucleo
{
    public interface IContextoUnidadDeTrabajo:IUnidadDeTrabajo,ISql
    {
        //Devuelve una instancia de DbSet para acceder a entidades del tipo dado en el contexto
        DbSet<TEntidad> CreateSet<TEntidad>() where TEntidad : class;
        //Cada vez que se pase un nuevo objeto a nuestra tabla, la deje en cola. Adjunta elemento en el administrador de estado de objeto
        void Attach<TEntidad>(TEntidad item) where TEntidad : class;
        //Setea un modificación que esta pendiente en memoria
        void SetModified<TEntidad>(TEntidad item, params Expression<Func<TEntidad,object>>[] properties) where TEntidad : EntidadBase;
        //Aplica los cambios
        void ApplyCurrentValues<TEntidad>(TEntidad original, TEntidad current) where TEntidad : class;

    
    }
}
