using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.RabbitMQ
{
    /// <summary>
    /// Contém dados padrões que toda mensagem enviada / consultada do RabbitMQ deve conter
    /// </summary>
    public interface IMensagem
    {
        /// <summary>
        /// Chave utilizada para inserção / consulta de mensagens no RabbitMQ
        /// </summary>
        public string Queue { get; set; }
    }
}
