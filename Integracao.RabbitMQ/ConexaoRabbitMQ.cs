using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Integracao.RabbitMQ
{
    /// <summary>
    /// Manipula classe de conexão com o RabbitMQ
    /// </summary>
    internal class ConexaoRabbitMQ
    {
        #region :: Atributos ::
        /// <summary>
        /// Connexão com o RabbitMQ
        /// </summary>
        private IConnection objCoxecaoRabbitMQ;

        #endregion :: Atributos ::

        #region :: Métodos Públicos ::

        /// <summary>
        /// Retorna conexão com o RabbitMQ. Caso já exista uma conexão ativa, não é criada uma nova.
        /// </summary>
        /// <returns>Conexão com o Rabbit</returns>
        public IConnection ResgatarConexaoRabbitMQ()
        {
            if (objCoxecaoRabbitMQ != null && objCoxecaoRabbitMQ.IsOpen)
            {
                return objCoxecaoRabbitMQ;
            }
            else
            {
                //TODO Entender melhor como vai ser a estrutura da infra.
                //Sugestão, criar arquivo no servidor em um lugar padrão OU buscar do BD
                //Da forma que está, quem foi consumir essa API sempre vai ter que ter esse arquivo junto ao projeto principal
                string strFileName = Directory.GetCurrentDirectory() + "\\appsettings_RabbitMQ.json";
                string strJson = File.ReadAllText(strFileName);
                ServiceConfiguration objConfiguracao = JsonConvert.DeserializeObject<ServiceConfiguration>(strJson);

                ConnectionFactory objConnectionFactory = new ConnectionFactory()
                {
                    HostName = objConfiguracao.RabbitMQAddress,
                    UserName = objConfiguracao.RabbitMQLogin,
                    Password = objConfiguracao.RabbitMQSenha

                };

                objCoxecaoRabbitMQ = objConnectionFactory.CreateConnection();
            }
            return objCoxecaoRabbitMQ;
        }
        #endregion ::Métodos Públicos
    }
}
