using EMDERSOFT.Models;

namespace EMDERSOFT.Patterns.Strategy
{
    public class OrdenarPorNombreDesc : IOrdenamientoStrategy
    {
        public string Nombre => "Nombre (Z-A)";
        public IEnumerable<Herramienta> Ordenar(IEnumerable<Herramienta> herramientas)
            => herramientas.OrderByDescending(h => h.Nombre);
    }
}
