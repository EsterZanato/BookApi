using Microsoft.EntityFrameworkCore;
using WebApi8.Data;
using WebApi8.DTO.Autor;
using WebApi8.DTO.Livro;
using WebApi8.Models;

namespace WebApi8.Services.Livro
{
    public class LivroService : ILivroInterface
    {

        private readonly AppDbContext _context;
        public LivroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<LivroModel>> BuscarLivroPorId(int idLivro)
        {
            ResponseModel<LivroModel> resposta = new ResponseModel<LivroModel>();
            try
            {
                var livro = await _context.Livros
                    .FirstOrDefaultAsync(livroBanco => livroBanco.Id == idLivro);

                if (livro == null)
                {
                    resposta.Mensagem = "Nenhum registro encontrado";
                    return resposta;
                }

                resposta.Dados = livro;
                resposta.Mensagem = "Livro localizado com sucesso";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<LivroModel>>> BuscarLivroPorIdAutor(int idAutor)
        {
            ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
            try
            {
                var livro = await _context.Livros
                    .Include(a => a.Autor)
                    .Where(livroBanco => livroBanco.Id == idAutor)
                    .ToListAsync();

                if (livro == null)
                {
                    resposta.Mensagem = "Nenhum registro encontrado";
                    return resposta;
                }

                resposta.Dados = livro;
                resposta.Mensagem = "Livro(s) localizado(s)!";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<LivroModel>>> CriarLivro(LivroCriacaoDto livroCriacaoDto)
        {
            ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
            try
            {
                var autor = await _context.Autores.FirstOrDefaultAsync(autorBanco => autorBanco.Id == livroCriacaoDto.Autor.Id);

                if (autor == null)
                {
                    resposta.Mensagem = ("Nenhum registro de autor localizado");
                    return resposta;
                }

                var livro = new LivroModel()
                {
                    Titulo = livroCriacaoDto.Titulo,
                    Autor = autor
                };

                _context.Add(livro);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Livros.Include(a => a.Autor).ToListAsync();
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<LivroModel>>> EditarLivro(LivroEdicaoDto livroEdicaoDto)
        {
            ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
            try
            {
                var livro = await _context.Livros
                    .Include(a => a.Autor)
                    .FirstOrDefaultAsync(livroBanco => livroBanco.Id == livroEdicaoDto.Autor.Id);

                var autor = await _context.Autores
                    .FirstOrDefaultAsync(autorBanco => autorBanco.Id == livroEdicaoDto.Autor.Id);

                if (autor == null || livro == null)
                {
                    resposta.Mensagem = "Nenhum registro encontrado";
                    return resposta;
                }

                livro.Titulo = livroEdicaoDto.Titulo;
                livro.Autor = autor;

                _context.Update(livro);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Livros.ToListAsync();
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }   
        }

        public async Task<ResponseModel<List<LivroModel>>> ExcluirLivro(int idLivro)
        {
            ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
            try
            {
                var livro = await _context.Livros.Include(a => a.Autor)
                    .FirstOrDefaultAsync(livroBanco => livroBanco.Id == idLivro);

                if (livro == null)
                {
                    resposta.Mensagem = "Livro não encontrado!";
                    resposta.Status = false;
                    return resposta;
                }

                _context.Remove(livro);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Livros.ToListAsync();
                resposta.Mensagem = "Livro removido com sucesso!";
                resposta.Status = true;

                return resposta;


            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<LivroModel>>> ListarLivros()
        {
            ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
            try
            {
                var Livros = await _context.Livros.Include(a => a.Autor).ToListAsync();

                resposta.Dados = Livros;
                resposta.Mensagem = "Todos os livros foram coletados";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }
    }
}

