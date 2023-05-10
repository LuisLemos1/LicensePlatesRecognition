namespace LeituraMatriculas
{
    public interface IModel
    {
        string Matricula { get; set; }
        bool TipoImagemValida(string caminhoImagem);
        void ProcessaImagem(string caminhoImagem);
        void SolicitarMatricula(ref string matricula);
    }
}
