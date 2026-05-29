namespace EMDERSOFT.Patterns.Facade
{
    public class ResultadoOperacion
    {
        public bool Exitoso { get; private set; }
        public string Mensaje { get; private set; }
        public string? Error { get; private set; }

        private ResultadoOperacion(bool exitoso, string mensaje, string? error = null)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
            Error = error;
        }

        public static ResultadoOperacion Exito(string mensaje)
            => new ResultadoOperacion(true, mensaje);

        public static ResultadoOperacion Fallo(string error)
            => new ResultadoOperacion(false, "", error);
    }
}
