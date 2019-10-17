USE Coorganicas;

/*** Inserindo os dados nas tabelas do projeto Coorganicas ***/

-- Tabela Tipo_Usuario
INSERT INTO Tipo_Usuario(Tipo) VALUES('Administrador'),('Comunidade'),('Agricultor'); 

SELECT * FROM Tipo_Usuario;

-- Tabela Produto
INSERT INTO Produto(Nome, Descricao) VALUES
('Banana', 'Prata'),('Maçã', 'Verde'),('Pera', 'Amarela'); 

SELECT * FROM Produto;

-- Tabela Usuario
INSERT INTO Usuario(Nome, CNPJ, Senha, Email, Id_Tipo_Usuario) VALUES
('Admin', '0000000000000', '123', 'admin@admin.com', 1),
('Thais', '1234567899123', '123', 'thais@gmail.com', 2),
('João', '1234567899132', '123', 'toao@hotmail.com', 3);

SELECT * FROM Usuario;

-- Tabela Telefone
INSERT INTO Telefone(Telefone, Id_Usuario) VALUES('112233-5600', 2),('113266-5600', 3);

SELECT * FROM Telefone

-- Tabela Receita
INSERT INTO Receita(Titulo, Conteudo, Id_Usuario) VALUES
('Bolo de Chocolate', 'Testetestestestestestetstestesteste', 2),
('Bolo de Laranja', 'Testetestestestestestetstestesteste', 2);

SELECT * FROM Receita

-- Tabela Endereco
INSERT INTO Endereco(Cep, Endereco, Numero, Id_Usuario) VALUES
('02435000', 'Rua Teste, 123', 63, 2),
('02435123', 'Av Teste, 321', 6333, 3);

SELECT * FROM Endereco

-- Tabela Oferta
INSERT INTO Oferta(Preco, Cidade, Validade, Quantidade, Regiao, Id_Usuario, Id_Produto) VALUES
('3.99', 'São Paulo', '23/10/2019', 23.5, 'Central', '3', 1);

SELECT * FROM Oferta

-- Tabela Reserva
INSERT INTO Reserva(Quantidade, Status_Reserva, Id_Usuario, Id_Oferta) VALUES
(2.5, 'Aguardando', 2, 1);

SELECT * FROM Reserva

/*** Inserindo os dados nas tabelas do projeto Coorganicas ***/
