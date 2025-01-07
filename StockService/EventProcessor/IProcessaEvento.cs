namespace StockService.EventProcessor
{
    public interface IProcessaEvento
    {
        void Processa(string mensagem);
    }
}