using GeoMaster.API.DTOs;
using GeoMaster.API.Interfaces;
using GeoMaster.API.Models;
using GeoMaster.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GeoMaster.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalculosController : ControllerBase
    {
        private readonly ICalculadoraService _calculadoraService;

        // Injeção de dependência do serviço no construtor
        public CalculosController(ICalculadoraService calculadoraService)
        {
            _calculadoraService = calculadoraService;
        }

        // Helper method para desserializar a forma correta
        private object DesserializarForma(FormaRequestDto dto)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return dto.TipoForma.ToLower() switch
            {
                "circulo" => dto.Propriedades.Deserialize<Circulo>(options),
                "retangulo" => dto.Propriedades.Deserialize<Retangulo>(options),
                "esfera" => dto.Propriedades.Deserialize<Esfera>(options),
                _ => throw new NotSupportedException("Tipo de forma desconhecido.")
            };
        }

        /// <summary>
        /// Calcula a área de uma forma geométrica 2D.
        /// </summary>
        /// <remarks>
        /// Envie um JSON especificando o 'tipoForma' e suas 'propriedades'.
        /// <br/>
        /// <b>Exemplo para um círculo:</b>
        /// ```json
        /// {
        ///   "tipoForma": "circulo",
        ///   "propriedades": {
        ///     "raio": 10
        ///   }
        /// }
        /// ```
        /// <b>Exemplo para um retângulo:</b>
        /// ```json
        /// {
        ///   "tipoForma": "retangulo",
        ///   "propriedades": {
        ///     "largura": 5,
        ///     "altura": 8
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <param name="formaDto">Objeto que representa a forma e suas dimensões.</param>
        /// <returns>Um objeto JSON com o resultado da área calculada.</returns>
        /// <response code="200">Retorna o resultado do cálculo.</response>
        /// <response code="400">Se o tipo da forma for inválido, se as propriedades não corresponderem à forma, se as dimensões forem negativas ou se a forma não for 2D.</response>
        [HttpPost("area")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult CalcularArea([FromBody] FormaRequestDto formaDto)
        {
            try
            {
                var forma = DesserializarForma(formaDto);

                if (forma is ICalculos2D forma2D)
                {
                    // Validação de dimensões negativas
                    if (forma is Circulo c && c.Raio < 0 || forma is Retangulo r && (r.Largura < 0 || r.Altura < 0))
                    {
                        return BadRequest("As dimensões da forma não podem ser negativas.");
                    }

                    var resultado = _calculadoraService.CalcularArea(forma2D);
                    return Ok(new { resultado });
                }

                // Retorna erro se a forma não for 2D 
                return BadRequest("O cálculo de área só é suportado para formas 2D.");
            }
            catch (NotSupportedException e)
            {
                return BadRequest(e.Message);
            }
            catch (JsonException)
            {
                return BadRequest("As propriedades fornecidas não correspondem à forma especificada.");
            }
        }

        /// <summary>
        /// Calcula o perímetro de uma forma geométrica 2D.
        /// </summary>
        /// <remarks>
        /// Envie um JSON especificando o 'tipoForma' e suas 'propriedades'.
        /// <br/>
        /// <b>Exemplo:</b>
        /// ```json
        /// {
        ///   "tipoForma": "retangulo",
        ///   "propriedades": {
        ///     "largura": 5,
        ///     "altura": 10
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <param name="formaDto">Objeto que representa a forma e suas dimensões.</param>
        /// <returns>Um objeto JSON com o resultado do perímetro calculado.</returns>
        /// <response code="200">Retorna o resultado do cálculo.</response>
        /// <response code="400">Se o tipo da forma for inválido, as propriedades incorretas ou a forma não for 2D.</response>
        [HttpPost("perimetro")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult CalcularPerimetro([FromBody] FormaRequestDto formaDto)
        {
            try
            {
                var forma = DesserializarForma(formaDto);

                if (forma is ICalculos2D forma2D)
                {
                    if (forma is Circulo c && c.Raio < 0 || forma is Retangulo r && (r.Largura < 0 || r.Altura < 0))
                    {
                        return BadRequest("As dimensões da forma não podem ser negativas.");
                    }

                    var resultado = _calculadoraService.CalcularPerimetro(forma2D);
                    return Ok(new { resultado });
                }

                return BadRequest("O cálculo de perímetro só é suportado para formas 2D.");
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (JsonException)
            {
                return BadRequest("As propriedades fornecidas não correspondem à forma especificada.");
            }
        }

        /// <summary>
        /// Calcula o volume de uma forma geométrica 3D.
        /// </summary>
        /// <remarks>
        /// Envie um JSON especificando o 'tipoForma' e suas 'propriedades'.
        /// <br/>
        /// <b>Exemplo para uma esfera:</b>
        /// ```json
        /// {
        ///   "tipoForma": "esfera",
        ///   "propriedades": {
        ///     "raio": 7
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <param name="formaDto">Objeto que representa a forma e suas dimensões.</param>
        /// <returns>Um objeto JSON com o resultado do volume calculado.</returns>
        /// <response code="200">Retorna o resultado do cálculo.</response>
        /// <response code="400">Se o tipo da forma for inválido, as propriedades incorretas ou a forma não for 3D.</response>
        [HttpPost("volume")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult CalcularVolume([FromBody] FormaRequestDto formaDto)
        {
            try
            {
                var forma = DesserializarForma(formaDto);

                if (forma is ICalculos3D forma3D)
                {
                    if (forma is Esfera e && e.Raio < 0)
                    {
                        return BadRequest("As dimensões da forma não podem ser negativas.");
                    }

                    var resultado = _calculadoraService.CalcularVolume(forma3D);
                    return Ok(new { resultado });
                }

                return BadRequest("O cálculo de volume só é suportado para formas 3D.");
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (JsonException)
            {
                return BadRequest("As propriedades fornecidas não correspondem à forma especificada.");
            }
        }

        /// <summary>
        /// (Desafio) Verifica se uma forma geométrica pode ser contida dentro de outra.
        /// </summary>
        /// <remarks>
        /// Este endpoint recebe duas formas e retorna `true` se a 'formaInterna' puder ser geometricamente contida dentro da 'formaExterna'.
        /// <br/>
        /// <b>Combinações suportadas:</b>
        /// <ul>
        ///   <li>Círculo dentro de um Retângulo</li>
        ///   <li>Retângulo dentro de um Círculo</li>
        /// </ul>
        /// <br/>
        /// <b>Exemplo de requisição:</b>
        /// ```json
        /// {
        ///   "formaExterna": {
        ///     "tipoForma": "retangulo",
        ///     "propriedades": { "largura": 10, "altura": 10 }
        ///   },
        ///   "formaInterna": {
        ///     "tipoForma": "circulo",
        ///     "propriedades": { "raio": 5 }
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <param name="request">Objeto contendo a forma externa e a forma interna.</param>
        /// <returns>Um booleano indicando se a forma interna está contida na externa.</returns>
        /// <response code="200">Retorna o resultado da verificação (`true` ou `false`).</response>
        /// <response code="400">Se a combinação de formas não for suportada ou se houver um erro nos dados de entrada.</response>
        [HttpPost("/api/v1/validacoes/forma-contida")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult VerificarFormaContida([FromBody] FormaContidaRequestDto request)
        {
            try
            {
                var externa = DesserializarForma(request.FormaExterna);
                var interna = DesserializarForma(request.FormaInterna);

                bool contido = false;

                // Lógica 1: Círculo dentro de um Retângulo
                if (externa is Retangulo r && interna is Circulo c)
                {
                    // O diâmetro do círculo deve ser menor ou igual à largura E altura do retângulo
                    contido = (c.Raio * 2 <= r.Largura) && (c.Raio * 2 <= r.Altura);
                }
                    // Lógica 2: Retângulo dentro de um Círculo
                else if (externa is Circulo c2 && interna is Retangulo r2)
                {
                    // A diagonal do retângulo deve ser menor ou igual ao diâmetro do círculo
                    var diagonalRetangulo = Math.Sqrt(Math.Pow(r2.Largura, 2) + Math.Pow(r2.Altura, 2));
                    contido = diagonalRetangulo <= (c2.Raio * 2);
                }
                else
                {
                    return BadRequest("A combinação de formas para verificação de contenção não é suportada.");
                }

                return Ok(new { contido });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao processar a requisição: {ex.Message}");
            }
        }
    }
}
