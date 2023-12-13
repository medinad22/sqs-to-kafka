namespace genesis_apolices_processamento_teste.EnviaApolice
{
    public interface IEnviaApolice
    {
        void ProduceMessage(string topic, string key, string value);
    }
}