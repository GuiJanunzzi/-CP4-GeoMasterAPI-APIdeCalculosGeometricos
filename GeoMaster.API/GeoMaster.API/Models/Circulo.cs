using GeoMaster.API.Interfaces;

namespace GeoMaster.API.Models
{
    public class Circulo : ICalculos2D
    {
        public double Raio { get; set; }

        public double CalcularArea()
        {
            // Fórmula da área: π * r²
            return Math.PI * Math.Pow(Raio, 2);
        }

        public double CalcularPerimetro()
        {
            // Fórmula do perímetro (circunferência): 2 * π * r
            return 2 * Math.PI * Raio;
        }
    }
}
