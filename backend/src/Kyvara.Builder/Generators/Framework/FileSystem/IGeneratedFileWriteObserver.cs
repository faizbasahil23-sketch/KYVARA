namespace Kyvara.Builder.Generators.Framework.FileSystem;

public interface IGeneratedFileWriteObserver
{
    void OnCompleted(GeneratedFileWriteResult result);
}
