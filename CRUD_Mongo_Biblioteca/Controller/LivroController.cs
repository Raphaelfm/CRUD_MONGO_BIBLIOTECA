using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Mongo_Biblioteca.Model;
using CRUD_Mongo_Biblioteca.Conexao;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CRUD_Mongo_Biblioteca.Controller
{
    public class LivroController
    {
        private Livro livro = new Livro();        
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();

        public void CadastrarLivro()
        {
            AplicaNulos();

            var codigo = GeraCodigoAsync();

            Console.WriteLine("Bem vindo ao cadastro de livros, a seguir informe os dados conforme solicitado!");

            livro.CodigoLivro = codigo.Result;

            Console.Write("Titulo do Livro: ");
            livro.Titulo = Console.ReadLine();

            Console.Write("Autor: ");
            livro.Autor = Console.ReadLine();

            Console.Write("Ano do Livro: ");
            livro.Ano = int.Parse(Console.ReadLine());

            Console.Write("Quantidade de páginas: ");
            livro.Paginas = int.Parse(Console.ReadLine());

            Console.Write("Quantidade Disponível: ");
            livro.QuantidadeDisponivel = int.Parse(Console.ReadLine());

            Console.Write("Valor aluguel: ");
            livro.ValorAluguel = Double.Parse(Console.ReadLine());


            //Cria uma lista para adicionar vários assuntos em uma unica chamada de método
            Console.Write("Assunto: ");
            bool insereMais = true;
            List<string> assuntos = new List<string>();
            while (insereMais)
            {
                int opcao = int.Parse("1");

                Console.Write("Assunto do livro: ");
                string? assunto = Console.ReadLine();
                assuntos.Add(assunto);

                Console.WriteLine("Deseja incluir mais assuntos para este livro? 0 - Sim, 1 - Não");
                opcao = int.Parse(Console.ReadLine());

                if (opcao == 1) { insereMais = false; }
            }
            livro.Assunto = assuntos;

            conexao.Livro.InsertOneAsync(livro);
            Console.WriteLine("Documento incluído com sucesso!");
            Console.Write("Pressione qualquer tecla para continuar: ");
            Console.ReadKey();
        }

        public async void RelatorioLivros()
        {
            Console.WriteLine("Listando Documentos");

            var listaLivros = await conexao.Livro.Find(new BsonDocument())
                                                           .ToListAsync();
            Console.WriteLine("{0, -5} {1, -32} {2, -4} {3, -4} {4, 9}\n", "Codigo", "Titulo", "Quantidade de Páginas", "Quantidade Disponível", "Valor");
            foreach (var doc in listaLivros)
            {
                Console.WriteLine("{0, -5} {1, -32} {2, 20} {3, 20} {4, 9}", doc.CodigoLivro, doc.Titulo, doc.Paginas, doc.QuantidadeDisponivel, doc.ValorAluguel);
            }

            Console.WriteLine("Fim da lista...");
        }

        public void RemoveLivro()
        {
            AplicaNulos();
            int aluguel = 0;
            int verificador = 0;
            int opcao = 0;
            int codigo = 0;
            Console.WriteLine("Verifique o código do livro que deseja remover na lista abaixo: ");
            RelatorioLivros();
            Thread.Sleep(2000);
            Console.WriteLine();
            Console.Write("Informe o código do livro que deseja remover: ");
            codigo = int.Parse(Console.ReadLine());

            Console.WriteLine("Tem certeza que deseja excluir esse registro? 1 - Sim, 0 - Não");
            opcao = int.Parse(Console.ReadLine());
            verificador = VerificaRegistro(codigo);
            Thread.Sleep(2000);
            if (opcao == 1)
            {
                if(verificador == 1)
                {
                    Console.WriteLine("O registro que você está tentando excluir está vinculado a um aluguel.");
                    Console.WriteLine("Para excluir o registro, será necessário excluir o mesmo do aluguel que o mesmo está vinculado.");
                    Console.WriteLine("Deseja excluir mesmo assim? 1 - Sim, 0 - Não");
                    opcao = int.Parse(Console.ReadLine());
                    if (opcao == 1)
                    {
                        VerificaRegistroAluguel(codigo);
                        ExcluiLivroAluguel(codigo);
                        ExcluiLivro(codigo);
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
                    ExcluiLivro(codigo);
                    Thread.Sleep(2000);
                    Console.WriteLine("Registro excluido com sucesso! \nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }                
            }
            else
            {
                Console.WriteLine("Retornando ao menu de opções...");
                Console.WriteLine("Pressione qualquer tecla para continuar... ");
                Console.ReadKey();
            }

        }

        public void ExcluiLivro(int codigo)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            Console.WriteLine("Excluindo livros");
            conexao.Livro.DeleteOneAsync(condicao);
            Console.WriteLine("Registro excluido com sucesso! \nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public void ExcluiLivroAluguel(int codigo)
        {
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            Console.WriteLine("Excluindo livros de Aluguel");
            conexao.LivroAluguel.DeleteOneAsync(condicao);
        }

        //Verifica se o livro que o usuário está tentando excluir existe em algum aluguel de livros
        public int VerificaRegistro(int codigo)
        {
            int existe = 0;
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = conexao.LivroAluguel.Find(new BsonDocument()).ToListAsync();

            if (listaLivros.Result.Any())
            {
                existe = 1;
            }

            return existe;
        }

        //Verifica se no aluguel existe algum livro associado além do livro que está sendo excluído para determinar se o aluguel será excluido
        public async void VerificaRegistroAluguel(int codigo)
        {       
            int existe = 0;
            int codigoAluguel = 0;
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.LivroAluguel.Find(condicao).ToListAsync();

            foreach(var cod in listaLivros)
            {                
                codigoAluguel = cod.CodigoAluguel.Value;
            }

            construtor = Builders<LivroAluguel>.Filter;
            condicao = construtor.Eq(x => x.CodigoAluguel, codigoAluguel);

            listaLivros = await conexao.LivroAluguel.Find(condicao).ToListAsync();

            foreach(var doc in listaLivros)
            {
                if(doc.CodigoLivro != codigo)
                {
                    existe= 1;
                }
            }

            if(existe != 1)
            {
                var construtor1 = Builders<Aluguel>.Filter;
                var condicao1 = construtor1.Eq(x => x.CodigoAluguel, codigoAluguel);

                Console.WriteLine("Excluindo Aluguel");
                await conexao.Aluguel.DeleteOneAsync(condicao1);

                Console.WriteLine("Aluguel Excluido com sucesso!");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        public void AlteraLivro()
        {            
            int opcao = 0;
            int codigo = 0;
            bool running = true;

            while (running)
            {
                AplicaNulos();
                Console.WriteLine("Verifique o código do Livro que deseja alterar na lista abaixo: ");
                Console.WriteLine();
                RelatorioLivros();
                Thread.Sleep(2000);
                Console.WriteLine();
                Console.WriteLine("Informe o campo que deseja alterar: (informe somente o número da opção)");
                Console.WriteLine();
                Console.WriteLine("1 - Alterar Autor");
                Console.WriteLine("2 - Alterar Ano do Livro");
                Console.WriteLine("3 - Alterar Quantidade de Páginas");
                Console.WriteLine("4 - Alterar Quantidade Disponível");
                Console.WriteLine("5 - Alterar Valor do Aluguel");
                Console.WriteLine("6 - Alterar Assunto");
                Console.WriteLine("7 - Voltar ao menu de opções anterior");
                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        Console.WriteLine("Informe o codigo do livro que deseja alterar: ");
                        codigo = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite o Novo Autor: ");
                        string novoAutor = Console.ReadLine();
                        AtualizarAutor(codigo, novoAutor);
                        Console.WriteLine();
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.Write("Pressione qualquer tecla para continuar... ");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.WriteLine("Informe o codigo do livro que deseja alterar: ");
                        codigo = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite o Novo Ano: ");
                        int novoAno = int.Parse(Console.ReadLine());
                        AtualizarAnoLivro(codigo, novoAno);
                        Console.WriteLine();
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.Write("Pressione qualquer tecla para continuar... ");
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.WriteLine("Informe o codigo do livro que deseja alterar: ");
                        codigo = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite a nova quantidade de páginas: ");
                        int novaQuantidade = int.Parse(Console.ReadLine());
                        AtualizarQPaginasLivro(codigo, novaQuantidade);
                        Console.WriteLine();
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.Write("Pressione qualquer tecla para continuar... ");
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.WriteLine("Informe o codigo do livro que deseja alterar: ");
                        codigo = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite a nova quantidade disponível: ");
                        int novaQuantidadeDisp = int.Parse(Console.ReadLine());
                        AtualizarQDisponivelLivro(codigo, novaQuantidadeDisp);
                        Console.WriteLine();
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.Write("Pressione qualquer tecla para continuar... ");
                        Console.ReadKey();
                        break;
                    case 5:
                        Console.WriteLine("Informe o codigo do livro que deseja alterar: ");
                        codigo = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite o novo valor do Aluguel do livro: ");
                        int novoValor = int.Parse(Console.ReadLine());
                        AtualizarValorAluguelLivro(codigo, novoValor);
                        Console.WriteLine();
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.Write("Pressione qualquer tecla para continuar... ");
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.WriteLine("Informe o codigo do livro que deseja alterar: ");
                        codigo = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite os novos assuntos do livro, separados por virgula (exemplo: assunto1, assunto2, etc, ... : ");
                        string novoAssunto = Console.ReadLine();
                        AtualizarAssuntoLivro(codigo, novoAssunto);
                        Console.WriteLine();
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.Write("Pressione qualquer tecla para continuar... ");
                        Console.ReadKey();
                        break;
                    case 7:
                        Console.WriteLine("Voltando para o menu de opções...");
                        Thread.Sleep(2000);
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválidade \nVerifique a opção desejada e tente novamente...");
                        Thread.Sleep(2000);
                        Console.Clear();
                        running = false;
                        break;
                }

                if (opcao == 7)
                {
                    running = false;
                }
                else
                {
                    Console.Write("Deseja atualizar outro campo? 1 - Sim, 0 - Não (Digite apenas o número para Sim ou Não): ");
                    opcao = int.Parse(Console.ReadLine());
                    if (opcao == 0)
                    {
                        Console.WriteLine("Retornando ao menu de opções... ");
                        running = false;
                    }
                }
            }            
        }

        public async Task<int> GeraCodigoAsync()
        {
            int codigo = 1;
            var listaLivro = await conexao.Livro.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaLivro)
            {
                if (doc.CodigoLivro.HasValue)
                {
                    codigo = doc.CodigoLivro.Value + 1;
                }
            }
            return codigo;
        }

        public int ContaEntidadeLivro()
        {
            int quantidadeLivro = 0;
            var livros = conexao.Livro.CountDocuments(new BsonDocument());
            quantidadeLivro = (int)livros;
            return quantidadeLivro;
        }

        public void AplicaNulos()
        {
            livro.Id = null;
            livro.CodigoLivro = null;
            livro.Titulo = null;
            livro.Autor = null;
            livro.Ano = null;
            livro.Paginas = null;
            livro.Assunto = null;
        }

        public async void AtualizarAutor(int codigo, string novoAutor)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.Livro.Find(condicao).ToListAsync();

            foreach (var doc in listaLivros)
            {
                doc.Autor = novoAutor;
                //Método do update no C# com mongo
                //Vou inserir o comando aqui demonstrando o tratamento do mesmo.

                //db.Livro.update("CodigoLivro": codigo, {"$set": {"Autor": novoAutor});
                await conexao.Livro.ReplaceOneAsync(condicao, doc);
            }
        }

        public async void AtualizarAnoLivro(int codigo, int novoAno)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.Livro.Find(condicao).ToListAsync();

            foreach (var doc in listaLivros)
            {
                doc.Ano = novoAno;
                //Método do update no C# com mongo                
                //Vou inserir o comando aqui demonstrando o tratamento do mesmo.

                //db.Livro.update("CodigoLivro": codigo, {"$set": {"Ano": novoAno});
                await conexao.Livro.ReplaceOneAsync(condicao, doc);
            }
        }

        public async void AtualizarQPaginasLivro(int codigo, int novaQuantidade)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.Livro.Find(condicao).ToListAsync();

            foreach (var doc in listaLivros)
            {
                doc.Paginas = novaQuantidade;
                //Método do update no C# com mongo                
                //Vou inserir o comando aqui demonstrando o tratamento do mesmo.

                //db.Livro.update("CodigoLivro": codigo, {"$set": {"Ano": novoAno});
                await conexao.Livro.ReplaceOneAsync(condicao, doc);
            }
        }

        public async void AtualizarQDisponivelLivro(int codigo, int novaQuantidade)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.Livro.Find(condicao).ToListAsync();

            foreach (var doc in listaLivros)
            {
                doc.QuantidadeDisponivel = novaQuantidade;
                //Método do update no C# com mongo                
                //Vou inserir o comando aqui demonstrando o tratamento do mesmo.

                //db.Livro.update("CodigoLivro": codigo, {"$set": {"Ano": novoAno});
                await conexao.Livro.ReplaceOneAsync(condicao, doc);
            }
        }

        public async void AtualizarValorAluguelLivro(int codigo, int novoValor)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.Livro.Find(condicao).ToListAsync();

            foreach (var doc in listaLivros)
            {
                doc.ValorAluguel = novoValor;
                //Método do update no C# com mongo                
                //Vou inserir o comando aqui demonstrando o tratamento do mesmo.

                //db.Livro.update("CodigoLivro": codigo, {"$set": {"Ano": novoAno});
                await conexao.Livro.ReplaceOneAsync(condicao, doc);
            }
        }

        public async void AtualizarAssuntoLivro(int codigo, string novosAssuntos)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.Livro.Find(condicao).ToListAsync();

            string[] vetAssunto = novosAssuntos.Split(',');
            List<string> vetAssunto2 = new List<string>();
            for (int i = 0; i <= vetAssunto.Length - 1; i++)
            {
                vetAssunto2.Add(vetAssunto[i].Trim());
            }

            foreach (var doc in listaLivros)
            {
                doc.Assunto = vetAssunto2;
                //Método do update no C# com mongo                
                //Vou inserir o comando aqui demonstrando o tratamento do mesmo.

                //db.Livro.update("CodigoLivro": codigo, {"$set": {"Assunto": ["Assunto"]});
                await conexao.Livro.ReplaceOneAsync(condicao, doc);
            }
        }
    }
}
