using genesis_apolices_processamento_teste.EnviaApolice;

namespace genesis_apolices_processamento_teste.ProcessaApolice
{
    public class ProcessaApolice : IProcessaApolice
    {
        private readonly IEnviaApolice enviaApolice;

        public ProcessaApolice(IEnviaApolice enviaApolice)
        {
            this.enviaApolice = enviaApolice;
        }

        public void ProcessaApoliceNormalizada()
        {
            enviaApolice.ProduceMessage("topic", "key", "value");
        }
    }
}
