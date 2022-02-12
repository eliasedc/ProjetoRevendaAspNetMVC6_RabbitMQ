using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Integracao.RabbitMQ
{
    /// <summary>
    /// Manipula classe de Canal de Conexão com o RabbitMQ
    /// </summary>
    internal class CanalRabbitMQ
    {
        #region :: Atributos / Propriedades ::

        /// <summary>
        /// Armazena conexão com o RabbitMQ
        /// </summary>
        private IConnection objConexaoRabbitMQ;
        /// <summary>
        /// Armazena canal de comunicação com o RabbitMQ
        /// </summary>
        private IModel objCanalRabbitMQ;

        #endregion

        #region :: Construtores ::
        public CanalRabbitMQ(ConexaoRabbitMQ pConexaoRabbitMQ)
        {
            objConexaoRabbitMQ = pConexaoRabbitMQ.ResgatarConexaoRabbitMQ();
        }
        #endregion :: Construtores ::

        #region :: Métodos Públicos :;
        public IModel ResgatarCanalRabbitMQ()
        {
            if (objCanalRabbitMQ != null && objCanalRabbitMQ.IsOpen)
            {
                return objCanalRabbitMQ;
            }
            else
            {
                objCanalRabbitMQ = objConexaoRabbitMQ.CreateModel();
                return objCanalRabbitMQ;
            }
        }

        public bool CanalEstaAtivo()
        {
            return objCanalRabbitMQ != null && objCanalRabbitMQ.IsOpen;
        }

        #endregion :: Métodos Públicos :;
    }
}
