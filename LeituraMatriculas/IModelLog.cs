using System;

namespace LeituraMatriculas
{
    public interface IModelLog
    {
        event Action NotificarLogAlterado;
        string Mensagem { get; set; }

        void LogErroImagem(string tipoImagemDesconhecida);
        void LogErroMatricula();
        void SolicitarLog(ref string log);
    }
}
