using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LeituraMatriculas
{
    public partial class FormLeituraMatriculas : Form
    {
        View view;
        public FormLeituraMatriculas()
        {
            InitializeComponent();
        }

        public View View { get => view; set => view = value; }

        private void botaoCarregarImagem_Click(object sender, EventArgs e)
        {

            view.CliqueCarregaImagem(sender, e);
        }

        private void botaoEncerrar_Click(object sender, EventArgs e)
        {
            view.CliqueEncerrar(sender, e);
        }

        public string MostraSelecaoImagem()
        {
            string caminhoImagem = "";
            OpenFileDialog ofd = new OpenFileDialog();

            // abre a pasta default de imagens de testes da aplicacao
            ofd.InitialDirectory = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName).FullName + "\\assets\\ImagensCarros\\";
            // abre a pasta que mais recente foi escolhida (no programa)
            ofd.RestoreDirectory = true;
            // titlo da Janela da selecao
            ofd.Title = "Selecione Imagem do Carro";
            // caso o utilizador se esqueca da extensao, adiciona-se o default
            ofd.DefaultExt = "jpg";
            // filtrar para imagens apenas
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jfif, *.png) | *.jpg; *.jpeg; *.jfif; *.png";
            // verificar que o ficheiro existe
            ofd.CheckFileExists = true;
            // verificar que o caminho existe
            ofd.CheckPathExists = true;
            // Apenas se pode selecionar uma imagem
            ofd.Multiselect = false;            

            // Se nao houver erros, atribui-se ao caminho da Imagem
            if (ofd.ShowDialog() == DialogResult.OK)
                caminhoImagem = ofd.FileName;

            // Se houver erro, o caminho da imagem sera ""
            return caminhoImagem;
        }

        public void MostrarImagem(string caminhoImagem)
        {
            // Mostra a imagem na caixa a partir do caminho
            caixaImagem.Image = Image.FromFile(caminhoImagem);
            // Preenche imagem na caixa
            caixaImagem.SizeMode = PictureBoxSizeMode.StretchImage;            
        }

        public void MostraMatricula(string matricula)
        {
            // Actualiza caixa de Texto
            textoMatricula.Text = matricula;
        }

        private void FormLeituraMatriculas_Load(object sender, EventArgs e)
        {
        }

    }
}
