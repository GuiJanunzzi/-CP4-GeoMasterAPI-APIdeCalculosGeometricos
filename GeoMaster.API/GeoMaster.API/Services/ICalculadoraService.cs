using GeoMaster.API.Interfaces;

namespace GeoMaster.API.Services
{
    public interface ICalculadoraService
    {
        double CalcularArea(ICalculos2D forma);
        double CalcularPerimetro(ICalculos2D forma);
        double CalcularVolume(ICalculos3D forma);
        double CalcularAreaSuperficial(ICalculos3D forma);
    }
}
