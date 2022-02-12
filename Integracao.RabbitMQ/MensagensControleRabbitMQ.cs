using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Integracao.RabbitMQ
{
    /// <summary>
    /// Classe utilizada para enviar e consultar inforamções do Rabbit
    /// </summary>
    public class MensagensControleRabbitMQ
    {
        #region :: Atributos / Propriedades ::

        /// <summary>
        /// Classe que contém a conexão com o RabbitMQ
        /// </summary>
        private ConexaoRabbitMQ objConexaoRabbitMQ;

        /// <summary>
        /// Utilizado para abrir / utilizar um canal de comunição com o RabbitMQ
        /// </summary>
        private CanalRabbitMQ objCanalRabbitMQ;

        #endregion :: Atributos ::

        #region :: Construtores ::

        /// <summary>
        /// Construtor
        /// </summary>
        public MensagensControleRabbitMQ()
        {
            objConexaoRabbitMQ = new ConexaoRabbitMQ();
            objCanalRabbitMQ = new CanalRabbitMQ(objConexaoRabbitMQ);
        }

        #endregion :: Construtores ::

        #region :: Métodos Públicos :: 

        /// <summary>
        /// Envia mensagem para o RabbitMQ
        /// </summary>
        /// <param name="pMensagem">Objeto que será serializado em JSON e enviado para o RabbitMQ</param>
        /// <param name="strErro">string com mensagem de erro caso o retorno do método seja false</param>
        /// <returns>true caso enviado com sucesso</returns>
        public bool EnviarParaRabbit(IMensagem pMensagem, out string strErro)
        {
            try
            {
                string strJson = JsonConvert.SerializeObject(pMensagem);
                using (var objConnection = objConexaoRabbitMQ.ResgatarConexaoRabbitMQ())
                {
                    using (var objChannel = objConnection.CreateModel())
                    {
                        //Create a message queue named 
                        objChannel.QueueDeclare(pMensagem.Queue, false, false, false, null);
                        var objProperties = objChannel.CreateBasicProperties();
                        objProperties.DeliveryMode = 1;

                        //The content of the message delivered
                        objChannel.BasicPublish(exchange: "", routingKey: pMensagem.Queue, objProperties, Encoding.UTF8.GetBytes(strJson)); //Production message
                    }
                }
                strErro = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                /*TODO, verificar se é permitido deixar o stacktrace junto com o erro*/
                strErro = "Erro ao enviar mensagem apra o RabbitMQ: " + ex.Message + (ex.InnerException?.Message) + ex.StackTrace;
                return false;
            }
        }

        /// <summary>
        /// Abre conexão e canal com RabbitMQ e dispara evento passado por parâmetro em caso de novas mensagens.
        /// Obs.: Caso a conexão esteja ativa, não será criada uma nova conexão
        /// </summary>
        /// <param name="pQueue">Nome do Queue no RabbitMQ (chave da mensagem que será lida)</param>
        /// <param name="pCallBack">delegate para evento que será disparado quando RabbitMQ receber a mensagem que está sendo consumida</param>
        /// <param name="strErro">Mensagem de erro caso o retorno deste método for false.</param>
        /// <returns>True caso a consulta seja feita com sucesso.</returns>
        public bool ConsultarRabbit(string pQueue, EventHandler<EventoDeliverRabbitMQ> pCallBack, out string strErro)
        {
            try
            {
                objConexaoRabbitMQ.ResgatarConexaoRabbitMQ();

                if (!objCanalRabbitMQ.CanalEstaAtivo())
                {
                    var objChannel = objCanalRabbitMQ.ResgatarCanalRabbitMQ();
                    //Create a message queue named 
                    objChannel.QueueDeclare(pQueue, false, false, false, null);
                    var objProperties = objChannel.CreateBasicProperties();
                    objProperties.DeliveryMode = 1;

                    var objMessageJson = new EventingBasicConsumer(objChannel);

                    this.Received += pCallBack;
                    objMessageJson.Received += this.MessageJSON_Received;

                    objChannel.BasicConsume(queue: pQueue, autoAck: true, consumer: objMessageJson);
                }

                strErro = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                strErro = " Erro ao Comunicar com RabbitMQ: " + ex.Message + (ex.InnerException?.Message) + ex.StackTrace;
                return false;
                //TODO salvar log em algum lugar? criar um painel de monitoramento?
            }
        }

        #endregion :: Métodos Públicos ::

        #region :: Eventos ::

        /// <summary>
        /// Disparado quando o RabbitMQ receber uma mensagem.
        /// </summary>
        private event EventHandler<EventoDeliverRabbitMQ> Received;

        /// <summary>
        /// CallBack para o RabbitMQ. É disparado quando o RabbitMQ receber uma mensagem.
        /// </summary>
        private void MessageJSON_Received(object? model, BasicDeliverEventArgs ea)
        {
            EventoDeliverRabbitMQ objEventArgs = new EventoDeliverRabbitMQ(ea.ConsumerTag, ea.DeliveryTag, ea.Redelivered, ea.Exchange, ea.RoutingKey, ea.BasicProperties, ea.Body);
            Received?.Invoke(model, objEventArgs);
        }

        #endregion
    }
}