using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CadastroCliente
{
    public partial class MainWindow : Window
    {
        // ADO.NET
        private SqlConnection connection; // Conexão com o SQL Server
        private SqlCommand command;       // Execução de comandos do SQL Server
        private SqlDataReader dataReader; // Lê os resultados de um comando SQL executado no SQL Server

        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=cliente;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private string query = string.Empty;  // Usada para criar os comandos SQL

        private List<Cliente> listaCliente;
        private Cliente cliente;

        public MainWindow()
        {
            InitializeComponent();

            ReadFromDatase();
        }

        private void ReadFromDatase()
        {
            try
            {
                listaCliente = new List<Cliente>();

                using (connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;

                    query = "SELECT * FROM cliente;";

                    using (command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                cliente = new Cliente();

                                // incluir todos os campos do banco

                                cliente.Id = dataReader.GetInt32(0); // 0 -> Coluna Id na tabela Cliente no SQL Server
                                cliente.Nome = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                                cliente.Cpf = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                                cliente.Cnpj = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                                cliente.Cep = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4);
                                cliente.Ie = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                                cliente.RazaoSocial = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetString(6);
                                cliente.Telefone = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetString(7);
                                cliente.Estado = dataReader.IsDBNull(8) ? string.Empty : dataReader.GetString(8);
                                cliente.Cidade = dataReader.IsDBNull(9) ? string.Empty : dataReader.GetString(9);
                                cliente.Bairro = dataReader.IsDBNull(10) ? string.Empty : dataReader.GetString(10);
                                cliente.Rua = dataReader.IsDBNull(11) ? string.Empty : dataReader.GetString(11);
                                cliente.NumeroCasa = !dataReader.IsDBNull(12) ? dataReader.GetInt32(12) : Convert.ToInt32(string.Empty);

                                listaCliente.Add(cliente);
                            }
                        }
                    }
                }

                DataGridCliente.ItemsSource = listaCliente;
            }
            catch (SqlException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        private void ButtonSair_Click(object sender, RoutedEventArgs e)
        {
            Close();      
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente encerrar a Aplicação?", "Encerrar!",
              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) e.Cancel = true;
        }

        private void ButtonNovo_Click(object sender, RoutedEventArgs e)
        {
            LimpaTela();
        }

        public void LimpaTela()
        {
            txtNome.Clear();
            txtCPF.Clear();
            txtCNPJ.Clear();
            txtIE.Clear();
            txtCEP.Clear();
            txtRsocial.Clear();
            txtTelefone.Clear();
            txtEstado.Clear();
            txtCidade.Clear();
            txtBairro.Clear();
            txtRua.Clear();
            txtNcasa.Clear();

            rbFisica.IsChecked = true;
        }

        private void rbFisica_Click(object sender, RoutedEventArgs e)
        {
            ExibirCamposPessoaFisica();
        }

        private void ExibirCamposPessoaFisica()
        {
            if (rbFisica.IsChecked == true)
            {
                lbRsocial.Visibility = Visibility.Hidden;
                txtRsocial.Visibility = Visibility.Hidden;

                lbIE.Visibility = Visibility.Hidden;
                txtIE.Visibility = Visibility.Hidden;

                lbCPFCNPJ.Content = "CPF:*";
                txtCPF.Visibility = Visibility.Visible;
                txtCNPJ.Visibility = Visibility.Hidden;

            }
        }

        private void rbJuridica_Click(object sender, RoutedEventArgs e)
        {
            ExibirCamposPessoaJuridica();
        }

        private void ExibirCamposPessoaJuridica()
        {
            if (rbJuridica.IsChecked == true)
            {
                lbRsocial.Visibility = Visibility.Visible;
                txtRsocial.Visibility = Visibility.Visible;

                lbIE.Visibility = Visibility.Visible;
                txtIE.Visibility = Visibility.Visible;

                lbCPFCNPJ.Content = "CNPJ:*";
                txtCPF.Visibility = Visibility.Hidden;
                txtCNPJ.Visibility = Visibility.Visible;
            }
        }

        private void txtCEP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BuscaEndereco.verificaCEP(txtCEP.Text) == true)
            {
                txtBairro.Text = BuscaEndereco.bairro;
                txtEstado.Text = BuscaEndereco.estado;
                txtCidade.Text = BuscaEndereco.cidade;
                txtRua.Text = BuscaEndereco.rua;
            }
        }

        private void ButtonIncluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Faz a validação se os campos estão preenchidos
                CamposObrigatorios();

                using (connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;

                    query = @"insert into cliente (nome, cpf, cnpj, cep, ie, razao_social, telefone, estado, cidade, bairro, rua, numero_casa) 
                    Values (@Nome, @Cpf, @Cnpj, @Cep, @Ie, @RazaoSocial, @Telefone, @Estado, @Cidade, @Bairro, @Rua, @NumeroCasa)";


                    using (command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@Nome", txtNome.Text);
                        command.Parameters.AddWithValue("@Cpf", txtCPF.Text);
                        command.Parameters.AddWithValue("@Cnpj", txtCNPJ.Text);
                        command.Parameters.AddWithValue("@Cep", txtCEP.Text);
                        command.Parameters.AddWithValue("@Ie", txtIE.Text);
                        command.Parameters.AddWithValue("@RazaoSocial", txtRsocial.Text);
                        command.Parameters.AddWithValue("@Telefone", txtTelefone.Text);
                        command.Parameters.AddWithValue("@Estado", txtEstado.Text);
                        command.Parameters.AddWithValue("@Cidade", txtCidade.Text);
                        command.Parameters.AddWithValue("@Bairro", txtBairro.Text);
                        command.Parameters.AddWithValue("@Rua", txtRua.Text);
                        command.Parameters.AddWithValue("@NumeroCasa", txtNcasa.Text);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cliente cadastrado com Sucesso!");

                ReadFromDatase();
                LimpaTela();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ButtonAtualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Faz a validação se os campos estão preenchidos
                CamposObrigatorios();

                using (connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;

                    query = @"update cliente 
                    set nome = @Nome, cpf = @Cpf, cnpj = @Cnpj, cep = @Cep, ie = @Ie, razao_social = @RazaoSocial, telefone = @Telefone, estado = @Estado, cidade = @Cidade, bairro = @Bairro, rua = @Rua, numero_casa = @NumeroCasa
                    where id = @Id";

                    using (command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@Nome", txtNome.Text);
                        command.Parameters.AddWithValue("@Cpf", txtCPF.Text);
                        command.Parameters.AddWithValue("@Cnpj", txtCNPJ.Text);
                        command.Parameters.AddWithValue("@Cep", txtCEP.Text);
                        command.Parameters.AddWithValue("@Ie", txtIE.Text);
                        command.Parameters.AddWithValue("@RazaoSocial", txtRsocial.Text);
                        command.Parameters.AddWithValue("@Telefone", txtTelefone.Text);
                        command.Parameters.AddWithValue("@Estado", txtEstado.Text);
                        command.Parameters.AddWithValue("@Cidade", txtCidade.Text);
                        command.Parameters.AddWithValue("@Bairro", txtBairro.Text);
                        command.Parameters.AddWithValue("@Rua", txtRua.Text);
                        command.Parameters.AddWithValue("@NumeroCasa", txtNcasa.Text);
                        command.Parameters.AddWithValue("@Id", cliente.Id);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cliente editado com Sucesso!");

                ReadFromDatase();
                LimpaTela();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;

                    query = @"delete from cliente where id = @Id";

                    using (command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@Id", cliente.Id);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cliente excluído com Sucesso!");

                ReadFromDatase();
                LimpaTela();
            }
            catch
            {
                MessageBox.Show("Erro ao tentar excluir o cliente!");
            }
        }

        private void DataGridCliente_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var clienteSelecionado = (Cliente)DataGridCliente.SelectedItem;

            if (clienteSelecionado != null)
            {
                txtNome.Text = clienteSelecionado.Nome;
                txtRsocial.Text = clienteSelecionado.RazaoSocial;

                if (!string.IsNullOrWhiteSpace(clienteSelecionado.Cpf))
                {
                    rbFisica.IsChecked = true;
                    ExibirCamposPessoaFisica();
                    txtCPF.Text = clienteSelecionado.Cpf;
                }
                else
                {
                    rbJuridica.IsChecked = true;
                    ExibirCamposPessoaJuridica();
                    txtCNPJ.Text = clienteSelecionado.Cnpj;
                }

                txtCEP.Text = clienteSelecionado.Cep;
                txtIE.Text = clienteSelecionado.Ie;
                txtTelefone.Text = clienteSelecionado.Telefone;
                txtEstado.Text = clienteSelecionado.Estado;
                txtCidade.Text = clienteSelecionado.Cidade;
                txtBairro.Text = clienteSelecionado.Bairro;
                txtRua.Text = clienteSelecionado.Rua;
                txtNcasa.Text = Convert.ToString(clienteSelecionado.NumeroCasa);
            }
        }

        private void CamposObrigatorios()
        {
            if (txtNome.Text.Trim().Length == 0)
            {
                throw new Exception("O nome é obrigatório");
            }

            if (rbJuridica.IsChecked == true)
            {
                if (txtCNPJ.Text.Trim().Length == 0)
                {
                    throw new Exception("O CNPJ é obrigatório");
                }

                if (Validacao.IsCnpj(txtCNPJ.Text) == false)
                {
                    throw new Exception("O CNPJ é Inválido");
                }

                if (txtRsocial.Text.Trim().Length == 0)
                {
                    throw new Exception("A Razão Social é obrigatório");
                }
            }

            if (rbFisica.IsChecked == true)
            {
                if (txtCPF.Text.Trim().Length == 0)
                {
                    throw new Exception("O CPF é obrigatório");
                }

                if (Validacao.IsCpf(txtCPF.Text) == false)
                {
                    throw new Exception("O CPF é Inválido");
                }
            }

            if (txtCEP.Text.Trim().Length == 0)
            {
                throw new Exception("O CEP é obrigatório");
            }

            if (txtCidade.Text.Trim().Length == 0)
            {
                throw new Exception("A cidade é obrigatória");
            }

            if (txtEstado.Text.Trim().Length == 0)
            {
                throw new Exception("O estado é obrigatório");
            }

            if (txtBairro.Text.Trim().Length == 0)
            {
                throw new Exception("O bairro é obrigatório");
            }

            if (txtRua.Text.Trim().Length == 0)
            {
                throw new Exception("A rua é obrigatória");
            }


        }
    }
}
