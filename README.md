# CRUD_MONGO_BIBLIOTECA
This project was created to practice the learning of databases in classroom at FAESA.

Link do Vídeo de apresentação do projeto no youtube: https://youtu.be/cbfybBqKPu8

Instalação para rodar projeto c# no visual studio code

--Instalar o SDK dotnet 6:

No windows: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

Em caso do linux, abra o terminal do linux e execute o comando a seguir: sudo snap install dotnet-sdk --classic --channel=6.0

sudo snap alias dotnet-sdk.dotnet dotnet

--verificar se o SDK dotnet 6 esta instalado:

dotnet --info

Vá até a pasta models, Abra a classe ConexãoBanco, aponte as configurações do seu banco de dados.

na loja do visual studio instalar: c# .NET Extension Pack

Coloque a seguinte string de conexão no Linux: string stringConexao = "mongodb://labdatabase:labDatabase2022@localhost:27017";


Ao baixar  o projeto no linux, também será necessário instalar o driver para rodar a conexão com o mongodb.

Abra a pasta CRUD_Mongo_Biblioteca que contém o arquivo Program.cs, abra um novo terminal e rode o seguinte comando para instalar o mongodb.driver: dotnet add package MongoDB.Driver --version 2.18.0 
