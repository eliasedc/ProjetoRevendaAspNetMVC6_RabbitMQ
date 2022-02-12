using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Integracao.RabbitMQ
{
    public class EventoDeliverRabbitMQ : BasicDeliverEventArgs
    {
        public EventoDeliverRabbitMQ() : 
            base()
        {
        }
        
        public EventoDeliverRabbitMQ(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body) : 
            base (consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body)
        {
        }

    }
}
