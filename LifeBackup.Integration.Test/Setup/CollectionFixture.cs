using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LifeBackup.Integration.Test.Setup
{
    [CollectionDefinition("api")]
    public class CollectionFixture : ICollectionFixture<TestContext>
    {

    }
}
