using Xunit;

namespace UnitTest
{
    //Define que todas as collection "Base collection" estarao ligadas com a BaseTestFixture
    //é como se informass que ele é a classe pai só que sem herança
    [CollectionDefinition("Base collection")]
    public abstract class BaseTestCollection : ICollectionFixture<BaseTestFixture>
    {
        
    }
}