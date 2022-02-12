using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjetoEmail
{
    /// <summary>
    /// Responsável por armazenar informações do arquivo appsettings.json
    /// </summary>
    public class ServiceConfiguration    {
        /// <summary>
        /// STMP do gmail
        /// </summary>
        public string GmailSmtp { get; set; }
        /// <summary>
        /// Email do gmail (from) 
        /// </summary>
        public string GmailEmail { get; set; }
        /// <summary>
        /// Senha do Email do gmail
        /// </summary>
        public string GmailSenha { get; set; }
        /// <summary>
        /// Porta a ser utilizada no envio smtp
        /// </summary>
        public int GmailPort { get; set; }
        /// <summary>
        /// Intervalo em milisegundos que o sistema consultará a conexão com o RabbitMQ
        /// </summary>
        public int Intervalo { get; set; }
    }
}
