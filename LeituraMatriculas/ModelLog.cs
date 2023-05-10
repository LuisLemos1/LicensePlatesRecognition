using System;

namespace LeituraMatriculas
{
    internal class ModelLog : IModelLog
    {
        public event Action NotificarLogAlterado;

        public string Mensagem { get; set; }

        public void LogErroImagem(string tipoImagemDesconhecida)
        {
            Mensagem = "Imagem desconhecida: " + tipoImagemDesconhecida.ToString() + System.Environment.NewLine;
            NotificarLogAlterado();
        }

        public void LogErroMatricula()
        {
            Mensagem = "Problema no processamento da Imagem";
            NotificarLogAlterado();
        }

        public void SolicitarLog(ref string mensagem)
        {
            mensagem = this.Mensagem;
        }
    }
}
