GO
USE DB_FOXSTORE
go

insert into USUARIO(Nombres,Apellidos,Correo,Contrasena,EsAdministrador) values ('test','user','admin@example.com','admin123',1)

GO
insert into CATEGORIA(Descripcion) values
('HOMBRE'),
('MUJER'),
('NIÑOS'),
('NIÑAS')

GO

insert into MARCA(Descripcion) values
('NIKE'),
('ADIDAS'),
('CONVERSE'),
('REEBOK'),
('PUMA')

GO

insert into PRODUCTO(Nombre,Descripcion,IdMarca,IdCategoria,Precio,Stock,RutaImagen) values('Consola de PS4 Pro 1TB Negro','Tipo: PS4
Procesador: AMD
Entradas USB: 3
Entradas HDMI: 1
Conectividad: WiFi',1,1,'2000','50','~/Imagenes/Productos/1.jpg')

go
