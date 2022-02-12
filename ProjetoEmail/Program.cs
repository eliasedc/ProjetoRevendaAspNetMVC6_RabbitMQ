using System.Net;
using System.Net.Mail;
using System.Text;
using Integracao.RabbitMQ;
using Newtonsoft.Json;

/// <summary>
/// Armazena parametros do arquivo de configuração
/// </summary>
ProjetoEmail.ServiceConfiguration objConfiguracao = null;
/// <summary>
/// Classe para integração com o RabbitMQ
/// </summary>
MensagensControleRabbitMQ objMensagensControleRabbitMQ = null;
/// <summary>
/// Utilizado para tentar fazer uma conexão por vez com o RabbitMQ, devido ao método ser disparado de acordo com a Thread de um Timer
/// </summary>
object objLockConexao = new object();

try
{
    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  Iniciando.. Pressionar Ctrl + Break para parar ");

    Timer tmrTimer;
    AutoResetEvent waitHandle = new AutoResetEvent(false);
    objMensagensControleRabbitMQ = new MensagensControleRabbitMQ();

    objConfiguracao = ResgatarParametrosConfiguracao();

    //se tiver debugando não abre o timer
    if (System.Diagnostics.Debugger.IsAttached)
    {
        ConsultarRabbit(objMensagensControleRabbitMQ);
    }
    else
    {
        // Configura o timer para execução e inicia sua execução imediata
        tmrTimer = new Timer(
             callback: TimerElapsed,
             state: null,
             dueTime: 0,
             period: objConfiguracao.Intervalo);
    }

    // Control + Break para parar a aplicação
    Console.CancelKeyPress += (o, e) =>
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Saindo...");
        Thread.Sleep(1000);
        // Libera a continuação da thread principal
        waitHandle.Set();
        Environment.Exit(0);
    };

    // Utilizado para aguardar o evento CancelKeyPress 
    waitHandle.WaitOne();
}
catch (Exception ex)
{
    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Erro na Thread Principal: " + ex.Message + (ex.InnerException?.Message) + ex.StackTrace);
    //TODO salvar log em algum lugar? criar um painel de monitoramento?
}

#region :: Métodos Privados ::

/// <summary>
/// Retorna dados do arquivo appsettings.json
/// </summary>
ProjetoEmail.ServiceConfiguration ResgatarParametrosConfiguracao()
{
    string strFileName = Directory.GetCurrentDirectory() + "\\appsettings.json";
    string strJson = File.ReadAllText(strFileName);
    return JsonConvert.DeserializeObject<ProjetoEmail.ServiceConfiguration>(strJson);
}

/// <summary>
/// Evento do timer. Utilizado para verificar / estabelecer conexão com o RabbitMQ
/// </summary>
void TimerElapsed(object? state)
{
    //Forçar liberação de memória
    GC.Collect();
    ConsultarRabbit(objMensagensControleRabbitMQ);
}

/// <summary>
/// CallBack para o RabbitMQ. É disparado quando o RabbitMQ receber uma mensagem.
/// </summary>
void MessageJSON_Received(object? model, EventoDeliverRabbitMQ ea)
{
    var objBody = ea.Body.ToArray();
    var objMessageJson = Encoding.UTF8.GetString(objBody);
    EnviarEmail(objMessageJson);
}

/// <summary>
/// Faz conexão com o RabbitMQ, consulta e dispara evento que envia email se for necessário.
/// </summary>
void ConsultarRabbit(MensagensControleRabbitMQ pMensagensControleRabbitMQ)
{
    try
    {
        lock (objLockConexao)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Conferindo status / estabelecendo comunicação com Rabbit...");
            if (!objMensagensControleRabbitMQ.ConsultarRabbit(new MensagemVeiculo().Queue, MessageJSON_Received, out string strErro))
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Erro ao Consultar RabbitMQ: " + strErro);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Erro ao Comunicar com RabbitMQ: " + ex.Message + (ex.InnerException?.Message) + ex.StackTrace);
        //TODO salvar log em algum lugar? criar um painel de monitoramento?
    }
}

/// <summary>
/// Envia email com alguns dados da classe MensagemVeiculo
/// </summary>
void EnviarEmail(string objMessageJson)
{
    try
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Deseralizando Mensagem");
        MensagemVeiculo objDadosVeiculo = JsonConvert.DeserializeObject<MensagemVeiculo>(objMessageJson);

        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $" Enviando email para: {objDadosVeiculo.EmailDestinatario} ");

        MailAddress objFromAddress = new MailAddress(objConfiguracao.GmailEmail);
        MailAddress objToAddress = new MailAddress(objDadosVeiculo.EmailDestinatario);
        string strSubject = $"Cadastro de Veículo {objDadosVeiculo.Modelo}";
        string strBody = $"Caro(a) Sr(a) {objDadosVeiculo.NomeProprietario}\r\n\r\n" +
            $"Informamos que o cadastro do veículo {objDadosVeiculo.Modelo} RENAVAM {objDadosVeiculo.Renavam} foi realizado com sucesso.\r\n\r\n" +
            $"Atenciosamente,\r\nRevenda Veículos LTDA";

        SmtpClient objSmtp = new SmtpClient
        {
            Host = objConfiguracao.GmailSmtp,
            Port = objConfiguracao.GmailPort,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(objFromAddress.Address, objConfiguracao.GmailSenha)
        };

        using (var objMessage = new MailMessage(objFromAddress, objToAddress)
        {
            Subject = strSubject,
            Body = strBody
        })
        {
            objSmtp.Send(objMessage);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Email Enviado");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Erro ao enviar email: " + ex.Message + (ex.InnerException?.Message) + ex.StackTrace);
        //TODO salvar log em algum lugar? criar um painel de monitoramento?
    }
}

#endregion :: Métodos Privados ::