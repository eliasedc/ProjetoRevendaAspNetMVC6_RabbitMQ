#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoRevenda.Models.DB;

namespace ProjetoRevenda.Controllers
{
    public class MarcasMVCController : Controller
    {
        private readonly BDDesenvolContext _context;

        #region :: Construtor ::

        public MarcasMVCController(BDDesenvolContext context)
        {
            _context = context;
        }

        #endregion :: Construtor ::

        #region :: Métodos CRUD ::

        // GET: MarcasMVC
        public async Task<IActionResult> Index()
        {
            return View(await _context.Marcas.ToListAsync());
        }

        // GET: MarcasMVC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // GET: MarcasMVC/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MarcasMVC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Status")] Marca marca)
        {
            if (ModelState.IsValid && ValidarCampos(marca, true))
            {
                _context.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(marca);
        }        

        // GET: MarcasMVC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        // POST: MarcasMVC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Status")] Marca marca)
        {
            if (id != marca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && ValidarCampos(marca, false))
            {
                try
                {
                    _context.Update(marca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(marca.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }              

        private bool MarcaExists(int id)
        {
            return _context.Marcas.Any(e => e.Id == id);
        }

        #endregion :: Métodos CRUD ::

        #region :: Métodos Privados ::

        /// <summary>
        /// Valida dados da Marca para insert ou update
        /// </summary>
        /// <param name="pMarca">objeto com campos preenchidos para validação</param>
        /// <param name="pInserir">Identifica se deve validar os campos para fazer insert. Caso false validará para update</param>
        /// <returns>True caso os campos estejam validos</returns>        
        private bool ValidarCampos(Marca pMarca, bool pInserir)
        {
            Marca objMarcaExistente = null;
            if (pInserir)
            {
                objMarcaExistente = _context.Marcas.Where(r => r.Nome.Trim() == pMarca.Nome.Trim()).FirstOrDefault();
                if (objMarcaExistente != null)
                {
                    ModelState.AddModelError(nameof(Marca.Nome), $"Já existe uma marca com o nome: {pMarca.Nome}");
                    return false;
                }

                if (pMarca.Status != "A")
                {
                    ModelState.AddModelError(nameof(Marca.Status), $"Não é permitido gravar um cadastro novo com status Cancelado.");
                    return false;
                }
            }
            else
            {
                objMarcaExistente = _context.Marcas.AsNoTracking().FirstOrDefault(r => r.Id == pMarca.Id);
                if (objMarcaExistente.Nome != pMarca.Nome)
                {
                    ModelState.AddModelError(nameof(Marca.Nome), $"Não é permitido alterar o nome de uma Marca.");
                    return false;
                }
            }
            //objMarcaExistente = null;
            return true;
        }

        #endregion :: Métodos Privados ::
    }
}
