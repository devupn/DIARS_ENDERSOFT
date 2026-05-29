using EMDERSOFT.Models;

namespace EMDERSOFT.Patterns.Strategy
{
    public interface IOrdenamientoStrategy
    {
        IEnumerable<Herramienta> Ordenar(IEnumerable<Herramienta> herramientas);
        string Nombre { get; }
    }
}
