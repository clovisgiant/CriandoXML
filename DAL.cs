using System;
using System.Data;
using Npgsql;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;

namespace CriandoXML
{
    class DAL
    {

        static string serverName = "192.168.66.44";  //localhost
        static string port = "5432";             //porta default
        static string userName = "postgres";     //nome do administrador
        static string password = "d19m11";     //senha do administrador
        static string databaseName = "agenciamento"; //nome do banco de dados    

       

        NpgsqlConnection pgsqlConnection = null;
        NpgsqlConnection pgsqlConnectionhouse = null;
        string connString = null;

        public DAL()
        {
            connString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                          serverName, port, userName, password, databaseName);
        }   
        
        
        public DataTable GetRegistromaster(string master)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select Distinct * from tb_pdfmaster where nrmaster = '"+master+"' order by nrmaster";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return dt;
        }

        public DataTable totalmaster(string master)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select SUM(CAST((COALESCE(quantidade,'0')) AS INTEGER)) As quantidade, SUM(COALESCE(cast(pesobruto as float),0))  As pesobruto, SUM(COALESCE(cast(pesotaxado as float),0))  As pesotaxado FROM tb_housepdfs where nrmaster ='" + master + "'" ;

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable GetRegistroDEPARA(string descricao)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select  ds_codigo,  ds_descricao from tb_depara where ds_descricao Like '%" + descricao + "%'";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return dt;
        }
        public DataTable Getcounthouses(string master)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select Distinct * from tb_pdfmaster where nrmaster = '" + master + "' order by nrmaster";            

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable GetMaster_house(string user)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "SELECT h.nrmaster, h.nrhouse FROM  tb_housepdfs  h where nomeuser = '" + user + "'  GROUP BY h.nrmaster, h.nrhouse ORDER  BY h.nrhouse DESC";
                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }


        public DataTable GetRegistrohouse(string nrmaster, string user)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select Distinct nrhouse, nrmaster,cliente,exportador,icoterm,origem,destino,agente,transportador,moedafrete,tipofrete,nrvoo,emissaoconhecimento,prevembarque,embarque,quantidade,pesobruto,pesotaxado,tar_vendamin From tb_housepdfs where nrmaster = '" + nrmaster + "' AND  nomeuser = '" + user + "' ORDER BY nrhouse";
                    //string cmdSeleciona = "SELECT DISTINCT ON (nrhouse) * FROM tb_housepdfs where nrmaster = '" + nrmaster + "' ORDER BY nrhouse DESC";
                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

      

        public DataTable GetRegistroAgentes()
        {
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    //string cmdSeleciona = "select * From tb_housepdfs where nrmaster = '" + agente + "'";
                    string cmdSeleciona = "select cd_agente,ds_razao_social, ds_nome_fantasia from tb_agente order by ds_nome_fantasia";
                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }


        public DataTable GetRegistroArmazem()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    //string cmdSeleciona = "select * From tb_housepdfs where nrmaster = '" + agente + "'";
                    string cmdSeleciona = "select cod_armazem, ds_armazem from tb_armazem order by ds_armazem";
                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable GetRegistroExportador()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    //string cmdSeleciona = "select * From tb_housepdfs where nrmaster = '" + agente + "'";
                    string cmdSeleciona = "select cd_exp_importador, ds_nome from tb_exp_importador order by ds_nome";
                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable GetRegistroHouses(string master, string user)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select Distinct nrhouse, nrmaster,cliente,exportador,icoterm,origem,destino,agente,transportador,moedafrete,tipofrete,nrvoo,emissaoconhecimento,prevembarque,embarque,quantidade,pesobruto,pesotaxado,tar_vendamin from tb_housepdfs where   nomeuser = '" + user + "' AND nrmaster = '" + master + "'";
                    //string cmdSeleciona = "select Distinct * from tb_housepdfs where nrmaster = '" + master + "' order by nrhouse";

                   // string cmdSeleciona = "SELECT DISTINCT ON (nrhouse) * FROM tb_housepdfs where nrmaster = '" + master + "' ORDER BY nrhouse DESC";
                  
                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable GetRegistroAlertas(string master)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select Distinct descricao from tb_alertasmaster where corrigiu <> 1 AND nrmaster = '" + master + "'";
                    // string cmdSeleciona = "select cd_agente,ds_razao_social, ds_nome_fantasia from tb_agente order by ds_nome_fantasia";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
                return dt;
          
        }

        public DataTable GetRegistroAlertastodos()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select Distinct descricao from tb_alertasmaster where corrigiu <> 1";
                    // string cmdSeleciona = "select cd_agente,ds_razao_social, ds_nome_fantasia from tb_agente order by ds_nome_fantasia";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return dt;

        }

        public void AtualizarMaster(string nrmaster, string textqtdprocesso, string txtorigem, string txtdestino,
            string txtagente, string texttransportador, string txtmoedafrete, string comboBoxtipo,
            string textvoo, string txtemissao, string maskedembarqueprev, string maskedembarque, string textquantidade, 
            string textpesobruto, string textpesotaxado)
        {                                                                                                                                                                                                                           
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdAtualiza = String.Format("Update tb_pdfmaster Set  origem = '" + txtorigem + "' , destino = '" + txtdestino + "' , agente = '" + txtagente + "' , transportador = '" + texttransportador + "' , moedafrete = '" + txtmoedafrete + "' , tipofrete = '" + comboBoxtipo + "' , nrvoo = '" + textvoo +  "' , prevembarque ='" + maskedembarqueprev + "' , embarque = '" + maskedembarque + "' Where nrmaster =  '" + nrmaster + "'");

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdAtualiza, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    pgsqlConnection.Close();
            //}
        }

        

        public void AtualizarAlertaMaster(string nrmaster, string origem, string destino, string agente,  string moedafrete, string tipofrete)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnectionhouse= new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnectionhouse.Open();

                    string cmdAtualiza = String.Format("Update tb_alertasmaster Set corrigiu = " + 1 + " , descricao ='Processo  ok  para ser enviados para Bysoft'  , origem = '" + origem + "' , destino = '" + destino + "' , agente = '" + agente + "' , moedafrete = '" + moedafrete + "' , tipofrete = '" + tipofrete + "' Where nrmaster = '" + nrmaster + "'");
                    
                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdAtualiza, pgsqlConnectionhouse))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void AtualizarHouse(string nrhouse, string cliente, string exportador, string icoterm, string origem, string destino, string agente, string moedafrete, string tipofrete)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnectionhouse = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnectionhouse.Open();

                    string cmdAtualiza = String.Format("Update tb_housepdfs Set cliente = '" + cliente + "' , exportador = '" + exportador + "' , icoterm = '" + icoterm + "' , origem = '" + origem + "' , destino = '" + destino + "' , agente = '" + agente + "' , moedafrete = '" + moedafrete + "' , tipofrete = '" + tipofrete + "' Where nrhouse = '" + nrhouse + "'");

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdAtualiza, pgsqlConnectionhouse))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Inserir registros
        public void InserirAlertaMaster(string descricao, string nrmaster)
        {


            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
            {
                //Abra a conexão com o PgSQL                  
                pgsqlConnection.Open();

                string cmdInserir = String.Format("Insert Into tb_alertasmaster(descricao,nrmaster) values('{0}','{1}')", descricao, nrmaster);

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }
            }

        }



        public DataTable GetDestinomaster(string nrmaster, string user)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select Distinct origem from  tb_housepdfs where nomeuser = '" + user + "' AND nrmaster =  '" + nrmaster + "'";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        //Deleta registros
        public void DeletaHouse(string nrmaster, string user)
        {


            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
            {
                //Abra a conexão com o PgSQL                  
                pgsqlConnection.Open();

                string cmdInserir = String.Format("Delete  From tb_housepdfs where nomeuser = '" + user + "' AND  nrmaster =  '" + nrmaster + "'");

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }
            }

        }

        //Deleta registros
        public void DeletaMaster(string nrmaster, string user)
        {


            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
            {
                //Abra a conexão com o PgSQL                  
                pgsqlConnection.Open();

                string cmdInserir = String.Format("Delete  From tb_pdfmaster where nomeuser = '" + user + "' AND  nrmaster =  '" + nrmaster + "'");

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }
            }

        }

        //Deleta registros
        public void DeletaArquivosMaster(string user)
        {


            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
            {
                //Abra a conexão com o PgSQL                  
                pgsqlConnection.Open();

                string cmdInserir = String.Format("Delete  From tb_arquivospdfs WHERE nomeuser = '" + user + "'");

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }
            }

        }


        public class Master
        {
            public string nrmaster;
            public string nrhouse;
            public string origem;
            public string Destino;
            public string agente;
            public string transportador;
            public string moedafrete;
            public string tipofrete;
            public string nrvoo;
            public string emissaoconhecimento;
            public string prevembarque;
            public string embarque;
            public string quantidade;
            public string pesobruto;
            public string pesotaxado;
            public string quantidadeprocesso;
        }

        public class House
        {
            public string nrhouse;
            public string nrmaster;
            public string cliente;
            public string Exportador;
            public string icoterm;
            public string tar_vendaMin;
            public string origem;
            public string Destino;
            public string agente;
            public string transportador;
            public string moedafrete;
            public string tipofrete;
            public string nrvoo;
            public string emissaoconhecimento;
            public string prevembarque;
            public string embarque;
            public string quantidade;
            public string pesobruto;
            public string pesotaxado;
            public string quantidadeprocesso;
           
           
        }


    }
}
