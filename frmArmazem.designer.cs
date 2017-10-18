namespace CriandoXML
{
    partial class frmArmazem
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
            this.lblTexto = new System.Windows.Forms.Label();
            this.tbTexto = new System.Windows.Forms.TextBox();
            this.lblInformacao = new System.Windows.Forms.Label();
            this.btnFechar = new System.Windows.Forms.Button();
            this.dataGridViewDestino = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDestino)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTexto
            // 
            this.lblTexto.AutoSize = true;
            this.lblTexto.Location = new System.Drawing.Point(173, 82);
            this.lblTexto.Name = "lblTexto";
            this.lblTexto.Size = new System.Drawing.Size(0, 13);
            this.lblTexto.TabIndex = 0;
            // 
            // tbTexto
            // 
            this.tbTexto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTexto.Location = new System.Drawing.Point(12, 37);
            this.tbTexto.Name = "tbTexto";
            this.tbTexto.Size = new System.Drawing.Size(186, 26);
            this.tbTexto.TabIndex = 4;
            this.tbTexto.TextChanged += new System.EventHandler(this.tbTexto_TextChanged);
            // 
            // lblInformacao
            // 
            this.lblInformacao.AutoSize = true;
            this.lblInformacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformacao.Location = new System.Drawing.Point(223, 37);
            this.lblInformacao.Name = "lblInformacao";
            this.lblInformacao.Size = new System.Drawing.Size(164, 20);
            this.lblInformacao.TabIndex = 3;
            this.lblInformacao.Text = "Selecione um Destino";
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(411, 10);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(72, 23);
            this.btnFechar.TabIndex = 5;
            this.btnFechar.Text = "OK";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // dataGridViewDestino
            // 
            this.dataGridViewDestino.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDestino.Location = new System.Drawing.Point(12, 68);
            this.dataGridViewDestino.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewDestino.Name = "dataGridViewDestino";
            this.dataGridViewDestino.RowTemplate.Height = 24;
            this.dataGridViewDestino.Size = new System.Drawing.Size(472, 231);
            this.dataGridViewDestino.TabIndex = 184;
            this.dataGridViewDestino.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewagente_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 185;
            this.label1.Text = "Pesquisar:";
            // 
            // frmArmazem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 310);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewDestino);
            this.Controls.Add(this.tbTexto);
            this.Controls.Add(this.lblInformacao);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.lblTexto);
            this.Name = "frmArmazem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Destino";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDestino)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTexto;
        private System.Windows.Forms.TextBox tbTexto;
        private System.Windows.Forms.Label lblInformacao;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.DataGridView dataGridViewDestino;
        private System.Windows.Forms.Label label1;
    }
}