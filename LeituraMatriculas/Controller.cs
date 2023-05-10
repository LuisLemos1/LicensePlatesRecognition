using System;

namespace LeituraMatriculas
{
    class Controller
    {
        private IModel model;
        private View view;
        private IModelLog modelLog;
        bool sair;

        public Controller()
        {
            sair = false;
            view = new View(this, model);
            model = new Model(this, view);
            modelLog = new ModelLog();

            /* Eventos */
            view.UtilizadorClicouCarregaImagem += UtilizadorPretendeSelecionarImagem;

            view.ImagemSelecionada += UtilizadorSelecionaImagem;
            view.PrecisoDaMatricula += model.SolicitarMatricula;

            view.PrecisoDeLog += modelLog.SolicitarLog;
            modelLog.NotificarLogAlterado += view.NotificacaoDeLogAlterado;

            view.UtilizadorClicouEncerrar += UtilizadorPretendeEncerrar;
        }

        public void IniciarPrograma()
        {
            do {
                view.AtivarInterface();
            } while (!sair);
        }

        private void ImagemDesconhecida(string imagem)
        {
            modelLog.LogErroImagem(imagem);
        }

        private void MatriculaDesconhecida()
        {
            modelLog.LogErroMatricula();
        }

        private void UtilizadorPretendeSelecionarImagem()
        {
            view.MostraSelecaoImagem();
        }

        private void UtilizadorSelecionaImagem(string caminhoImagem)
        {
            try
            {
                model.ProcessaImagem(caminhoImagem);
            }
            catch (Exception ex)
            {
                // verificar as varias exceptions possiveis
                if (ex is ExceptionImagemDesconhecida)
                    ImagemDesconhecida(ex.Message);
                if (ex is ExceptionMatriculaDesconhecida)
                    MatriculaDesconhecida();
            }
        }

        private void UtilizadorPretendeEncerrar()
        {
            sair = true;
            view.TerminarPrograma();
        }
    }
}
