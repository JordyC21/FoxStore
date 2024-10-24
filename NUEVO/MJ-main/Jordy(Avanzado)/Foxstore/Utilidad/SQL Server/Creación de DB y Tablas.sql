create database DB_FOXSTORE

GO
USE DB_FOXSTORE
GO

CREATE TABLE CATEGORIA(
IdCategoria int primary key identity,
Descripcion varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
SELECT *FROM CATEGORIA
go

CREATE TABLE MARCA(
IdMarca int primary key identity,
Descripcion varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
SELECT *FROM MARCA
go

CREATE TABLE PRODUCTO(
IdProducto int primary key identity,
Nombre varchar(500),
Descripcion varchar(500),
IdMarca int references Marca(IdMarca),
IdCategoria int references Categoria(IdCategoria),
Precio decimal(10,2) default 0,
Stock int,
RutaImagen varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
SELECT *FROM PRODUCTO
go

CREATE TABLE USUARIO(
IdUsuario int primary key identity,
Nombres varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Contrasena varchar(100),
EsAdministrador bit,
Activo bit default 1,
FechaRegistro datetime default getdate()
)
SELECT *FROM USUARIO
go

CREATE TABLE CARRITO(
IdCarrito int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
IdProducto int references PRODUCTO(IdProducto)
)
SELECT *FROM CARRITO
go

create table COMPRA(
IdCompra int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
TotalProducto int,
Total decimal(10,2),
Contacto varchar(50),
Telefono varchar(50),
Direccion varchar(500),
IdMunicipio varchar(10),
FechaCompra datetime default getdate()
)
SELECT *FROM COMPRA 
go

create table DETALLE_COMPRA(
IdDetalleCompra int primary key identity,
IdCompra int references Compra(IdCompra),
IdProducto int references PRODUCTO(IdProducto),
Cantidad int,
Total decimal(10,2)
)
SELECT *FROM DETALLE_COMPRA
go

CREATE TABLE DEPARTAMENTO (
  IdDepartamento varchar(2) NOT NULL,
  Descripcion varchar(45) NOT NULL
) 
SELECT *FROM DEPARTAMENTO
go

CREATE TABLE MUNICIPIO (
  IdMunicipio varchar(4) NOT NULL,
  Descripcion varchar(45) NOT NULL,
  IdDepartamento varchar(2) NOT NULL
) 
SELECT *FROM MUNICIPIO
go
