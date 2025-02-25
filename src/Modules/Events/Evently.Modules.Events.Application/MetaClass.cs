using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Evently.Modules.Events.Application;
public static class MetaClass
{
    public static readonly Assembly EventApplicationAssembly = typeof(MetaClass).Assembly;
}
