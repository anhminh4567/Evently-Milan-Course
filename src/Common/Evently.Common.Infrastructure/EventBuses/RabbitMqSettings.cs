using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evently.Common.Infrastructure.EventBuses;
public class RabbitMqSettings
{
    public string Host { get; set; }
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
}
