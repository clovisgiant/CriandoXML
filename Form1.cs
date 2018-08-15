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
using System.Drawing;
using System.Xml.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

using System.Diagnostics;



namespace CriandoXML
{

    public partial class Form1 : Form
    {

        static string serverName = "192.168.66.44";  //localhost
        static string port = "5432";             //porta default
        static string userName = "postgres";     //nome do administrador
        static string password = "d19m11";     //senha do administrador
        static string databaseName = "agenciamento"; //nome do banco de dados       
        string connString = null;
        bool tabhouse = false;
        bool tabmaster = false;
        string ftpIPServidor;
        string ftpUsuarioID;
        string ftpSenha;


       
        public Form1()
        {
            InitializeComponent();          

        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            //if the item state is selected them change the back color 
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.Yellow);//Choose the color

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Draw the current item text
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void listBox1_DrawItemverde(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            //if the item state is selected them change the back color 
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.Green);//Choose the color

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Draw the current item text
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        public string m_CurUser;

        public string CurrentUser()
        {

            {
                if (string.IsNullOrEmpty(m_CurUser))
                {
                    var who = System.Security.Principal.WindowsIdentity.GetCurrent();
                    if (who == null)
                        m_CurUser = System.Environment.UserName;
                    else
                        //m_CurUser = who.Name;
                        m_CurUser = System.Environment.UserName;

                }
                return m_CurUser;
            }
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
                    DAL houses = new DAL();
                    update.AtualizarHouse(texthouse.Text, textcliente.Text, textexportador.Text, comboBoxincoterm.Text, textorigemhouse.Text, textdestinohouse.Text, textagentehouse.Text, textmoedafretehouse.Text, comboBoxtipofrete.Text);
                    //MessageBox.Show("Arquivo XML gerado com sucesso.");
              

                update.AtualizarAlertaMaster(texthouse.Text, textorigemhouse.Text, textdestinohouse.Text, textagentehouse.Text, textmoedafretehouse.Text, comboBoxtipofrete.Text);
                    
                    GeraXML();
                    string user = CurrentUser();
                    dataGridView1.DataSource = houses.GetRegistroHouses(txtmaster.Text, user).DefaultView;                
               
                }
            else
                {
                    DAL update2 = new DAL();
                    update2.AtualizarMaster(txtmaster.Text, textqtdprocesso.Text, txtorigem.Text, 
                        txtdestino.Text, txtagente.Text, texttransportador.Text, txtmoedafrete.Text,
                        comboBoxtipo.Text, textvoo.Text, txtemissao.Text, maskedembarqueprev.Text,
                        maskedembarque.Text, textquantidade.Text, textpesobruto.Text, textpesotaxado.Text);
                    //MessageBox.Show("Arquivo XML gerado com sucesso.");

                    update2.AtualizarAlertaMaster(txtmaster.Text,  txtorigem.Text, txtdestino.Text, txtagente.Text, txtmoedafrete.Text, comboBoxtipo.Text);
              
                
                    GeraXML();

                    DataSet ds = new DataSet();
                    //IEnumerable<string> arquivos = GetAllFilesRecursively(@"C:\dados");
                    DirectoryInfo d = new DirectoryInfo(@"C:\dados");//Assuming Test is your Folder
                    FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories);
                    //  FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories); //Getting Text files
                    string str = "";
                    foreach (FileInfo file in Files)
                    {
                        // str = str + ", " + file.Name;
                        //DataSet ds = new DataSet();
                        ds.ReadXml(@"c:\dados\" + file.Name);

                    }


                    dgvXML.DataSource = ds.Tables[1].DefaultView;
                    dgvXML.Columns["numeromaster"].DisplayIndex = 0;
                    dgvXML.Columns["agente"].DisplayIndex = 1;
                    
                  
                }         


        }
        private void GeraXML()
        {
            DataTable dt;
            DataTable dtdestino;
            DataTable dtdestinoMaster;
            DataTable totaismaster;
            DataTable depara; 
            DataTable dtxml;
            DAL acesso = new DAL();
            DALBysoft acessoby = new DALBysoft();  
             List<string> _items = new List<string>(); // <-- Add this     
             string user;
             user = CurrentUser(); //pega o usuario da aplicação
            DataTable dthouse;
            dtxml = acesso.GetMaster_house(user);
            for (int x = 0; x < dtxml.Rows.Count; x++)
            {
                XmlTextWriter writer = new XmlTextWriter(@"c:\dados\" + dtxml.Rows[x].ItemArray[0].ToString().TrimEnd() + ".xml", null);
                dthouse = acesso.GetRegistrohouse(dtxml.Rows[x].ItemArray[0].ToString().TrimEnd(),user);

               

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

                string unidOperacional = "000001";
                string servico = "000007";
                int totalprocesso = 0;
                if (dt.Rows.Count == 0)
                {
                 
                    writer.WriteStartElement("Master");
                    //Inclui um atributo ao elemento
                    writer.WriteAttributeString("numeromaster", dthouse.Rows[0].ItemArray[2].ToString().TrimEnd());
                    //e sub-elementos
                    // writer.WriteElementString("nrhouses", dt.Rows[0].ItemArray[0].ToString());
   
                    //depara = acesso.GetRegistroDEPARA(dthouse.Rows[0].ItemArray[2].ToString());

                    //if (depara.Rows.Count > 0)
                    //{
                    //    writer.WriteElementString("origem", depara.Rows[2].ToString().TrimEnd());
                    //}
                    //else
                    //{
                    // writer.WriteElementString("origem", dthouse.Rows[0].ItemArray[2].ToString().TrimEnd());
                       
                    //}

                    writer.WriteElementString("origem", dthouse.Rows[0].ItemArray[2].ToString().TrimEnd());
                    writer.WriteElementString("destino", dthouse.Rows[0].ItemArray[3].ToString().TrimEnd());
                    writer.WriteElementString("agente", dthouse.Rows[0].ItemArray[4].ToString().TrimEnd());
                    writer.WriteElementString("transportador", dthouse.Rows[0].ItemArray[5].ToString().TrimEnd());
                    writer.WriteElementString("moedafrete", dthouse.Rows[0].ItemArray[6].ToString().TrimEnd());
                    writer.WriteElementString("tipofrete", dthouse.Rows[0].ItemArray[7].ToString().TrimEnd());
                    writer.WriteElementString("nrvoo", dthouse.Rows[0].ItemArray[8].ToString().TrimEnd());
                    writer.WriteElementString("prevembarque", dthouse.Rows[0].ItemArray[9].ToString().TrimEnd());
                    writer.WriteElementString("embarque", dthouse.Rows[0].ItemArray[10].ToString().TrimEnd());
                    writer.WriteElementString("quantidade", dthouse.Rows[0].ItemArray[11].ToString().TrimEnd());
                    writer.WriteElementString("pesobruto", dthouse.Rows[0].ItemArray[12].ToString().TrimEnd());
                    writer.WriteElementString("pesotaxado", dthouse.Rows[0].ItemArray[13].ToString().TrimEnd());
                    writer.WriteElementString("quantidadeprocesso", dthouse.Rows[0].ItemArray[14].ToString().TrimEnd());
                    writer.WriteElementString("tarifahouse", dthouse.Rows[0].ItemArray[16].ToString().TrimEnd());
                    writer.WriteElementString("unidoperacional", unidOperacional);
                    writer.WriteElementString("servico", servico);

                 
                }
                else
                {

                    int moedamaster = 0;
                    String caseSwitch2;                   
                    unidOperacional = "000001";
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
                        case "SGD":
                            moedamaster = 195;
                            break;

                        case "JPY":
                            moedamaster = 470;
                            break;  
                        case "CNY":
                            moedamaster = 156;                          
                            break;
                        case "GBP":
                            moedamaster = 540;
                            break;
                        default:
                            moedamaster = 220;
                            Console.WriteLine("moeda nao econtrada");
                            break;
                    }
                    writer.WriteStartElement("Master");
                    //Inclui um atributo ao elemento
                    writer.WriteAttributeString("numeromaster", dt.Rows[0].ItemArray[15].ToString());



                    //if (x < 1)  -------------------Alertas para campo origem-----------------------
                    //{
                        DAL acessobysoft = new DAL();
                        dtdestino = acessobysoft.GetDestinomaster(dt.Rows[0].ItemArray[2].ToString(),user);
                        if (dtdestino.Rows.Count > 0)
                        {
                            writer.WriteElementString("origem", dtdestino.Rows[0].ItemArray[0].ToString());

                            acesso.InserirAlertaMaster("Foi encontrado mais de um registro para  Origem:" + dt.Rows[0].ItemArray[2].ToString()+" no master: "+ dt.Rows[0].ItemArray[15].ToString(),dt.Rows[0].ItemArray[15].ToString());
                            //MessageBox.Show("Foi encontrado mais de um registro para campo Origem no master"); 
                        }
                        else
                        {
                         
                             DAL acessobysoft2 = new DAL();

                             dtdestinoMaster = acessobysoft2.GetDestinomaster(dt.Rows[0].ItemArray[15].ToString(),user);


                             if (dtdestinoMaster.Rows[0].ItemArray[0].ToString().Trim().Length > 3)
                             {
                                 acesso.InserirAlertaMaster(" favor informar o codigo para  Origem: " + dt.Rows[0].ItemArray[2].ToString() + " no master: " + dt.Rows[0].ItemArray[15].ToString(), dt.Rows[0].ItemArray[15].ToString());
                             }

                             // dtdestinoMaster = acessobysoft2.GetDestinomaster(dt.Rows[0].ItemArray[15].ToString().Trim());

                             writer.WriteElementString("origem", dtdestinoMaster.Rows[0].ItemArray[0].ToString().TrimEnd());

                        }
                        dtdestino.Dispose();
                   
                        
                        // }------------------------------------------------------------------------------------------- DE PARA Emplementar

                        //depara = acesso.GetRegistroDEPARA(dtdestino.Rows[0].ItemArray[0].ToString());

                        //if (depara.Rows.Count > 0)
                        //{
                        //    writer.WriteElementString("origem", depara.Rows[2].ToString().TrimEnd());
                        //}
                        //else
                        //{
                        //    writer.WriteElementString("origem", dthouse.Rows[0].ItemArray[2].ToString().TrimEnd());

                        //}


                        // }------------------------------------------------------------------------------------------- DE PARA Emplementar
                   // writer.WriteElementString("origem", dt.Rows[0].ItemArray[2].ToString());
                     writer.WriteElementString("destino", dt.Rows[0].ItemArray[3].ToString().TrimEnd());
                     writer.WriteElementString("agente", dt.Rows[0].ItemArray[4].ToString().TrimEnd());
                     writer.WriteElementString("transportador", dt.Rows[0].ItemArray[5].ToString().TrimEnd());
                     writer.WriteElementString("moedafrete", moedamaster.ToString().TrimEnd());
                     writer.WriteElementString("tipofrete", dt.Rows[0].ItemArray[7].ToString().TrimEnd());
                     writer.WriteElementString("nrvoo", dt.Rows[0].ItemArray[8].ToString().TrimEnd());
                     writer.WriteElementString("prevembarque", dt.Rows[0].ItemArray[9].ToString().TrimEnd());
                     writer.WriteElementString("embarque", dt.Rows[0].ItemArray[10].ToString().TrimEnd());
                     writer.WriteElementString("quantidade", totaismaster.Rows[0].ItemArray[0].ToString().TrimEnd());
                     writer.WriteElementString("pesobruto", totaismaster.Rows[0].ItemArray[1].ToString().TrimEnd());
                     writer.WriteElementString("pesotaxado", totaismaster.Rows[0].ItemArray[2].ToString().TrimEnd());
                     writer.WriteElementString("quantidadeprocesso", dthouse.Rows.Count.ToString().TrimEnd());
                     writer.WriteElementString("tarifamaster", dt.Rows[0].ItemArray[16].ToString().TrimEnd());
                     writer.WriteElementString("nrmaster", dt.Rows[0].ItemArray[15].ToString().TrimEnd());
                     writer.WriteElementString("unidoperacional", unidOperacional);
                     writer.WriteElementString("servico", servico);
                    
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
                    string caseSwitch = dthouse.Rows[d].ItemArray[9].ToString().TrimEnd();
                    int moeda = 0;
                    string tipofrete = dthouse.Rows[d].ItemArray[10].ToString().TrimEnd();
                    string iconterm = dthouse.Rows[d].ItemArray[4].ToString().TrimEnd();

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
                        case "SGD":
                            moeda = 195;
                            break;
                        case "JPY":
                            moeda = 470;
                            break;
                        case "CNY":
                            moeda = 156;
                            break;
                        case "GBP":
                            moeda = 540;
                            break;
                        default:
                            moeda = 220;
                            Console.WriteLine("moeda nao econtrada");
                            break;
                    }


                    //escreve um segundo elemento usando um raw de dados strings

                   
                    writer.WriteRaw("<House nrhouse=" + quote + dthouse.Rows[d].ItemArray[0].ToString().TrimEnd() + quote + ">" +
                        "<nrmaster>" + dthouse.Rows[0].ItemArray[1].ToString().TrimEnd() + "</nrmaster>" 
                        + "<cliente>" + dthouse.Rows[d].ItemArray[2].ToString().TrimEnd() + "</cliente>" +
                        "<exportador>" + dthouse.Rows[d].ItemArray[3].ToString().TrimEnd() + "</exportador>" +
                        "<incoterm>" + iconterm + "</incoterm>" + 
                        "<origem>" + dthouse.Rows[d].ItemArray[5].ToString().TrimEnd() + "</origem>" + 
                        "<destino>" + dthouse.Rows[d].ItemArray[6].ToString().TrimEnd() + "</destino>" + 
                        "<agente>" + dthouse.Rows[d].ItemArray[7].ToString().TrimEnd() + "</agente>" +
                        //"<transportador>" + dthouse.Rows[d].ItemArray[9].ToString().TrimEnd() +
                        //"</transportador>" + 
                        "<moedafrete>" + moeda.ToString().TrimEnd() +
                        "</moedafrete>" + "<tipofrete>" + tipofrete +
                        "</tipofrete>" + 
                        //"<emissaoconhecimento>" + dt.Rows[0].ItemArray[9].ToString().TrimEnd() +
                        //"</emissaoconhecimento>" + "<prevembarque>" + dt.Rows[0].ItemArray[9].ToString() +
                        //"</prevembarque>" + "<embarque>" +  dt.Rows[0].ItemArray[10].ToString() + "</embarque>" + 
                        "<quantidade>" + dthouse.Rows[d].ItemArray[15].ToString().TrimEnd() + "</quantidade>" + 
                        "<pesobruto>" + dthouse.Rows[d].ItemArray[16].ToString().TrimEnd() + "</pesobruto>" +
                        "<pesovolumetrico>" + dthouse.Rows[d].ItemArray[17].ToString().TrimEnd() + "</pesovolumetrico>" + 
                        //"<quantidadeprocesso>" + dthouse.Rows.Count.ToString().TrimEnd() + "</quantidadeprocesso>" +
                        "<tar_vendamin>" + dthouse.Rows[d].ItemArray[18].ToString().TrimEnd() + "</tar_vendamin>" + "</House>");

                       _items.Add("-------------> INICIO <------------------");
                       _items.Add(dthouse.Rows[d].ItemArray[1].ToString().TrimEnd() + "--> Numero House");
                       _items.Add(dthouse.Rows[d].ItemArray[2].ToString().TrimEnd() + "--> Numero Master");
                       _items.Add(dthouse.Rows[d].ItemArray[3].ToString().TrimEnd() + "--> Falta informar o codigo do cliente");
                       _items.Add(dthouse.Rows[d].ItemArray[4].ToString().TrimEnd() + "--> Falta informar o codigo do Exportador");
                       _items.Add(dthouse.Rows[d].ItemArray[6].ToString().TrimEnd() + "--> Falta informar o codigo da Origem");
                       _items.Add(dthouse.Rows[d].ItemArray[8].ToString().TrimEnd() + "--> Falta informar o codigo do Agente");
                       _items.Add("-------------> FIM <------------------");
                       listBox1.DrawMode = DrawMode.OwnerDrawFixed;
                       listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);

                     //  listBox1.DataSource = _items;
                }
                     
               
                writer.WriteFullEndElement();
                writer.Close();
                dthouse.Dispose();
                dt.Dispose();
                totaismaster.Dispose();
               
            }
            //escreve o XML para o arquivo e fecha o escritor 
            //DAL del = new DAL();
           // del.DeletaArquivosMaster();
           // del.DeletaMaster();
           // del.DeletaHouse();
           // MessageBox.Show("Arquivo XML gerado com sucesso.");

          

          


        //    //Função que envia email--------------------------------------------------------------------------

            //SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            //var mail = new MailMessage();
            //mail.From = new MailAddress("manecasps@hotmail.com");
            //mail.To.Add("clovis.eduardosilva23@gmail.com");
            ////mail.To.Add("alba@giantcargo.com.br");
            //mail.Subject = "Segunda etapa concluida verificar o Agente e Exportador";
            //mail.IsBodyHtml = true;
            //string htmlBody = " ";
            //string texto = "";
            //for (int y = 0; y < _items.Count; y++)
            //{
               
            //   // htmlBody += "<table border=0 cellpadding=0 cellspacing=0><tbody><tr><td><a href=http://htmlemail.io target=_blank>" + _items[y] + "</a> </td></tr></tbody></table><p>This is a really simple email template. Its sole purpose is to get the recipient to click the button with no distractions.</p><p>Good luck! Hope it works.</p>";
            //    texto += "<p>" + y + " --> " + _items[y] + "</p>";
            //}

            //mail.Body = texto;
            //SmtpServer.Port = 587;
            //SmtpServer.UseDefaultCredentials = false;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("manecasps@hotmail.com", "anapaula23");
            //SmtpServer.EnableSsl = true;
            //SmtpServer.Send(mail);

        //    // FIm  da Função ---------------------------------------------------------------------------------

           
        }

        private void btnCriarXML2_Click(object sender, EventArgs e)
        {
            GeraXML();        
       
        }

        private void btnLerXML_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            //IEnumerable<string> arquivos = GetAllFilesRecursively(@"C:\dados");
            DirectoryInfo d = new DirectoryInfo(@"C:\dados");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories);
          //  FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
              // str = str + ", " + file.Name;
                //DataSet ds = new DataSet();
                ds.ReadXml(@"c:\dados\" + file.Name);

            }  
                 
           
            dgvXML.DataSource = ds.Tables[1].DefaultView;
            dgvXML.Columns["numeromaster"].DisplayIndex = 0;
            dgvXML.Columns["agente"].DisplayIndex = 1;

           
            //void GetFiles()
            //    {
            //        DirectoryInfo d= new DirectoryInfo(@"c:\dados\");
            //       //file extension for pdf
            //        var files = d.GetFiles("*.xml*");
            //        FileInfo[] subfileInfo = files.ToArray<FileInfo>();

            //        if (subfileInfo.Length > 0)
            //        {
            //            for (int j = 0; j < subfileInfo.Length; j++)
            //            {
            //                bool isHidden = ((File.GetAttributes(subfileInfo[j].FullName) & FileAttributes.Hidden) == FileAttributes.Hidden);
            //                if (!isHidden)
            //                {
            //                    string strExtention = th.GetExtension(subfileInfo[j].FullName);
            //                    if (strExtention.Contains("pdf"))
            //                    {                            
            //                        string path = subfileInfo[j].FullName;
            //                        string name = bfileInfo[j].Name;                           
            //                    }
            //                }
            //            }
            //        }ol, vcxsacv b;ço87ewq    


           
        }


        public static IEnumerable<string> GetAllFilesRecursively(string inputFolder)
        {
            var queue = new Queue<string>();
            queue.Enqueue(inputFolder);
            while (queue.Count > 0)
            {
                inputFolder = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(inputFolder))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("GetAllFilesRecursively: " + ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(inputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("GetAllFilesRecursively: " + ex);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
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
                string numeroMaster = Convert.ToString(mSelectedRow.Cells[14].Value);
                string tarifamaster = Convert.ToString(mSelectedRow.Cells[13].Value);
                txtorigem.Text = origem;
                txtdestino.Text = destino;
                txtagente.Text = agente;
                texttransportador.Text = transportador;
                txtmoedafrete.Text = moedafrete;
                comboBoxtipo.Text  = tipofrete;
                textvoo.Text = nrvoo;
                maskedembarqueprev.Text = prevembarque;
                maskedembarque.Text = embarque;
                txtemissao.Text = embarque;
                textquantidade.Text = quantidade;
                textpesobruto.Text = pesobruto;
                textpesotaxado.Text = pesotaxado;
                txtmaster.Text = numeroMaster;
                textqtdprocesso.Text = quantidadeprocesso;
                txttotalmaster.Text = tarifamaster;

                 DAL houses = new DAL();                
                 //houses.GetRegistroHouses(numeroMaster);
                 DataSet ds = new DataSet();
                 string user;
                 user = CurrentUser();

                 dataGridView1.DataSource = houses.GetRegistroHouses(numeroMaster, user).DefaultView;
                dataGridView1.Columns["nrhouse"].DisplayIndex = 0;
                dataGridView1.Columns["nrmaster"].DisplayIndex = 1;

                
                DAL Alertas = new DAL();
                DataTable alertasprocesso;               

                    alertasprocesso = Alertas.GetRegistroAlertas(numeroMaster);

                    if (alertasprocesso.Rows.Count > 0)
                    {

                        List<string> list = alertasprocesso.AsEnumerable()
                                 .Select(r => r.Field<string>("descricao"))
                                 .ToList();

                        listBox1.DrawMode = DrawMode.OwnerDrawFixed;
                        listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
                        listBox1.DataSource = list;

                        string textoalerta = "";
                        for (int x = 0; x < alertasprocesso.Rows.Count; x++)
                        {

                            //htmlBody = "<table border=0 cellpadding=0 cellspacing=0><tbody><tr><td><a href=http://htmlemail.io target=_blank>" + _items[x] + "</a> </td></tr></tbody></table><p>This is a really simple email template. Its sole purpose is to get the recipient to click the button with no distractions.</p><p>Good luck! Hope it works.</p>";
                            textoalerta += "<p>" + x + " --> " + alertasprocesso.Rows[x].ItemArray[0].ToString() + "</p>";
                        }

                        alertasprocesso.Dispose();


                    }
                    else
                    {
                       

                        listBox1.DrawMode = DrawMode.OwnerDrawFixed;
                        listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItemverde);
                        listBox1.Refresh();
                       // listBox1.Items.Add("Processos estão ok , para ser enviados para Bysoft ");

                        string textoalerta = "";
                        for (int x = 0; x < alertasprocesso.Rows.Count; x++)
                        {

                            //htmlBody = "<table border=0 cellpadding=0 cellspacing=0><tbody><tr><td><a href=http://htmlemail.io target=_blank>" + _items[x] + "</a> </td></tr></tbody></table><p>This is a really simple email template. Its sole purpose is to get the recipient to click the button with no distractions.</p><p>Good luck! Hope it works.</p>";
                            textoalerta += "<p>" + x + " --> " + alertasprocesso.Rows[x].ItemArray[0].ToString() + "</p>";
                        }

                        alertasprocesso.Dispose();
                    }


                   
              

               // Enviaremail(textoalerta);                                           
              
            }
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0) // Checking to see if any cell is selected
            {
                int mSelectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                //dataGridView1.Columns["nrhouse"].DisplayIndex = 0;
                DataGridViewRow mSelectedRow = dataGridView1.Rows[mSelectedRowIndex];
                string cliente = Convert.ToString(mSelectedRow.Cells[2].Value);
                string exportador = Convert.ToString(mSelectedRow.Cells[3].Value);
                string icoterm = Convert.ToString(mSelectedRow.Cells[4].Value);
                string origemhouse = Convert.ToString(mSelectedRow.Cells[5].Value);
                string destinohouse = Convert.ToString(mSelectedRow.Cells[6].Value);
                string agentehouse = Convert.ToString(mSelectedRow.Cells[7].Value);
                string transportadorhouse = Convert.ToString(mSelectedRow.Cells[8].Value);
                string moedafretehouse = Convert.ToString(mSelectedRow.Cells[9].Value);
                string tipofretehouse = Convert.ToString(mSelectedRow.Cells[10].Value);
                string nrvoohouse = Convert.ToString(mSelectedRow.Cells[11].Value);
                string emissaoconhecimentohouse = Convert.ToString(mSelectedRow.Cells[12].Value);
                string prevembarquehouse = Convert.ToString(mSelectedRow.Cells[13].Value);
                string embarquehouse = Convert.ToString(mSelectedRow.Cells[14].Value);
                string nrmaster = Convert.ToString(mSelectedRow.Cells[1].Value);
                string pesobrutohouse = Convert.ToString(mSelectedRow.Cells[15].Value);
                string quantidadeprocessohouse = Convert.ToString(mSelectedRow.Cells[16].Value);
                string pesotaxadohouse = Convert.ToString(mSelectedRow.Cells[17].Value);
                string tar_vendamin = Convert.ToString(mSelectedRow.Cells[18].Value);
                //string tar_vendamin2 = Convert.ToString(mSelectedRow.Cells[19].Value);
                string nrhouse = Convert.ToString(mSelectedRow.Cells[0].Value);

                texthouse.Text = nrhouse;
                textvenda.Text = tar_vendamin;
                textcliente.Text = cliente;
                textexportador.Text = exportador;
                textorigemhouse.Text = origemhouse;
                textdestinohouse.Text = destinohouse;
                textgateway.Text = destinohouse;
                textmoedafretehouse.Text =  moedafretehouse ;
                comboBoxincoterm.Text = icoterm;
                textagentehouse.Text = agentehouse;
                comboBoxtipofrete.Text = tipofretehouse;
                checkBoxvenda.Checked = true;          
                
                if (nrhouse == "")
                {

                }

                
               
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



        public void Enviaremail(string alertas)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            var mail = new MailMessage();
            mail.From = new MailAddress("manecasps@hotmail.com");
            mail.To.Add("clovis.eduardosilva23@gmail.com");
            mail.To.Add("tecnologia@giantcargo.com.br");
            mail.Subject = "Email da Aplicação Agenciamento";
            mail.IsBodyHtml = true;
            string htmlBody;
            string texto = "";
           // for (int x = 0; x < alertas.Count; x++)
           // {

                //htmlBody = "<table border=0 cellpadding=0 cellspacing=0><tbody><tr><td><a href=http://htmlemail.io target=_blank>" + _items[x] + "</a> </td></tr></tbody></table><p>This is a really simple email template. Its sole purpose is to get the recipient to click the button with no distractions.</p><p>Good luck! Hope it works.</p>";
                texto += "<p>" + alertas + "</p>";
          //  }



            //mail.Body = texto;
            //SmtpServer.Port = 587;
            //SmtpServer.UseDefaultCredentials = false;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("manecasps@hotmail.com", "eduardo23");
            //SmtpServer.EnableSsl = true;
            //SmtpServer.Send(mail);
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

        private void btnSair_Click(object sender, EventArgs e)
        {

            //aqui você deve colocar os dados do servidor ftp e suas credenciais
            ftpIPServidor = "integracaoxml.giantcargo.com.br"; //intxmlagimp.giantcargo.com.br:2121";
            ftpUsuarioID = "integracaoxml.giantcargo.com.br|integracaoxml";  //"intxmlagimp";
            ftpSenha = "Ft7x4LCC";          

            
            
            DirectoryInfo d = new DirectoryInfo(@"C:\dados");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories);
            //  FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                // str = str + ", " + file.Name;
                //DataSet ds = new DataSet();
                //ds.ReadXml(@"c:\dados\" + file.Name);
                Upload(@"C:\dados\" + file.Name);
                //MoveArquivo(file.Name);
                DAL del = new DAL();
                string user = CurrentUser();

                del.DeletaArquivosMaster(user);
                del.DeletaMaster(file.Name.Substring(0, file.Name.Length - 4), user);
                del.DeletaHouse(file.Name.Substring(0, file.Name.Length - 4), user);

               // File.Delete(@"C:\dados\" + file.Name);  
            }
            MessageBox.Show(" Upload de Arquivos OK"); 
       
            
            
        }



        private void MoveArquivo(string _nomeArquivo)
        {
            //Move Arquivo depois de enviar para bysofot

            DirectoryInfo d = new DirectoryInfo(@"C:\Integracao - Itraffic Import" + _nomeArquivo);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.pdf", SearchOption.AllDirectories);
            //  FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories); //Getting Text files
            string str = "";

            foreach (FileInfo file in Files)
            {
                string fileName = _nomeArquivo;
                string sourcePath = fileName;
                string targetPath = @"C:\Processados\";

                // Use Path class to manipulate file and directory paths.
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName);

                // To copy a folder's contents to a new location:
                // Create a new target folder, if necessary.
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

              
                System.IO.File.Copy(sourceFile, destFile, true);
                File.Delete(sourcePath + fileName);

               
                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(s, destFile, true);


                    }
                }
                else
                {
                    MessageBox.Show("Diretorio não encontrado : " + sourcePath);
                }



            }

            

        }


        private void Upload(string _nomeArquivo)
        {
            FileInfo _arquivoInfo = new FileInfo(_nomeArquivo);
            string uri = "ftp://" + ftpIPServidor + "/" + _arquivoInfo.Name;
            FtpWebRequest requisicaoFTP;


            // Cria um objeto FtpWebRequest a partir da Uri fornecida
            requisicaoFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpIPServidor + "/" + _arquivoInfo.Name));

            // Fornece as credenciais de WebPermission
            requisicaoFTP.Credentials = new NetworkCredential(ftpUsuarioID, ftpSenha);
            requisicaoFTP.UsePassive = false;
            // Por padrão KeepAlive é true, 
            requisicaoFTP.KeepAlive = false;

            // Especifica o comando a ser executado
            requisicaoFTP.Method = WebRequestMethods.Ftp.UploadFile;

            // Especifica o tipo de dados a ser transferido
            requisicaoFTP.UseBinary = true;

            // Notifica o servidor seobre o tamanho do arquivo a enviar
            requisicaoFTP.ContentLength = _arquivoInfo.Length;

            // Define o tamanho do buffer para 2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int _tamanhoConteudo;

            // Abre um stream (System.IO.FileStream) para o arquivo a ser enviado
            FileStream fs = _arquivoInfo.OpenRead();

            try
            {
                // Stream  para o qual o arquivo a ser enviado será escrito
                Stream strm = requisicaoFTP.GetRequestStream();

                // Lê a partir do arquivo stream, 2k por vez
                _tamanhoConteudo = fs.Read(buff, 0, buffLength);

                // ate o conteudo do stream terminar
                while (_tamanhoConteudo != 0)
                {
                    // Escreve o conteudo a partir do arquivo para o stream FTP 
                    strm.Write(buff, 0, _tamanhoConteudo);
                    _tamanhoConteudo = fs.Read(buff, 0, buffLength);
                }

                // Fecha o stream a requisição
                strm.Close();
                fs.Close();

             
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro de Upload");          
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string user;
            user = CurrentUser();

            label22.Text = user;

            GeraXML();

            DAL Alertas = new DAL();
            DataTable alertasprocesso;

            alertasprocesso = Alertas.GetRegistroAlertastodos();

            if (alertasprocesso.Rows.Count > 0)
            {

                List<string> list = alertasprocesso.AsEnumerable()
                         .Select(r => r.Field<string>("descricao"))
                         .ToList();

                listBox1.DrawMode = DrawMode.OwnerDrawFixed;
                listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
                listBox1.DataSource = list;

                string textoalerta = "";
                for (int x = 0; x < alertasprocesso.Rows.Count; x++)
                {

                    //htmlBody = "<table border=0 cellpadding=0 cellspacing=0><tbody><tr><td><a href=http://htmlemail.io target=_blank>" + _items[x] + "</a> </td></tr></tbody></table><p>This is a really simple email template. Its sole purpose is to get the recipient to click the button with no distractions.</p><p>Good luck! Hope it works.</p>";
                    textoalerta += "<p>" + x + " --> " + alertasprocesso.Rows[x].ItemArray[0].ToString() + "</p>";
                }

                alertasprocesso.Dispose();


            }
            else
            {


                listBox1.DrawMode = DrawMode.OwnerDrawFixed;
                listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItemverde);
                listBox1.Refresh();
                // listBox1.Items.Add("Processos estão ok , para ser enviados para Bysoft ");

                string textoalerta = "";
                for (int x = 0; x < alertasprocesso.Rows.Count; x++)
                {

                    //htmlBody = "<table border=0 cellpadding=0 cellspacing=0><tbody><tr><td><a href=http://htmlemail.io target=_blank>" + _items[x] + "</a> </td></tr></tbody></table><p>This is a really simple email template. Its sole purpose is to get the recipient to click the button with no distractions.</p><p>Good luck! Hope it works.</p>";
                    textoalerta += "<p>" + x + " --> " + alertasprocesso.Rows[x].ItemArray[0].ToString() + "</p>";
                }

                alertasprocesso.Dispose();
            }


            DataSet ds = new DataSet();
            //IEnumerable<string> arquivos = GetAllFilesRecursively(@"C:\dados");
            DirectoryInfo d = new DirectoryInfo(@"C:\dados");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories);
            //  FileInfo[] Files = d.GetFiles("*.xml", SearchOption.AllDirectories); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                // str = str + ", " + file.Name;
                //DataSet ds = new DataSet();
                ds.ReadXml(@"c:\dados\" + file.Name);

            }

           

            dgvXML.DataSource = ds.Tables[1].DefaultView;
            dgvXML.Columns["numeromaster"].DisplayIndex = 0;
            dgvXML.Columns["agente"].DisplayIndex = 1;


            //void GetFiles()
            //    {
            //        DirectoryInfo d= new DirectoryInfo(@"c:\dados\");
            //       //file extension for pdf
            //        var files = d.GetFiles("*.xml*");
            //        FileInfo[] subfileInfo = files.ToArray<FileInfo>();

            //        if (subfileInfo.Length > 0)
            //        {
            //            for (int j = 0; j < subfileInfo.Length; j++)
            //            {
            //                bool isHidden = ((File.GetAttributes(subfileInfo[j].FullName) & FileAttributes.Hidden) == FileAttributes.Hidden);
            //                if (!isHidden)
            //                {
            //                    string strExtention = th.GetExtension(subfileInfo[j].FullName);
            //                    if (strExtention.Contains("pdf"))
            //                    {                            
            //                        string path = subfileInfo[j].FullName;
            //                        string name = bfileInfo[j].Name;                           
            //                    }
            //                }
            //            }
            //        }ol, vcxsacv b;ço87ewq    


        }

            
        
    }
}
