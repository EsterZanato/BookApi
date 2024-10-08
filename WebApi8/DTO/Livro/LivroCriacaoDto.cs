using WebApi8.DTO.Vinculo;
using WebApi8.Models;

namespace WebApi8.DTO.Livro
{
    public class LivroCriacaoDto
    {
        public string Titulo { get; set; }
        public AutorVinculoDto Autor { get; set;}
    }
}
