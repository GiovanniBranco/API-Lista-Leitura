using System;
using System.Linq;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Alura.WebAPI.Api.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Alura.WebAPI.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/livros")] //Versionando versão no desenvolvimento;
    public class DsnLivrosController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public DsnLivrosController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IActionResult ListaDeLivros([FromQuery] LivroFiltro filtro, [FromQuery] LivroOrdem ordem,
                                            [FromQuery] LivroPaginacao paginacao)
        {
            var lista = _repo.All
                .AplicaFiltro(filtro)
                .AplicaOrdem(ordem)
                .Select(l => l.ToApi())
                .ToLivroPaginado(paginacao);

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Recuperar(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model.ToApi());
        }

        [HttpGet("{id}/capa")]
        public IActionResult ImagemCapa(int id)
        {
            byte[] img = _repo.All
                .Where(l => l.Id == id)
                .Select(l => l.ImagemCapa)
                .FirstOrDefault();
            if (img != null)
            {
                return File(img, "image/png");
            }
            return File("~/images/capas/capa-vazia.png", "image/png");
        }


        [HttpPost]
        public IActionResult Incluir([FromForm] LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                _repo.Incluir(livro);
                var uri = Url.Action("Recuperar", new { id = livro.Id });
                return Created(uri, livro); //201
            }
            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }


        [HttpPut]
        public IActionResult Alterar([FromForm] LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if (model.Capa == null)
                {
                    livro.ImagemCapa = _repo.All
                        .Where(l => l.Id == livro.Id)
                        .Select(l => l.ImagemCapa)
                        .FirstOrDefault();
                }
                _repo.Alterar(livro);
                return Ok(); //200
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Remover(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _repo.Excluir(model);
            return NoContent(); //203
        }
    }
}
