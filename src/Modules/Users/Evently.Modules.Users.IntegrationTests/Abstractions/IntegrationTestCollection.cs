using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evently.Modules.Users.IntegrationTests.Abstractions;
//[CollectionDefinition(nameof(IntegrationTestCollection))]
[CollectionDefinition("IntegrationTestCollection")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>
{
}
