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
    class DALBysoft
    {

       

        /// <summary>
        /// conexão bysoft
        /// </summary>

        static string serverNamebysoft = "189.91.32.81";  //localhost
        static string portbysoft = "5432";             //porta default
        static string userNamebysoft = "postgres";     //nome do administrador
        static string passwordbysoft = "masterkey";     //senha do administrador
        static string databaseNamebysoft = "iglobal"; //nome do banco de dados

       

        NpgsqlConnection pgsqlConnection = null;
        string connStringBysoft = null;

        public DALBysoft()
        {
            connStringBysoft = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                          serverNamebysoft, portbysoft, userNamebysoft, passwordbysoft, databaseNamebysoft);
        }


        public DataTable GetRegistromasterBysoft()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select cdpessoa As Codigo_Agente,appessoa As Nome_Agente from pessoa where indagenteinternacional = 'S' AND appessoa IS NOT NULL  ORDER by appessoa ";

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



        public DataTable GetTransportadora()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select cdpessoa As Codigo_Agente,appessoa As Nome_Agente from pessoa where indcompanhiaaerea = 'S' AND appessoa IS NOT NULL  ORDER by appessoa ";

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


        public DataTable GetExportadorBysoft()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select cdpessoa As Codigo_Agente,appessoa As Nome_Agente  from pessoa where indexportador = 'S' AND appessoa IS NOT NULL ORDER by appessoa";

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

        public DataTable GetClienteBysoft()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select cdpessoa As Codigo_Agente,appessoa As Nome_Agente from pessoa where indcliente = 'S'  AND appessoa IS NOT NULL ORDER by appessoa";

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

        public DataTable GetDestino()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select cdterminalcarga As Codigo_destino ,nmterminalcarga as Destino from terminalcarga where  nmterminalcarga IS NOT NULL ORDER by nmterminalcarga";

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
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
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


        public DataTable Getcounthouses(string master)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
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

        public DataTable GetMaster_house()
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "SELECT h.nrmaster, h.nrhouse FROM  tb_housepdfs  h GROUP BY h.nrmaster, h.nrhouse ORDER  BY h.nrhouse DESC";
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


        public DataTable GetRegistrohouse(string nrmaster)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select * From tb_housepdfs where nrmaster = '"+nrmaster+"'";
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
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
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
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
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
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
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

        public DataTable GetRegistroHouses(string master)
        {

            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "select Distinct nrmaster,cliente,exportador,icoterm,origem,destino,agente,transportador,moedafrete,tipofrete,nrvoo,emissaoconhecimento,prevembarque,embarque,quantidade,pesobruto,pesotaxado, * from tb_housepdfs where nrmaster = '" + master + "'";
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


        public void AtualizarHouse(string nrmaster, string cliente, string exportador, string icoterm, string origem, string destino, string agente, string transportador, string moedafrete, string tipofrete, string nrvoo, string emissaoconhecimento, string prevembarque, string embarque, string quantidade, string pesobruto, string pesotaxado, string nrhouse)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connStringBysoft))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdAtualiza = String.Format("Update tb_housepdfs Set cliente = '" + cliente + "' , exportador = " + exportador + "' , icoterm = " + icoterm + "' , origem = " + origem + "' , destino = " + destino + "' , agente = " + agente + "' , transportador = " + transportador + "' , moedafrete = " + moedafrete + "' , tipofrete = " + tipofrete + "' , nrvoo = " + nrvoo + "' , emissaoconhecimento = " + emissaoconhecimento + "' , prevembarque = " + prevembarque + "' , embarque = " + embarque + "' , pesobruto = " + pesobruto + "' , pesotaxado = " + pesotaxado + " Where nrhouse = " + nrhouse);

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
            finally
            {
                pgsqlConnection.Close();
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
