using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LeituraMatriculas
{
    class Model : IModel
    {
        private Controller controller;
        private View view;
        public string Matricula { get; set; }

        public Model(Controller controller, View view)
        {
            this.controller = controller;
            this.view = view;
            Matricula = "nao encontrada";
        }

        public bool TipoImagemValida(string caminhoImagem)
        {
            byte[] header, temp;
            string fileExt;

            // guardar num dicionario as informacoes do header do tipo de ficheiros de imagens utilizados na aplicacao
            Dictionary<string, byte[]> imageHeader = new Dictionary<string, byte[]>();

            //imageHeader.Add("JPG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 });
            //imageHeader.Add("JPEG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
            //imageHeader.Add("PNG", new byte[] { 0x89, 0x50, 0x4E, 0x47 });
            //imageHeader.Add("JFIF", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
            imageHeader.Add("JPG", new byte[] { 0xFF, 0xD8, 0xFF });
            imageHeader.Add("JPEG", new byte[] { 0xFF, 0xD8, 0xFF });
            imageHeader.Add("PNG", new byte[] { 0x89, 0x50, 0x4E, 0x47 });
            imageHeader.Add("JFIF", new byte[] { 0xFF, 0xD8, 0xFF });

            // obter a extensao do caminho de imagem
            fileExt = caminhoImagem.Substring(caminhoImagem.LastIndexOf('.') + 1).ToUpper();

            // caso nao existir a extensao, devolve erro
            if (!imageHeader.ContainsKey(fileExt))
                return false;

            // obter o header do dicionario correspondente
            temp = imageHeader[fileExt];

            // pegar no header do ficheiro a validar
            header = new byte[temp.Length];
            header = File.ReadAllBytes(caminhoImagem).Take(header.Length).ToArray();

            if (temp.Length != header.Length)
                return false;

            // comparar os headers
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] != header[i])
                    return false;
            }

            return true;
        }

        public void ProcessaImagem(string caminhoImagem)
        {
            // Controllar para o maximo de 10 MB
            if ((new FileInfo(caminhoImagem)).Length > 10000000)
                throw new ExceptionImagemDesconhecida(caminhoImagem);

            // Controllar para caminhos vazios
            if (string.IsNullOrEmpty(caminhoImagem))
                throw new ExceptionImagemDesconhecida(caminhoImagem);

            // Controllar para o caso de nao exister o ficheiro
            if (!File.Exists(caminhoImagem))
                throw new ExceptionImagemDesconhecida(caminhoImagem);

            // Validar tipo de imagem
            if (!TipoImagemValida(caminhoImagem))
                throw new ExceptionImagemDesconhecida(caminhoImagem);

            // Tentar Processar a Imagem
            try
            {
                string dataPath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName).FullName + "\\assets\\tessdata\\";
                ProcessamentoMatriculas processar = new ProcessamentoMatriculas(dataPath);
                // Devolve no formato 00-AA-00 ou AA-00-AA ou 00-00-AA ou AA-00-00
                Matricula = processar.ObterMatriculas(caminhoImagem);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                throw new ExceptionMatriculaDesconhecida();
            }

            view.ProcessamentoTerminado(caminhoImagem);
        }

        public void SolicitarMatricula(ref string matricula)
        {
            matricula = this.Matricula;
        }
    }
}
