#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integracao.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoRevenda.Models.DB;
using ProjetoRevenda.Utils;
using RabbitMQ.Client;

namespace ProjetoRevenda.Controllers
{
    public class VeiculosMVCController : Controller
    {
        private readonly BDDesenvolContext _context;

        #region :: Construtores ::
        public VeiculosMVCController(BDDesenvolContext context)
        {
            _context = context;
        }
        #endregion :: Construtores ::

        #region :: Métodos  CRUD ::

        // GET: VeiculosMVC
        public async Task<IActionResult> Index()
        {
            var bDDesenvolContext = _context.Veiculos.Include(v => v.Marca).Include(v => v.Proprietario).Include(v => v.StatusVeiculo);
            return View(await bDDesenvolContext.ToListAsync());
        }

        // GET: VeiculosMVC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .Include(v => v.Marca)
                .Include(v => v.Proprietario)
                .Include(v => v.StatusVeiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: VeiculosMVC/Create
        public IActionResult Create()
        {            
            CarregarListas(null);
            return View();
        }

        // POST: VeiculosMVC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProprietarioId,Renavam,MarcaId,Modelo,AnoFabricacao,AnoModelo,Quilometragem,Valor,StatusVeiculoId")] Veiculo veiculo)
        {
            CarregarForeingKeys(veiculo);
            
            if (ModelState.IsValid && ValidarCampos(veiculo, true))
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();

                MensagensControleRabbitMQ objMensagensControle = new MensagensControleRabbitMQ();
                if(!objMensagensControle.EnviarParaRabbit(new MensagemVeiculo(veiculo.Renavam, veiculo.Modelo,
                                                                              veiculo.Proprietario.Email, veiculo.Proprietario.Nome), out string strErro))
                {
                    //TODO ver com a equipe que deve ser feito caso de erro ao enviar para o Rabbit.
                    //Permitir ao usuário enviar novamente?
                    //ter um painel de controle sobre isso?
                    //Registrar de alguma forma que o email nao pode ser enviado e ao editar o cadastro enviar novamente?
                };
                
                return RedirectToAction(nameof(Index));
            }
            CarregarListas(veiculo);
            return View(veiculo);
        }
               
        // GET: VeiculosMVC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }
            CarregarListas(veiculo);            
            return View(veiculo);
        }

        // POST: VeiculosMVC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProprietarioId,Renavam,MarcaId,Modelo,AnoFabricacao,AnoModelo,Quilometragem,Valor,StatusVeiculoId")] Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return NotFound();
            }
            CarregarForeingKeys(veiculo);
            if (ModelState.IsValid && ValidarCampos(veiculo, false))
            {
                try
                {
                    //Descomentar aqui caso queira testar fazendo update de um veículo
                    //EnviarParaRabbit(veiculo);
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.Id))
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
            CarregarListas(veiculo);
            return View(veiculo);
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
        #endregion :: Métodos CRUD ::

        #region :: Métodos Privados ::
        /// <summary>
        /// Carrega listas para serem pesquisadas / selecionadas pelo usuário na tela.
        /// </summary>
        /// <param name="pVeiculoSelecionado">Veiculo setado com os registros que devem abrir a tela já selecionados. null caso não tenha nenhum específico</param>
        private void CarregarListas(Veiculo pVeiculoSelecionado)
        {
            var lstProprietarios = from p in _context.Proprietarios
                                   where p.Status == "A" || 
                                         p.Id == (pVeiculoSelecionado != null ? pVeiculoSelecionado.ProprietarioId : -1)
                                   select new
                                   {
                                       p.Id,
                                       DataTextField = p.Nome + " - " + (p.Documento.Length == 11 ?
                                                                        Convert.ToUInt64(p.Documento).ToString(@"000\.000\.000\-00") :
                                                                        Convert.ToUInt64(p.Documento).ToString(@"00\.000\.000\/0000\-00"))
                                   };
                        
            ViewData["MarcaId"] = new SelectList(_context.Marcas.Where(r => r.Status == "A" ||
                                                                            r.Id == (pVeiculoSelecionado != null ? pVeiculoSelecionado.MarcaId : -1))
                                                , "Id", "Nome", (pVeiculoSelecionado != null ? pVeiculoSelecionado.MarcaId : null));
            ViewData["ProprietarioId"] = new SelectList(lstProprietarios, "Id", "DataTextField", (pVeiculoSelecionado != null ? pVeiculoSelecionado.ProprietarioId : null));
            ViewData["StatusVeiculoId"] = new SelectList(_context.VeiculoStatuses, "Id", "DescricaoStatus");
        }

        /// <summary>
        /// Carrega os objetos FK do modelo para persistir no banco de dados.
        /// </summary>
        /// <param name="pVeiculo">Veiculo com os "Id" das FK preenchido. Exemplo Veiculo.StatusVeiculoId</param>
        private void CarregarForeingKeys(Veiculo pVeiculo)
        {
            pVeiculo.StatusVeiculo = _context.VeiculoStatuses.Find(pVeiculo.StatusVeiculoId);            
            pVeiculo.Marca = _context.Marcas.Find(pVeiculo.MarcaId);
            pVeiculo.Proprietario = _context.Proprietarios.Find(pVeiculo.ProprietarioId);
            ModelState.Remove("Proprietario");
            ModelState.Remove("StatusVeiculo");
            ModelState.Remove("Marca");
        }

        /// <summary>
        /// Valida dados do Veículo para insert ou update
        /// </summary>
        /// <param name="pProprietario">objeto com campos preenchidos para validação</param>
        /// <param name="pInserir">Identifica se deve validar os campos para fazer insert. Caso false validará para update</param>
        /// <returns>True caso os campos estejam válidos</returns> 
        private bool ValidarCampos(Veiculo pVeiculo, bool pInserir)
        {
            Utilidades objUtilidades = new Utilidades();
            Veiculo objVeiculoExistente = null;

            if (pInserir)
            {
                objVeiculoExistente = _context.Veiculos.Where(r => r.Renavam == pVeiculo.Renavam).FirstOrDefault();                
                if (objVeiculoExistente != null)
                {
                    CarregarForeingKeys(objVeiculoExistente);
                    ModelState.AddModelError(nameof(Veiculo.Renavam),
                        $"Renavam já atribuido para: {objVeiculoExistente.Modelo}");
                    return false;
                }

                if (pVeiculo.Marca.Status != "A")
                {
                    ModelState.AddModelError(nameof(Veiculo.MarcaId), $"A Marca deve estar Ativa.");
                    return false;
                }

                if (pVeiculo.Proprietario.Status != "A")
                {
                    ModelState.AddModelError(nameof(Veiculo.ProprietarioId), $"O Proprietário deve estar Ativa.");
                    return false;
                }
                if (pVeiculo.StatusVeiculo.StatusInicial != "S")
                {
                    ModelState.AddModelError(nameof(Veiculo.StatusVeiculoId), $"Status inválido para o cadastro inicial do veículo.");
                    return false;
                }
            }
            else
            {
                objVeiculoExistente = _context.Veiculos.AsNoTracking().Where(r =>
                                                    r.Renavam == pVeiculo.Renavam &&
                                                    r.Id != pVeiculo.Id).FirstOrDefault();
                
                if (objVeiculoExistente != null)
                {
                    ModelState.AddModelError(nameof(Veiculo.Renavam),
                        $"Renavam já atribuido para: {objVeiculoExistente.Modelo}");
                    return false;
                }

                objVeiculoExistente = _context.Veiculos.AsNoTracking().Where(r => r.Id == pVeiculo.Id).FirstOrDefault();
                CarregarForeingKeys(objVeiculoExistente);

                if (objVeiculoExistente.StatusVeiculo.StatusInicial != "S" && pVeiculo.StatusVeiculo.StatusInicial == "S")
                {
                    ModelState.AddModelError(nameof(Veiculo.StatusVeiculoId),
                        $"Não é permitido voltar o status para {pVeiculo.StatusVeiculo.DescricaoStatus}.");
                    return false;
                }

                if (objVeiculoExistente.MarcaId != pVeiculo.MarcaId && pVeiculo.Marca.Status != "A")
                {
                    ModelState.AddModelError(nameof(Veiculo.MarcaId),
                        $"Caso deseja alterar a marca. A nova marca deve estar ativa.");
                    return false;
                }

                if (objVeiculoExistente.ProprietarioId != pVeiculo.ProprietarioId && pVeiculo.Proprietario.Status != "A")
                {
                    ModelState.AddModelError(nameof(Veiculo.ProprietarioId),
                        $"Caso deseja alterar o Proprietário. O novo proprietário deve estar ativo.");
                    return false;
                }
            }
            
            if (!objUtilidades.IsRenavamValid(pVeiculo.Renavam.ToString())){
                ModelState.AddModelError(nameof(Veiculo.Renavam),
                        $"Renavam inválido.");
                return false;
            }

            if(pVeiculo.Valor <= 0)
            {
                ModelState.AddModelError(nameof(Veiculo.Valor), $"Valor deve ser maior que zero");
                return false;
            }

            if (pVeiculo.Valor > 99999999.99m)
            {
                ModelState.AddModelError(nameof(Veiculo.Valor), $"Valor máximo permitido: 99.999.999,99");
                return false;
            }

            return true;
        }
       
        #endregion :: Métodos Privados ::
    }
}
