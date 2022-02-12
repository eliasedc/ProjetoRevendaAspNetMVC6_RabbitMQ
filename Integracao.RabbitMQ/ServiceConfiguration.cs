using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Integracao.RabbitMQ
{
    /// <summary>
    /// Responsável por armazenar informações do arquivo appsettings_RabbitMQ.json
    /// </summary>
    internal class ServiceConfiguration
    {
        /// <summary>
        /// Endereço de comunicação com o RabbitMQ
        /// </summary>
        public string RabbitMQAddress { get; set; }
        /// <summary>
        /// Login (padrão guest)
        /// </summary>
        public string RabbitMQLogin { get; set; }
        /// <summary>
        /// Senha (padrão guest)
        /// </summary>
        public string RabbitMQSenha { get; set; }
    }
}
