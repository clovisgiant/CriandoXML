using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;
using Npgsql;



namespace CriandoXML
{

    public partial class Form1 : Form
    {

        static string serverName = "localhost";  //localhost
        static string port = "5432";             //porta default
        static string userName = "postgres";     //nome do administrador
        static string password = "d19m11";     //senha do administrador
        static string databaseName = "giant_novo_producao"; //nome do banco de dados       
        string connString = null;
        bool tabhouse = false;
        bool tabmaster = false;

        public Form1()
        {
            InitializeComponent();          

        }

       

        public void DAL()
        {
             connString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                           serverName, port, userName, password, databaseName);
        }

          
        private void btnCriarXML_Click(object sender, EventArgs e)
        {

            if (tabhouse)
                {
                    DAL update = new DAL();
                    update.AtualizarHouse(texthouse.Text, textcliente.Text, textexportador.Text, comboBoxincoterm.Text, textorigemhouse.Text, textdestinohouse.Text, textagentehouse.Text, textmoedafretehouse.Text, comboBoxtipofrete.Text);
                    //MessageBox.Show("Arquivo XML gerado com sucesso.");
                    GeraXML();                 
                   
                }
            else
                {
                    DAL update2 = new DAL();
                    update2.AtualizarMaster(txtmaster.Text, textqtdprocesso.Text, txtorigem.Text, 
                        txtdestino.Text, txtagente.Text, texttransportador.Text, txtmoedafrete.Text,
                        comboBoxtipo.Text, textvoo.Text, txtemissao.Text, maskedembarqueprev.Text,
                        maskedembarque.Text, textquantidade.Text, textpesobruto.Text, textpesotaxado.Text);
                    //MessageBox.Show("Arquivo XML gerado com sucesso.");
                    GeraXML();
                  
                }         


        }
        private void GeraXML()
        {
            DataTable dt;
            DataTable totaismaster;
            DataTable dtxml;
            DAL acesso = new DAL();
            DALBysoft acessoby = new DALBysoft();

           // DataTable dtBysoft;
           //dtBysoft =  acessoby.GetRegistromasterBysoft();

           //for (int x = 0; x < dtBysoft.Rows.Count; x++)
           //{
           //    dtBysoft.Rows[x].ItemArray[0].ToString().TrimEnd();
           //}

            

            DataTable dthouse;
            //dthouse = acesso.GetRegistrohouse();
            dtxml = acesso.GetMaster_house();
            for (int x = 0; x < dtxml.Rows.Count; x++)
            {
                XmlTextWriter writer = new XmlTextWriter(@"c:\dados\" + dtxml.Rows[x].ItemArray[0].ToString().TrimEnd() + ".xml", null);
                dthouse = acesso.GetRegistrohouse(dtxml.Rows[x].ItemArray[0].ToString().TrimEnd());  

                //inicia o documento xml
                writer.WriteStartDocument();
                //Usa a formatação
                writer.Formatting = Formatting.Indented;
                //Escreve o elemento raiz
                writer.WriteStartElement("Giant");
                writer.WriteAttributeString("tipoprocesso", "aereo");
                //Inicia um elemento 
                dt = acesso.GetRegistromaster(dtxml.Rows[x].ItemArray[0].ToString().TrimEnd());
                totaismaster = acesso.totalmaster(dtxml.Rows[x].ItemArray[0].ToString().TrimEnd());
               

                int totalprocesso = 0;
                if (dt.Rows.Count == 0)
                {

                 
                    writer.WriteStartElement("Master");
                    //Inclui um atributo ao elemento
                    writer.WriteAttributeString("numeromaster", dthouse.Rows[0].ItemArray[2].ToString().TrimEnd());
                    //e sub-elementos
                    // writer.WriteElementString("nrhouses", dt.Rows[0].ItemArray[0].ToString());                 
                    writer.WriteElementString("origem", dthouse.Rows[0].ItemArray[2].ToString());
                    writer.WriteElementString("destino", dthouse.Rows[0].ItemArray[3].ToString());
                    writer.WriteElementString("agente", dthouse.Rows[0].ItemArray[4].ToString());
                    writer.WriteElementString("transportador", dthouse.Rows[0].ItemArray[5].ToString());
                    writer.WriteElementString("moedafrete", dthouse.Rows[0].ItemArray[6].ToString());
                    writer.WriteElementString("tipofrete", dthouse.Rows[0].ItemArray[7].ToString());
                    writer.WriteElementString("nrvoo", dthouse.Rows[0].ItemArray[8].ToString());
                    writer.WriteElementString("prevembarque", dthouse.Rows[0].ItemArray[9].ToString());
                    writer.WriteElementString("embarque", dthouse.Rows[0].ItemArray[10].ToString());
                    writer.WriteElementString("quantidade", dthouse.Rows[0].ItemArray[11].ToString());
                    writer.WriteElementString("pesobruto", dthouse.Rows[0].ItemArray[12].ToString());
                    writer.WriteElementString("pesotaxado", dthouse.Rows[0].ItemArray[13].ToString());
                    writer.WriteElementString("quantidadeprocesso", dthouse.Rows[0].ItemArray[14].ToString());
                    // writer.WriteElementString("nrmaster", dt.Rows[0].ItemArray[15].ToString());
                }
                else
                {

                    int moedamaster = 0;
                    String caseSwitch2;
                    caseSwitch2 = dt.Rows[0].ItemArray[6].ToString().TrimEnd();
                    switch (caseSwitch2)
                    {
                        case "USD":
                            moedamaster = 220;
                            break;
                        case "EUR":
                            moedamaster = 978;
                            break;
                        case "CAD":
                            moedamaster = 165;
                            break;
                        default:
                            moedamaster = 220;
                            Console.WriteLine("moeda nao econtrada");
                            break;
                    }
                    writer.WriteStartElement("Master");
                    //Inclui um atributo ao elemento
                    writer.WriteAttributeString("numeromaster", dt.Rows[0].ItemArray[15].ToString());
                    //e sub-elementos
                    // writer.WriteElementString("nrhouses", dt.Rows[0].ItemArray[0].ToString());
                    writer.WriteElementString("origem", dt.Rows[0].ItemArray[2].ToString());
                    writer.WriteElementString("destino", dt.Rows[0].ItemArray[3].ToString());
                    writer.WriteElementString("agente", dt.Rows[0].ItemArray[4].ToString());
                    writer.WriteElementString("transportador", dt.Rows[0].ItemArray[5].ToString());
                    writer.WriteElementString("moedafrete", moedamaster.ToString());
                    writer.WriteElementString("tipofrete", dt.Rows[0].ItemArray[7].ToString());
                    writer.WriteElementString("nrvoo", dt.Rows[0].ItemArray[8].ToString());
                    writer.WriteElementString("prevembarque", dt.Rows[0].ItemArray[9].ToString());
                    writer.WriteElementString("embarque", dt.Rows[0].ItemArray[10].ToString());
                    writer.WriteElementString("quantidade", totaismaster.Rows[0].ItemArray[0].ToString());
                    writer.WriteElementString("pesobruto", totaismaster.Rows[0].ItemArray[1].ToString());
                    writer.WriteElementString("pesotaxado", totaismaster.Rows[0].ItemArray[2].ToString());
                    writer.WriteElementString("quantidadeprocesso", dthouse.Rows.Count.ToString());
                    // writer.WriteElementString("nrmaster", dt.Rows[0].ItemArray[15].ToString());
                }

                string str = "\"nnr123785458\"";
                string str1 = "teste";
                //encerra os elementos itens
                writer.WriteEndElement();
                // encerra o item
                //escreve alguns espaços entre os nodes
                writer.WriteWhitespace("");
                for (int d = 0; d < dthouse.Rows.Count; d++)
                {
                    const string quote = "\"";
                    string caseSwitch = dthouse.Rows[d].ItemArray[10].ToString().TrimEnd();
                    int moeda = 0;
                    string tipofrete = "EXW";
                    string iconterm = "EXW";

                    switch (caseSwitch)
                    {
                        case "USD":
                            moeda = 220;
                            break;
                        case "EUR":
                            moeda = 978;
                            break;
                        case "CAD":
                            moeda = 165;
                            break;
                        default:
                            Console.WriteLine("moeda nao econtrada");
                            break;
                    }


                    //escreve um segundo elemento usando um raw de dados strings
                       writer.WriteRaw("<House nrhouse=" + quote + dthouse.Rows[d].ItemArray[1].ToString().TrimEnd() + quote + ">" +
                        "<nrmaster>" + dthouse.Rows[0].ItemArray[2].ToString().TrimEnd() + "</nrmaster>" 
                        + "<cliente>" + dthouse.Rows[d].ItemArray[3].ToString().TrimEnd() + "</cliente>" +
                        "<exportador>" + dthouse.Rows[d].ItemArray[4].ToString().TrimEnd() + "</exportador>" +
                        "<icoterm>" + iconterm + "</icoterm>" + 
                        "<origem>" + dthouse.Rows[d].ItemArray[6].ToString().TrimEnd() + "</origem>" + 
                        "<destino>" + dthouse.Rows[d].ItemArray[7].ToString().TrimEnd() + "</destino>" + 
                        "<agente>" + dthouse.Rows[d].ItemArray[8].ToString().TrimEnd() + "</agente>" +
                        //"<transportador>" + dthouse.Rows[d].ItemArray[9].ToString().TrimEnd() +
                        //"</transportador>" + 
                        "<moedafrete>" + moeda.ToString().TrimEnd() +
                        "</moedafrete>" + "<tipofrete>" + tipofrete +
                        "</tipofrete>" + 
                        //"<emissaoconhecimento>" + dt.Rows[0].ItemArray[9].ToString().TrimEnd() +
                        //"</emissaoconhecimento>" + "<prevembarque>" + dt.Rows[0].ItemArray[9].ToString() +
                        //"</prevembarque>" + "<embarque>" +  dt.Rows[0].ItemArray[10].ToString() + "</embarque>" + 
                        "<quantidade>" + dthouse.Rows[d].ItemArray[16].ToString().TrimEnd() + "</quantidade>" + 
                        "<pesobruto>" + dthouse.Rows[d].ItemArray[17].ToString().TrimEnd() + "</pesobruto>" +
                        "<pesovolumetrico>" + dthouse.Rows[d].ItemArray[18].ToString().TrimEnd() + "</pesovolumetrico>" + 
                        //"<quantidadeprocesso>" + dthouse.Rows.Count.ToString().TrimEnd() + "</quantidadeprocesso>" +
                        "<tar_vendamin>" + "5" + "</tar_vendamin>" + "</House>");


                     

                }

                writer.WriteFullEndElement();
                writer.Close();
                dthouse.Dispose();
                dt.Dispose();
                totaismaster.Dispose();
            }
            //escreve o XML para o arquivo e fecha o escritor            
            MessageBox.Show("Arquivo XML gerado com sucesso.");           
           
        }

        private void btnCriarXML2_Click(object sender, EventArgs e)
        {
            GeraXML();
        }

        private void btnLerXML_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();            

            DirectoryInfo d = new DirectoryInfo(@"C:\dados");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
              // str = str + ", " + file.Name;
                //DataSet ds = new DataSet();
                ds.ReadXml(@"c:\dados\" + file.Name);
            }


            
          
            //DataTable tempDT = new DataTable();  
            //tempDT = ds.Tables[1].DefaultView.ToTable(true, "numeromaster", "Agente");
            ////Now bind this to DataGridView  
           
            dgvXML.DataSource = ds.Tables[1].DefaultView;
            dgvXML.Columns["numeromaster"].DisplayIndex = 0;
            dgvXML.Columns["agente"].DisplayIndex = 1;
          

           
        }

        private void dgvXML_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvXML.SelectedCells.Count > 0) // Checking to see if any cell is selected
            {
                int mSelectedRowIndex = dgvXML.SelectedCells[0].RowIndex;

                DataGridViewRow mSelectedRow = dgvXML.Rows[mSelectedRowIndex];

                string origem = Convert.ToString(mSelectedRow.Cells[0].Value);
                string destino = Convert.ToString(mSelectedRow.Cells[1].Value);
                string agente = Convert.ToString(mSelectedRow.Cells[2].Value);               
                string transportador = Convert.ToString(mSelectedRow.Cells[3].Value);
                string moedafrete = Convert.ToString(mSelectedRow.Cells[4].Value);
                string tipofrete = Convert.ToString(mSelectedRow.Cells[5].Value);
                string nrvoo = Convert.ToString(mSelectedRow.Cells[6].Value);
                string prevembarque = Convert.ToString(mSelectedRow.Cells[7].Value);
                string embarque = Convert.ToString(mSelectedRow.Cells[8].Value);
                string quantidade = Convert.ToString(mSelectedRow.Cells[9].Value);
                string pesobruto = Convert.ToString(mSelectedRow.Cells[10].Value);
                string pesotaxado = Convert.ToString(mSelectedRow.Cells[11].Value);
                string quantidadeprocesso = Convert.ToString(mSelectedRow.Cells[12].Value);
                string numeroMaster = Convert.ToString(mSelectedRow.Cells[13].Value);
                txtorigem.Text = origem;
                txtdestino.Text = destino;
                txtagente.Text = agente;
                texttransportador.Text = transportador;
                txtmoedafrete.Text = moedafrete;
                comboBoxtipo.SelectedText = " ";
                textvoo.Text = nrvoo;
                maskedembarqueprev.Text = prevembarque;
                maskedembarque.Text = embarque;
                txtemissao.Text = embarque;
                textquantidade.Text = quantidade;
                textpesobruto.Text = pesobruto;
                textpesotaxado.Text = pesotaxado;
                txtmaster.Text = numeroMaster;
                textqtdprocesso.Text = quantidadeprocesso;

                 DAL houses = new DAL();                
                 houses.GetRegistroHouses(numeroMaster);
                 DataSet ds = new DataSet();                 
                
                dataGridView1.DataSource = houses.GetRegistroHouses(numeroMaster).DefaultView;
                dataGridView1.Columns["nrhouse"].DisplayIndex = 0;
                dataGridView1.Columns["nrmaster"].DisplayIndex = 1;
                
              
            }
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0) // Checking to see if any cell is selected
            {
                int mSelectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

                DataGridViewRow mSelectedRow = dataGridView1.Rows[mSelectedRowIndex];

                string cliente = Convert.ToString(mSelectedRow.Cells[1].Value);
                string exportador = Convert.ToString(mSelectedRow.Cells[2].Value);
                string icoterm = Convert.ToString(mSelectedRow.Cells[3].Value);
                string origemhouse = Convert.ToString(mSelectedRow.Cells[4].Value);
                string destinohouse = Convert.ToString(mSelectedRow.Cells[5].Value);
                string agentehouse = Convert.ToString(mSelectedRow.Cells[6].Value);
                string transportadorhouse = Convert.ToString(mSelectedRow.Cells[7].Value);
                string moedafretehouse = Convert.ToString(mSelectedRow.Cells[8].Value);
                string tipofretehouse = Convert.ToString(mSelectedRow.Cells[9].Value);
                string nrvoohouse = Convert.ToString(mSelectedRow.Cells[10].Value);
                string emissaoconhecimentohouse = Convert.ToString(mSelectedRow.Cells[11].Value);
                string prevembarquehouse = Convert.ToString(mSelectedRow.Cells[12].Value);
                string embarquehouse = Convert.ToString(mSelectedRow.Cells[13].Value);
                string nrmaster = Convert.ToString(mSelectedRow.Cells[0].Value);
                string pesobrutohouse = Convert.ToString(mSelectedRow.Cells[15].Value);
                string quantidadeprocessohouse = Convert.ToString(mSelectedRow.Cells[14].Value);
                string pesotaxadohouse = Convert.ToString(mSelectedRow.Cells[16].Value);
                string tar_vendamin = Convert.ToString(mSelectedRow.Cells[17].Value);
                string tar_vendamin2 = Convert.ToString(mSelectedRow.Cells[18].Value);
                string nrhouse = Convert.ToString(mSelectedRow.Cells[18].Value);

                texthouse.Text = nrhouse;
                textvenda.Text = "5";
                textcliente.Text = cliente;
                textexportador.Text = exportador;
                textorigemhouse.Text = origemhouse;
                textdestinohouse.Text = destinohouse;
                textgateway.Text = destinohouse;
                textmoedafretehouse.Text = moedafretehouse;
                comboBoxincoterm.Text = icoterm;
                textagentehouse.Text = agentehouse;
                comboBoxtipofrete.Text = tipofretehouse;
                checkBoxvenda.Checked = true;

                
               
            }
        }

        public void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tbOperation")
            {
                tabhouse = true;
                tabmaster = false;
                //DAL acesso = new DAL();
                //acesso.AtualizarHouse();      
                //Application.Exit();
            }
            else
            {
                tabmaster = true;
                tabhouse = false;
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            frmSecundaria novaForm = new frmSecundaria(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
           // lblTextoRecebido.Text = novaForm.TextoTeste;
            textagentehouse.Text = novaForm.TextoTeste;
          
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmSecundaria novaForm = new frmSecundaria(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
            txtagente.Text = novaForm.TextoTeste;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            frmArmazem novaForm = new frmArmazem(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
            //lblTextoRecebido.Text = novaForm.TextoTeste;
            txtdestino.Text = novaForm.TextoTeste;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmtransportador novaForm = new frmtransportador(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
           // lblTextoRecebido.Text = novaForm.TextoTeste;
            texttransportador.Text = novaForm.TextoTeste;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            frmCliente novaForm = new frmCliente(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();
            textcliente.Text = novaForm.TextoTeste;

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
            //lblTextoRecebido.Text = novaForm.TextoTeste;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            frmArmazem novaForm = new frmArmazem(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
           // lblTextoRecebido.Text = novaForm.TextoTeste;
            textorigemhouse.Text = novaForm.TextoTeste;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            frmExportador novaForm = new frmExportador(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
            textexportador.Text = novaForm.TextoTeste;
            //lblTextoRecebido.Text = novaForm.TextoTeste;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            frmArmazem novaForm = new frmArmazem(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
           // lblTextoRecebido.Text = novaForm.TextoTeste;
            textdestinohouse.Text = novaForm.TextoTeste;
            textgateway.Text = novaForm.TextoTeste;

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            frmArmazem novaForm = new frmArmazem(textagentehouse.Text);
            novaForm.ShowDialog();
            //novaForm.Show();

            //Pegando o valor da propriedade da Form novaForm e colocando no Label
            // lblTextoRecebido.Text = novaForm.TextoTeste;
            txtorigem.Text = novaForm.TextoTeste;
        } 
              
        
    }
}
