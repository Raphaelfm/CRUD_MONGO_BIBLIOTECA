using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Mongo_Biblioteca.Conexao;
using CRUD_Mongo_Biblioteca.Model;
using MongoDB.Driver;

namespace CRUD_Mongo_Biblioteca.Controller
{
    public class LivroAluguelController
    {
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();
        private LivroAluguel itemAluguel = new LivroAluguel();
        //private LivroController livros = new LivroController();
        //private Aluguel aluguel = new Aluguel();
        //private AluguelController alugueis = new AluguelController();

        //Método para inserir novos livros em aluguel
        public void CadastrarLivroAluguel()
        {
            bool running = true;
            int opcao = 0;
            Console.WriteLine("Bem vindo ao cadastro de aluguel de livros, a seguir informe os dados conforme solicitado!");
            Console.WriteLine();
            while (running)
            {
                AplicaNulos();

                Console.WriteLine("Veja abaixo as informações necessárias, e cadastre os dados conforme codigos de livros: ");
                Console.WriteLine();
                Console.WriteLine("Listando Livros...");
                Console.WriteLine();
                ListarLivros();
                Thread.Sleep(1000);
                Console.WriteLine();

                Console.WriteLine("Insira as informações conforme os dados apresentados: ");
                itemAluguel.CodigoAluguel = PegaCodigoAluguel().Result;
                itemAluguel.CodigoLeitor = PegaCodigoLeitorl().Result;
                Thread.Sleep(1000);
                Console.Write("Codigo Livro: ");
                string codigo = Console.ReadLine();
                itemAluguel.CodigoLivro = int.Parse(codigo);
                itemAluguel.Titulo = PegaTituloLivro(int.Parse(codigo)).Result;
                Thread.Sleep(1000);

                Console.Write("Quantidade que está sendo alugada: ");
                itemAluguel.QuantidadeLivro = int.Parse(Console.ReadLine());

                itemAluguel.ValorUnitarioLivro = PegaValorLivro(int.Parse(codigo)).Result;
                Thread.Sleep(1000);
                itemAluguel.ValorTotalLivro = itemAluguel.QuantidadeLivro * itemAluguel.ValorUnitarioLivro;

                conexao.LivroAluguel.InsertOneAsync(itemAluguel);
                Console.WriteLine("Documento incluído com sucesso!");
                ValorTotalAluguel(PegaCodigoAluguel().Result);
                Console.WriteLine("Deseja incluir outro livro ao aluguel? 1 - Sim, 0 - Não (Digite apenas o número da opção)");
                opcao = int.Parse(Console.ReadLine());

                if (opcao == 0)
                {
                    Console.WriteLine("Encerrando pedido...");
                    running = false;
                }
            }



            Console.Write("Pressione qualquer tecla para continuar: ");
            Console.ReadKey();
        }

        //Chama um relatório para livros alugados
        public async void RelatorioLivrosAlugados()
        {
            Console.WriteLine("Listando Documentos");

            var listaLivros = await conexao.LivroAluguel.Find(new BsonDocument())
                                                           .ToListAsync();
            Console.WriteLine("{0, -12} {1, -12} {2, -32} {3, -4} {4, 9}\n", "Codigo Livro - ", "Codigo Aluguel - ", "Titulo", "Quantidade Alugada", "Valor");
            foreach (var doc in listaLivros)
            {
                Console.WriteLine("{0, 12} {1, 14} {2, 32} {3,12} {4, 22}", doc.CodigoLivro, doc.CodigoAluguel, doc.Titulo, doc.QuantidadeLivro, doc.ValorTotalLivro);
            }

            Console.WriteLine("Fim da lista...");
        }

        //Menu para remoção de livros alugados
        public void RemoveLivroAluguel()
        {
            AplicaNulos();
            int opcao = 0;
            int codigo = 0;
            int codigoAluguel = 0;
            Console.WriteLine("Verifique o código do livro alugado e o codigo do Aluguel que deseja remover na lista abaixo: ");
            RelatorioLivrosAlugados();
            Thread.Sleep(2000);
            Console.WriteLine();
            Console.Write("Informe o código do livro que deseja remover: ");
            codigo = int.Parse(Console.ReadLine());

            Console.WriteLine("Informe o código do aluguel que esse livro pertence: ");
            codigoAluguel = int.Parse(Console.ReadLine());

            Console.WriteLine("Tem certeza que deseja excluir esse registro? 1 - Sim, 0 - Não");
            opcao = int.Parse(Console.ReadLine());

            if (opcao == 1)
            {
                Console.WriteLine("Ao remover este livro, caso no aluguel não tenha outro registro, o Aluguel também será excluído.");
                Console.Write("Deseja realemente continuar? 1 - Sim, 0 - Não (Digite apenas o número da opção): ");
                opcao = int.Parse(Console.ReadLine());
                if (opcao == 1)
                {
                    VerificaRegistroAluguel(codigo);
                    Thread.Sleep(2000);                    
                    ExcluiLivro(codigo, codigoAluguel);
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("Retornando ao menu de opções...");
                }
            }            
        }

        //Método para remover os livros de Aluguel
        public void ExcluiLivro(int codigo, int codigoAluguel)
        {
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo) & construtor.Eq(x => x.CodigoAluguel, codigoAluguel);

            Console.WriteLine($"Removendo o livro selecionado do aluguel {codigoAluguel}...");
            conexao.LivroAluguel.DeleteOneAsync(condicao);
            Console.WriteLine("Registro excluido com sucesso!");
        }

        //Verifica se no aluguel existe algum livro associado além do livro que está sendo excluído para determinar se o aluguel será excluido
        public async void VerificaRegistroAluguel(int codigo)
        {
            int existe = 0;
            int codigoAluguel = 0;
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.LivroAluguel.Find(condicao).ToListAsync();

            foreach (var cod in listaLivros)
            {
                codigoAluguel = cod.CodigoAluguel.Value;
            }

            construtor = Builders<LivroAluguel>.Filter;
            condicao = construtor.Eq(x => x.CodigoAluguel, codigoAluguel);

            listaLivros = await conexao.LivroAluguel.Find(condicao).ToListAsync();

            foreach (var doc in listaLivros)
            {
                if (doc.CodigoLivro != codigo)
                {
                    existe = 1;
                }
            }

            if (existe != 1)
            {
                var construtor1 = Builders<Aluguel>.Filter;
                var condicao1 = construtor1.Eq(x => x.CodigoAluguel, codigoAluguel);

                Console.WriteLine("Excluindo Aluguel");
                await conexao.Aluguel.DeleteOneAsync(condicao1);
                Console.WriteLine("Aluguel Excluido com sucesso!");
            }
        }

        //Conta a quantidade de registros existentes dentro da entidade LivroAluguel
        public int ContaEntidadeLivroAluguel()
        {
            int quantidadeAluguel = 0;
            var aluguel = conexao.LivroAluguel.CountDocuments(new BsonDocument());
            quantidadeAluguel = (int)aluguel;
            return quantidadeAluguel;
        }

        //Lista os livros cadastrados
        public async void ListarLivros()
        {
            var listaLivros = await conexao.Livro.Find(new BsonDocument())
                                                           .ToListAsync();
            //int quantidade = livros.ContaEntidadeLivro();
            foreach (var doc in listaLivros)
            {
                Console.WriteLine($"Codigo Livro: {doc.CodigoLivro} | Titulo: {doc.Titulo} | Estoque disponível: {doc.QuantidadeDisponivel}");
            }
        }

        //Pega o código do ultimo aluguel cadastrado para fazer a inclusão de livros no momento em que o usuário estiver registrando um Aluguel
        public async Task<int> PegaCodigoAluguel()
        {
            int codigo = 1;
            var listaAlugel = await conexao.Aluguel.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaAlugel)
            {
                if (doc.CodigoAluguel.HasValue)
                {
                    codigo = doc.CodigoAluguel.Value;
                }
            }
            return codigo;
        }

        //Pega o código do leitor vinculado ao ultimo aluguel para fazer a inclusão no momento em que o usuário estiver registrando um Aluguel
        public async Task<int> PegaCodigoLeitorl()
        {
            int codigo = 1;
            var listaAlugel = await conexao.Aluguel.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaAlugel)
            {
                if (doc.CodigoLeitor.HasValue)
                {
                    codigo = doc.CodigoLeitor.Value;
                }
            }
            return codigo;
        }

        //Pega os valores dos livros dentro da entidade livro para registrar no aluguel
        public async Task<double> PegaValorLivro(int codigo)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);
            double valor = 0;
            var listaLeitores = await conexao.Livro.Find(condicao)
                                                           .ToListAsync();

            foreach (var doc in listaLeitores)
            {
                valor = doc.ValorAluguel.Value;
            }

            return valor;
        }

        //Menu de alterações
        //Por lógica, decidimos deixar o usuário alterar apenas o livro. Pois o mesmo pode alterar o leitor através de Aluguel
        public void AlteraLivroAluguel()
        {
            int opcao = 0;
            int codigo = 0;
            bool running = true;

            while (running)
            {
                AplicaNulos();
                Console.WriteLine("Verifique o código do Livro que deseja alterar na lista abaixo: ");
                Console.WriteLine("Livros alugados...");
                Console.WriteLine();
                RelatorioLivrosAlugados();
                Thread.Sleep(2000);
                Console.WriteLine("Verifique os livros cadastrados no banco de dados e informe os dados quando solicitados... ");
                Console.WriteLine("Livros cadastrados... ");
                RelatorioLivros();
                Thread.Sleep(2000);
                Console.WriteLine();
                Console.WriteLine("Informe o campo que deseja alterar: (informe somente o número da opção)");
                Console.WriteLine();
                Console.WriteLine("1 - Alterar livro mantendo a quantidade");
                Console.WriteLine("2 - Alterar livro alterando a quantidade");                
                Console.WriteLine("3 - Voltar ao menu de opções anterior");
                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        Console.WriteLine("Informe o codigo do livro que deseja alterar: ");
                        codigo = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite o código do novo Livro: ");
                        int novoLivro = int.Parse(Console.ReadLine());
                        Console.WriteLine("Informe o código do Aluguel");
                        int codigoAluguel = int.Parse(Console.ReadLine());
                        AtualizarLivro(codigo, novoLivro, codigoAluguel);
                        Console.WriteLine();
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.Write("Pressione qualquer tecla para continuar... ");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.WriteLine("Informe o codigo do livro que deseja alterar: ");
                        codigo = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite o código do novo Livro: ");
                        int novoLivro1 = int.Parse(Console.ReadLine());
                        Console.WriteLine("Digite a quantidade alugada: ");
                        int novaQuantidade = int.Parse(Console.ReadLine());
                        Console.WriteLine("Informe o código do Aluguel");
                        codigoAluguel = int.Parse(Console.ReadLine());
                        AtualizarLivroQuantidade(codigo, novoLivro1, novaQuantidade, codigoAluguel);
                        Console.WriteLine();
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.Write("Pressione qualquer tecla para continuar... ");
                        Console.ReadKey();
                        break;                    
                    case 3:
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

                if(opcao == 3)
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

        //Pega o título do livro para inserir dentro do aluguel
        public async Task<string> PegaTituloLivro(int codigo)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);
            string titulo = "";
            var listaLeitores = await conexao.Livro.Find(condicao)
                                                           .ToListAsync();

            foreach (var doc in listaLeitores)
            {
                titulo = doc.Titulo;
            }

            return titulo;
        }

        //Limpa os atributos aplicando nulo
        public void AplicaNulos()
        {
            itemAluguel.Id = null;
            itemAluguel.CodigoAluguel = null;
            itemAluguel.CodigoLeitor = null;
            itemAluguel.QuantidadeLivro = null;
            itemAluguel.CodigoLivro = null;
            itemAluguel.Titulo = null;
            itemAluguel.ValorUnitarioLivro = null;
            itemAluguel.ValorTotalLivro = null;
        }

        //Método para atualizar o livro dentro de aluguel, conforme os novos dados informados pelo usuário, sem alterar a quantidade alugada
        public async void AtualizarLivro(int codigo, int novoLivro,int codigoAluguel)
        {            
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo) & construtor.Eq(x => x.CodigoAluguel, codigoAluguel);

            var listaLivros = await conexao.LivroAluguel.Find(condicao).ToListAsync();
            

            foreach (var doc in listaLivros)
            {
                doc.CodigoLivro = novoLivro;
                doc.Titulo = await PegaTituloLivro(novoLivro);
                Thread.Sleep(2000);
                doc.ValorUnitarioLivro = await PegaValorLivro(novoLivro);
                Thread.Sleep(2000);
                doc.ValorTotalLivro = doc.QuantidadeLivro * doc.ValorUnitarioLivro;
                //Método do update no C# com mongo
                //Vou inserir o comando aqui demonstrando o tratamento do mesmo.

                //db.Livro.update("CodigoLivro": codigo, {"$set": {"CodigoLivro": novoLivro});
                //db.Livro.update("CodigoLivro": novoLivro, {"$set": {"Titulo": doc.Titulo});
                //db.Livro.update("CodigoLivro": novoLivro, {"$set": {"ValorUnitarioLivro": doc.ValorUnitarioLivro});
                //db.Livro.update("CodigoLivro": novoLivro, {"$set": {"ValorTotalLivro": doc.ValorTotalLivro});

                var construtorAlteracaoLivroAluguel = Builders<LivroAluguel>.Update;
                var condicaoAlteracaoCodigoLivro = construtorAlteracaoLivroAluguel.Set(x => x.CodigoLivro, doc.CodigoLivro);
                var condicaoAlteracaoTituloLivro = construtorAlteracaoLivroAluguel.Set(x => x.Titulo, doc.Titulo);
                var condicaoAlteracaoValorUnitarioLivro = construtorAlteracaoLivroAluguel.Set(x => x.ValorUnitarioLivro, doc.ValorUnitarioLivro);
                var condicaoAlteracaoValorTotalLivro = construtorAlteracaoLivroAluguel.Set(x => x.ValorTotalLivro, doc.ValorTotalLivro);

                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoCodigoLivro);
                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoTituloLivro);
                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoValorUnitarioLivro);
                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoValorTotalLivro);
            }
        }

        //Método para atualizar o livro dentro de aluguel, conforme os novos dados informados pelo usuário, alterando a quantidade alugada
        public async void AtualizarLivroQuantidade(int codigo, int novoLivro1, int novaQuantidade, int codigoAluguel)
        {
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo) & construtor.Eq(x => x.CodigoAluguel, codigoAluguel);

            var listaLivros = await conexao.LivroAluguel.Find(condicao).ToListAsync();


            foreach (var doc in listaLivros)
            {
                doc.CodigoLivro = novoLivro1;
                doc.Titulo = await PegaTituloLivro(novoLivro1);
                Thread.Sleep(2000);
                doc.ValorUnitarioLivro = await PegaValorLivro(novoLivro1);
                Thread.Sleep(2000);
                doc.QuantidadeLivro = novaQuantidade;
                doc.ValorTotalLivro = novaQuantidade * doc.ValorUnitarioLivro;
                //Método do update no C# com mongo
                //Vou inserir o comando aqui demonstrando o tratamento do mesmo.

                //db.Livro.update("CodigoLivro": codigo, {"$set": {"CodigoLivro": novoLivro});
                //db.Livro.update("CodigoLivro": novoLivro, {"$set": {"Titulo": doc.Titulo});
                //db.Livro.update("CodigoLivro": novoLivro, {"$set": {"ValorUnitarioLivro": doc.ValorUnitarioLivro});
                //db.Livro.update("CodigoLivro": novoLivro, {"$set": {"ValorTotalLivro": doc.ValorTotalLivro});

                var construtorAlteracaoLivroAluguel = Builders<LivroAluguel>.Update;
                var condicaoAlteracaoCodigoLivro = construtorAlteracaoLivroAluguel.Set(x => x.CodigoLivro, doc.CodigoLivro);
                var condicaoAlteracaoTituloLivro = construtorAlteracaoLivroAluguel.Set(x => x.Titulo, doc.Titulo);
                var condicaoAlteracaoValorUnitarioLivro = construtorAlteracaoLivroAluguel.Set(x => x.ValorUnitarioLivro, doc.ValorUnitarioLivro);
                var condicaoAlteracaoQuantidade = construtorAlteracaoLivroAluguel.Set(x => x.QuantidadeLivro, doc.QuantidadeLivro);
                var condicaoAlteracaoValorTotalLivro = construtorAlteracaoLivroAluguel.Set(x => x.ValorTotalLivro, doc.ValorTotalLivro);

                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoCodigoLivro);
                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoTituloLivro);
                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoValorUnitarioLivro);
                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoQuantidade);
                await conexao.LivroAluguel.UpdateOneAsync(condicao, condicaoAlteracaoValorTotalLivro);
            }
        }

        //Chama relatório de livros cadastradas para auxiliar o usuário
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

        //Atualiza o valor total de alguel dentro de Aluguel
        public async void ValorTotalAluguel(int codigo)
        {
            double? valorTotal = 0;
            var construtor = Builders<Aluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoAluguel, codigo);

            var listaAluguel = await conexao.Aluguel.Find(condicao).ToListAsync();

            var construtor1 = Builders<LivroAluguel>.Filter;
            var condicao1 = construtor1.Eq(x => x.CodigoAluguel, codigo);

            var listaLivros = await conexao.LivroAluguel.Find(condicao1).ToListAsync();

            foreach (var doc in listaLivros)
            {
                valorTotal = valorTotal + doc.ValorTotalLivro;
            }

            var construtorAlteracao = Builders<Aluguel>.Update;
            var condicaoAlteracao = construtorAlteracao.Set(x => x.ValorTotal, valorTotal);
            await conexao.Aluguel.UpdateOneAsync(condicao, condicaoAlteracao);

        }
    }
}
