using CRUD_Mongo_Biblioteca.Conexao;
using CRUD_Mongo_Biblioteca.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Mongo_Biblioteca.Controller
{
    public class LeitorController
    {
        private Leitor leitor = new Leitor();
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();

        public void CadastrarLeitor()
        {
            AplicaNulos();

            var codigo = GeraCodigoAsync();

            Console.WriteLine("Bem vindo ao cadastro de leitores, a seguir informe os dados conforme solicitado!");

            leitor.CodigoLeitor = codigo.Result;

            Console.Write("Nome do Leitor: ");
            leitor.Nome = Console.ReadLine();

            Console.Write("CPF: ");
            leitor.Cpf = Console.ReadLine();

            conexao.Leitor.InsertOneAsync(leitor);

            Console.WriteLine("Documento incluído com sucesso!");
            Console.Write("Pressione qualquer tecla para continuar: ");
            Console.ReadKey();
        }

        public async void RelatorioLeitores()
        {
            Console.WriteLine("Listando Documentos");

            var listaLeitores = await conexao.Leitor.Find(new BsonDocument())
                                                           .ToListAsync();
            Console.WriteLine("{0, -5} {1, -32} {2, 12} \n", "Codigo", "Nome", "CPF");
            foreach (var doc in listaLeitores)
            {
                Console.WriteLine("{0, -5} {1, -32} {2, 20}", doc.CodigoLeitor, doc.Nome, doc.Cpf);
            }

            Console.WriteLine("Fim da lista...");
        }

        public void RemoverLeitores()
        {
            AplicaNulos();
            int aluguel = 0;
            int verificador = 0;
            int opcao = 0;
            int codigo = 0;
            Console.WriteLine("Verifique o código do Leitor que deseja remover na lista abaixo: ");
            RelatorioLeitores();
            Thread.Sleep(2000);
            Console.WriteLine();
            Console.Write("Informe o código do leitor que deseja remover: ");
            codigo = int.Parse(Console.ReadLine());

            Console.WriteLine("Tem certeza que deseja excluir esse registro? 1 - Sim, 0 - Não");
            opcao = int.Parse(Console.ReadLine());
            verificador = VerificaRegistro(codigo);
            Thread.Sleep(2000);
            if (opcao == 1)
            {
                if (verificador == 1)
                {
                    Console.WriteLine("O registro que você está tentando excluir está vinculado a um aluguel.");
                    Console.WriteLine("Para excluir o registro, será necessário excluir também do aluguel que o mesmo está vinculado.");
                    Console.WriteLine("Deseja excluir mesmo assim? 1 - Sim, 0 - Não");
                    opcao = int.Parse(Console.ReadLine());
                    if (opcao == 1)
                    {
                        VerificaRegistroAluguel(codigo);
                        Thread.Sleep(2000);
                        ExcluiLeitor(codigo);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("Retornando ao menu...");
                        Console.WriteLine("Pressione qualquer tecla para continuar");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Excluindo Registro...");
                    ExcluiLeitor(codigo);
                    Thread.Sleep(2000);
                    Console.WriteLine("Registro excluido com sucesso! \nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
            Console.WriteLine("Retornando ao menu de opções...");
            Console.WriteLine("Pressione qualquer tecla para continuar... ");
            Console.ReadKey();
        }

        public void ExcluiLeitor(int codigo)
        {
            var construtor = Builders<Leitor>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLeitor, codigo);

            Console.WriteLine("Excluindo Leitor...");
            conexao.Leitor.DeleteOneAsync(condicao);
            Console.WriteLine("Leitor excluido com sucesso! \nPressione qualquer tecla para continuar... ");
            Console.ReadKey();
        }

        //Verifica se o leitor que o usuário está tentando excluir existe em algum aluguel de livros
        public int VerificaRegistro(int codigo)
        {
            int existe = 0;
            var construtor = Builders<Aluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLeitor, codigo);

            var listaLeitor = conexao.Aluguel.Find(new BsonDocument()).ToListAsync();

            if (listaLeitor.Result.Any())
            {
                existe = 1;
            }

            return existe;
        }

        //Verifica o aluguel que o Leitor está vinculado e exclui os livros do aluguel e também o aluguel
        public void VerificaRegistroAluguel(int codigo)
        {
            int existe = 0;
            int codigoAluguel = 0;
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLeitor, codigo);

            var listaLivros = conexao.LivroAluguel.Find(condicao).ToListAsync();

            Console.WriteLine("Excluindo Livros de Aluguel");
            Console.WriteLine();
            conexao.LivroAluguel.DeleteManyAsync(condicao);
            Console.WriteLine("Livros de Aluguel vinculados ao leitor excluidos com sucesso!");
            Console.WriteLine("Pressione qualquer tecla para continuar... ");
            Console.ReadKey();

            var construtor1 = Builders<Aluguel>.Filter;
            var condicao1 = construtor1.Eq(x => x.CodigoLeitor, codigo);

            Console.WriteLine("Excluindo Aluguel");
            conexao.Aluguel.DeleteManyAsync(condicao1);

            Console.WriteLine("Aluguel Excluido com sucesso!");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();

        }

        public void AlteraLeitor()
        {

        }

        public async Task<int> GeraCodigoAsync()
        {
            int codigo = 1;
            var listaLeitor = await conexao.Leitor.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaLeitor)
            {
                if (doc.CodigoLeitor.HasValue)
                {
                    codigo = doc.CodigoLeitor.Value + 1;
                }
            }
            return codigo;
        }

        public int ContaEntidadeLeitor()
        {
            int quantidadeLeitor = 0;
            var leitores = conexao.Leitor.CountDocuments(new BsonDocument());
            quantidadeLeitor = (int)leitores;
            return quantidadeLeitor;
        }

        private void AplicaNulos()
        {
            leitor.Id = null;
            leitor.CodigoLeitor = null;
            leitor.Nome = null;
            leitor.Cpf = null;
        }
    }
}
