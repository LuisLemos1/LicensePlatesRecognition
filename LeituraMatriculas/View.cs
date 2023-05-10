using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeituraMatriculas
{
    public class View
    {
        private Controller controller;
        private IModel model;
        private FormLeituraMatriculas janela;

        /*  Evento para quando o utilizador clica em Carregar Imagem
            Tem apenas um subscriber, no Controller.cs - chama a funcao de mostrar janela
        */
        public event Action UtilizadorClicouCarregaImagem;

        /* Evento para quando o utilizador seleciona a localizacao da Imagem
           Tem apenas um subscriber, no Controller.cs
        */
        public delegate void UtilizadorSelecionouCaminhoImagem(string caminhoImagem);
        public event UtilizadorSelecionouCaminhoImagem ImagemSelecionada;

        /*  Evento para quando o utilizador clica em Encerrar Programa
            Tem apenas um subscriber, no Controller.cs - chama a funcao de encerrar
        */
        public event Action UtilizadorClicouEncerrar;

        //  Solicitacao de Matricula do View para Model
        public delegate void SolicitacaoMatricula(ref string matricula);
        public event SolicitacaoMatricula PrecisoDaMatricula;

        // Adicionado evento para ser utilizado no controller
        public delegate void SolicitacaoLog(ref string log);
        public event SolicitacaoLog PrecisoDeLog;

        internal View(Controller controller, IModel model)
        {
            this.controller = controller;
            this.model = model;
        }

        // Iniciar o Form
        public void AtivarInterface()
        {
            janela = new FormLeituraMatriculas();
            janela.View = this;
            janela.ShowDialog();
        }

        //  Chamada no Form -> view.CliqueCarregaImagem(object sender, EventArgs e)
        public void CliqueCarregaImagem(object sender, EventArgs e)
        {
            UtilizadorClicouCarregaImagem();
        }

        public void MostraSelecaoImagem()
        {
            string caminhoImagem = janela.MostraSelecaoImagem();

            if (caminhoImagem.Length > 0)
                ImagemSelecionada(caminhoImagem);
        }

        public void NotificacaoDeLogAlterado()
        {
            string log = "";
            PrecisoDeLog(ref log);
            MessageBox.Show(log);
        }

        public void ProcessamentoTerminado(string caminho)
        {
            string matricula = "";

            PrecisoDaMatricula(ref matricula);

            janela.MostrarImagem(caminho);
            janela.MostraMatricula(matricula);
        }

        public void CliqueEncerrar(object sender, EventArgs e)
        {
            UtilizadorClicouEncerrar();
        }

        public void TerminarPrograma()
        {
            janela.Dispose();
        }
    }
}
