using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.RabbitMQ
{
    /// <summary>
    /// Utilizada para armazenar informações de uma mensagem específica a ser enviada / consultada do Rabbit
    /// </summary>
    public class MensagemVeiculo : IMensagem
    {
        #region :: Constantes ::
        /// <summary>
        /// Chave que será armazenada para registrar / consultar a mensagem
        /// </summary>
        private const string QUEUE = "CADASTRO_VEICULO";

        #endregion :: Constantes ::

        #region :: Atributos / Propriedades :: 
        /// <summary>
        /// Renavam do veículo. Exemplo: 19472689798
        /// </summary>
        public long Renavam { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Modelo { get; set; }
        /// <summary>
        /// Modelo do Veículo. Exemplo: Fusca 1600
        /// </summary>
        public string EmailDestinatario { get; set; }
        /// <summary>
        /// Nome do proprietário do veículo.
        /// </summary>
        public string NomeProprietario { get; set; }
        /// <summary>
        /// Atributo Herdado. Armazena a chave da mensagem que será enviada/consultada do RabbitMQ
        /// </summary>
        public string Queue { get; set; }

        #endregion :: Atributos / Propriedades ::

        #region :: Construtores ::
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="pRenavam">Renavam do veículo. Exemplo: 19472689798 </param>
        /// <param name="pModelo">Modelo do Veiculo. Exemplo: Fusca 1600</param>
        /// <param name="pEmailDestinatario">Email do propritário do veículo.</param>
        /// <param name="pNomeProprietario">Nome do proprietário do veículo.</param>
        public MensagemVeiculo(long pRenavam, string pModelo, string pEmailDestinatario, string pNomeProprietario)
        {
            Renavam = pRenavam;
            Modelo = pModelo;
            EmailDestinatario = pEmailDestinatario;
            NomeProprietario = pNomeProprietario;
            Queue = QUEUE;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public MensagemVeiculo()
        {
            Queue = QUEUE;
        }
        #endregion :: Construtores
    }
}
