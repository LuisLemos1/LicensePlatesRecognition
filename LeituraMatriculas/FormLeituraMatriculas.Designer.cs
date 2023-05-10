using System;

namespace LeituraMatriculas
{
    partial class FormLeituraMatriculas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.botaoCarregarImagem = new System.Windows.Forms.Button();
            this.caixaImagem = new System.Windows.Forms.PictureBox();
            this.botaoEncerrar = new System.Windows.Forms.Button();
            this.textoMatricula = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.caixaImagem)).BeginInit();
            this.SuspendLayout();
            // 
            // botaoCarregarImagem
            // 
            this.botaoCarregarImagem.Location = new System.Drawing.Point(549, 28);
            this.botaoCarregarImagem.Name = "botaoCarregarImagem";
            this.botaoCarregarImagem.Size = new System.Drawing.Size(189, 46);
            this.botaoCarregarImagem.TabIndex = 3;
            this.botaoCarregarImagem.Text = "Carregar Imagem";
            this.botaoCarregarImagem.UseVisualStyleBackColor = true;
            this.botaoCarregarImagem.Click += new System.EventHandler(this.botaoCarregarImagem_Click);
            // 
            // caixaImagem
            // 
            this.caixaImagem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.caixaImagem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.caixaImagem.Location = new System.Drawing.Point(19, 11);
            this.caixaImagem.Name = "caixaImagem";
            this.caixaImagem.Size = new System.Drawing.Size(472, 386);
            this.caixaImagem.TabIndex = 4;
            this.caixaImagem.TabStop = false;
            // 
            // botaoEncerrar
            // 
            this.botaoEncerrar.BackColor = System.Drawing.Color.LightSlateGray;
            this.botaoEncerrar.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botaoEncerrar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.botaoEncerrar.Location = new System.Drawing.Point(550, 113);
            this.botaoEncerrar.Name = "botaoEncerrar";
            this.botaoEncerrar.Size = new System.Drawing.Size(188, 47);
            this.botaoEncerrar.TabIndex = 5;
            this.botaoEncerrar.Text = "Encerrar Programa";
            this.botaoEncerrar.UseVisualStyleBackColor = false;
            this.botaoEncerrar.Click += new System.EventHandler(this.botaoEncerrar_Click);
            // 
            // textoMatricula
            // 
            this.textoMatricula.AutoEllipsis = true;
            this.textoMatricula.AutoSize = true;
            this.textoMatricula.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textoMatricula.Location = new System.Drawing.Point(557, 279);
            this.textoMatricula.Name = "textoMatricula";
            this.textoMatricula.Size = new System.Drawing.Size(170, 15);
            this.textoMatricula.TabIndex = 6;
            this.textoMatricula.Text = "Bem vindo! Carregue uma imagem";
            this.textoMatricula.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormLeituraMatriculas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(800, 415);
            this.ControlBox = false;
            this.Controls.Add(this.textoMatricula);
            this.Controls.Add(this.botaoEncerrar);
            this.Controls.Add(this.caixaImagem);
            this.Controls.Add(this.botaoCarregarImagem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FormLeituraMatriculas";
            this.Text = "Leitura de Matriculas";
            this.Load += new System.EventHandler(this.FormLeituraMatriculas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.caixaImagem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button botaoCarregarImagem;
        private System.Windows.Forms.PictureBox caixaImagem;
        private System.Windows.Forms.Button botaoEncerrar;
        private System.Windows.Forms.Label textoMatricula;
    }
}

