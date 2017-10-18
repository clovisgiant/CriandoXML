using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Security;
using Npgsql;

namespace CriandoXML
{
    public partial class frmCliente : Form
    {
        //A form que receberá a variável deve ter a declaração no construtor
        public string TextoTeste { get; set; }


        public frmCliente(string texto)
        {
            InitializeComponent();
            lblTexto.Text = texto;
            DataTable dt;
            DataTable dtxml;
            DataSet ds = new DataSet();
            //DAL acesso = new DAL();
            DALBysoft acesso = new DALBysoft();


            dt = acesso.GetClienteBysoft();
            dataGridViewCliente.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCliente.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridViewCliente.DataSource = dt;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            //Atribuir valor na propriedade
            //  TextoTeste = tbTexto.Text +" "+ lblTexto.Text;
            //Fechar este Form
            this.Close();
        }


        public void dataGridViewagente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCliente.SelectedCells.Count > 0) // Checking to see if any cell is selected
            {
                int mSelectedRowIndex = dataGridViewCliente.SelectedCells[0].RowIndex;

                DataGridViewRow mSelectedRow = dataGridViewCliente.Rows[mSelectedRowIndex];


                string agente = Convert.ToString(mSelectedRow.Cells[0].Value);

                TextoTeste = agente;
                tbTexto.Text = agente;

            }

        }

        private void tbTexto_TextChanged(object sender, EventArgs e)
        {
            (dataGridViewCliente.DataSource as DataTable).DefaultView.RowFilter = string.Format("Nome_Agente LIKE '%{0}%'", tbTexto.Text);
        }



    }
}
