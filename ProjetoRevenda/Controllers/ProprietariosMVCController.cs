#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoRevenda.Models.DB;
using ProjetoRevenda.Utils;

namespace ProjetoRevenda.Controllers
{
    public class ProprietariosMVCController : Controller
    {
        private readonly BDDesenvolContext _context;

        #region :: Construtor ::
        public ProprietariosMVCController(BDDesenvolContext context)
        {
            _context = context;
        }

        #endregion :: Construtor ::

        #region :: Métodos CRUD ::
        // GET: ProprietariosMVC
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proprietarios.ToListAsync());
        }

        // GET: ProprietariosMVC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietario = await _context.Proprietarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proprietario == null)
            {
                return NotFound();
            }

            return View(proprietario);
        }

        // GET: ProprietariosMVC/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProprietariosMVC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Documento,Email,Cep,Endereco,Complemento,Status")] Proprietario proprietario)
        {
            if (ModelState.IsValid && ValidarCampos(proprietario, true))
            {
                _context.Add(proprietario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proprietario);
        }

        // GET: ProprietariosMVC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietario = await _context.Proprietarios.FindAsync(id);
            if (proprietario == null)
            {
                return NotFound();
            }
            return View(proprietario);
        }

        // POST: ProprietariosMVC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Documento,Email,Cep,Endereco,Complemento,Status")] Proprietario proprietario)
        {
            if (id != proprietario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && ValidarCampos(proprietario, false))
            {
                try
                {
                    _context.Update(proprietario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProprietarioExists(proprietario.Id))
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
            return View(proprietario);
        }
       
        private bool ProprietarioExists(int id)
        {
            return _context.Proprietarios.Any(e => e.Id == id);
        }
        #endregion :: Métodos CRUD ::

        #region :: Métodos Privados ::

        /// <summary>
        /// Valida dados do Proprietario para insert ou update
        /// </summary>
        /// <param name="pProprietario">objeto com campos preenchidos para validação</param>
        /// <param name="pInserir">Identifica se deve validar os campos para fazer insert. Caso false validará para update</param>
        /// <returns>True caso os campos estejam válidos</returns> 
        private bool ValidarCampos(Proprietario pProprietario, bool pInserir)
        {
            Proprietario objProprietarioExistente = null;
            Utilidades objUtilidades = new Utilidades();

            if (pInserir)
            {
                objProprietarioExistente = _context.Proprietarios.Where(r => r.Documento.Trim() == pProprietario.Documento.Trim()).FirstOrDefault();
                if (objProprietarioExistente != null)
                {
                    ModelState.AddModelError(nameof(Proprietario.Documento), $"Documento já atribuido para: {objProprietarioExistente.Nome}");
                    return false;
                }

                if(pProprietario.Status != "A")
                {
                    ModelState.AddModelError(nameof(Proprietario.Status), $"Não é permitido gravar um cadastro novo com status Cancelado.");
                    return false;
                }
            }
            else
            {
                objProprietarioExistente = _context.Proprietarios.AsNoTracking().Where(r =>
                                                    r.Documento.Trim() == pProprietario.Documento.Trim() &&
                                                    r.Id != pProprietario.Id).FirstOrDefault();
                if (objProprietarioExistente != null)
                {
                    ModelState.AddModelError(nameof(Proprietario.Documento), $"Documento já atribuido para: {pProprietario.Nome}");
                    return false;
                }
            }

            if (!objUtilidades.IsCpfCnpjValid(pProprietario.Documento))
            {
                ModelState.AddModelError(nameof(Proprietario.Documento), "CPF / CNPJ inválido");
                return false;
            }

            if (pProprietario.Cep.ToString().Length != 8)
            {
                ModelState.AddModelError(nameof(Proprietario.Cep), "CEP deve conter 8 dígitos");
                return false;
            }

            return true;
        }

        #endregion ::Métodos Privados ::
    }
}
