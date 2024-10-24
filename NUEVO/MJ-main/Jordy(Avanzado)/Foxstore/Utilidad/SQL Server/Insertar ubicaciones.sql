
GO
USE DB_FOXSTORE

go

INSERT INTO DEPARTAMENTO(IdDepartamento, Descripcion) VALUES
('01', 'Ahuachapán'),
('02', 'Santa Ana'),
('03', 'Sonsonate'),
('04', 'Chalatenango'),
('05', 'Cuscatlán'),
('06', 'San Salvador'),
('07', 'La Libertad'),
('08', 'San Vicente'),
('09', ' Cabañas'),
('10', 'La Paz'),
('11', 'Usulután'),
('12', 'San Miguel'),
('13', 'Morazán'),
('14', 'La Unión')

go

INSERT INTO MUNICIPIO(IdMunicipio, Descripcion, IdDepartamento) VALUES
--Municipios de Ahuachapán
('0101', 'Ahuachapán', '01'),
('0102', 'Apaneca', '01'),
('0103', 'Atiquizaya', '01'),
('0104', 'Concepción de Ataco', '01'),

--Municipios de  Santa Ana
('0201', 'Candelaria de la Frontera', '02'),
('0202', 'Chalchuapa', '02'),
('0203', 'Coatepeque', '02'),
('0204', 'El Congo', '02'),

--Municipios de Sonsonate
('0301', 'Acajutla', '03'),
('0302', 'Armenia', '03'),
('0303', 'Caluco', '03'),
('0304', 'Cuisnahuat', '03'),

--Municipios de Chalatenango
('0401', 'Agua Caliente', '04'),
('0402', 'Arcatao', '04'),
('0403', 'Azacualpa', '04'),
('0404', 'Chalatenango', '04')

go