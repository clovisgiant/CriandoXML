namespace CriandoXML
{
    partial class frmtransportador
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
            this.dataGridViewTransportador = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransportador)).BeginInit();
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
            this.tbTexto.Location = new System.Drawing.Point(12, 33);
            this.tbTexto.Name = "tbTexto";
            this.tbTexto.Size = new System.Drawing.Size(168, 26);
            this.tbTexto.TabIndex = 4;
            this.tbTexto.TextChanged += new System.EventHandler(this.tbTexto_TextChanged);
            // 
            // lblInformacao
            // 
            this.lblInformacao.AutoSize = true;
            this.lblInformacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformacao.Location = new System.Drawing.Point(186, 39);
            this.lblInformacao.Name = "lblInformacao";
            this.lblInformacao.Size = new System.Drawing.Size(209, 20);
            this.lblInformacao.TabIndex = 3;
            this.lblInformacao.Text = "Selecione um Transportador";
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(410, 16);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(72, 23);
            this.btnFechar.TabIndex = 5;
            this.btnFechar.Text = "OK";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // dataGridViewTransportador
            // 
            this.dataGridViewTransportador.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTransportador.Location = new System.Drawing.Point(12, 65);
            this.dataGridViewTransportador.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewTransportador.Name = "dataGridViewTransportador";
            this.dataGridViewTransportador.RowTemplate.Height = 24;
            this.dataGridViewTransportador.Size = new System.Drawing.Size(471, 234);
            this.dataGridViewTransportador.TabIndex = 184;
            this.dataGridViewTransportador.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewagente_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 185;
            this.label1.Text = "Pesquisar:";
            // 
            // frmtransportador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 310);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewTransportador);
            this.Controls.Add(this.tbTexto);
            this.Controls.Add(this.lblInformacao);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.lblTexto);
            this.Name = "frmtransportador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transportador";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransportador)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTexto;
        private System.Windows.Forms.TextBox tbTexto;
        private System.Windows.Forms.Label lblInformacao;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.DataGridView dataGridViewTransportador;
        private System.Windows.Forms.Label label1;
    }
}