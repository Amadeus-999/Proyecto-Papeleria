USE papeleria

SELECT* FROM usuarios

SELECT * FROM productos

SELECT * FROM categorias

SELECT * FROM proveedores

SELECT * FROM ventas

SELECT * FROM pedidos

SELECT p.id_producto, p.nombre_producto, p.descripcion, p.precio, p.stock, p.fecha_registro, p.ImagenUri, c.nombre_categoria
FROM productos p
JOIN categorias c ON p.id_categoria = c.id_categoria

SELECT dv.id_producto, p.nombre_producto, dv.cantidad, dv.precio_unitario
FROM detalle_ventas dv
INNER JOIN productos p ON dv.id_producto = p.id_producto
WHERE dv.id_venta = 1

SELECT v.id_venta, v.id_usuario, u.nombre, v.fecha_venta, v.total 
FROM ventas v
INNER JOIN usuarios u ON v.id_usuario = u.id_usuario

SELECT dv.id_producto, p.nombre_producto, dv.cantidad, dv.precio_unitario
FROM detalle_ventas dv
INNER JOIN productos p ON dv.id_producto = p.id_producto
WHERE dv.id_venta =id_venta

INSERT INTO detalle_ventas (id_venta, id_producto, cantidad, precio_unitario)
VALUES
(1, 3, 2, 15.50),
(1, 5, 1, 30.00),
(2, 2, 1, 12.75),
(2, 4, 2, 22.90),
(3, 1, 5, 10.00),
(3, 6, 3, 18.20),
(4, 7, 1, 45.00),
(4, 8, 2, 25.75),
(5, 3, 3, 20.50),
(5, 10, 2, 35.00);


INSERT INTO ventas (id_usuario, fecha_venta, total)
VALUES
(1, '2024-12-01 10:00:00', 150.75),
(2, '2024-12-02 11:30:00', 250.50),
(3, '2024-12-03 15:45:00', 320.00),
(4, '2024-12-04 09:15:00', 430.25),
(5, '2024-12-05 17:00:00', 540.80);


INSERT INTO proveedores (nombre_proveedor, telefono, correo, direccion)
VALUES 
('Proveedor A', '1234567890', 'contacto@proveedora.com', 'Calle Ficticia 123, Ciudad A'),
('Proveedor B', '2345678901', 'contacto@proveedorb.com', 'Avenida Real 456, Ciudad B'),
('Proveedor C', '3456789012', 'contacto@proveedorc.com', 'Boulevard Principal 789, Ciudad C'),
('Proveedor D', '4567890123', 'contacto@proveedord.com', 'Plaza Mayor 101, Ciudad D'),
('Proveedor E', '5678901234', 'contacto@proveedore.com', 'Calle Luna 202, Ciudad E'),
('Proveedor F', '6789012345', 'contacto@proveedorf.com', 'Avenida Sol 303, Ciudad F'),
('Proveedor G', '7890123456', 'contacto@proveedorg.com', 'Calle Mar 404, Ciudad G'),
('Proveedor H', '8901234567', 'contacto@proveedorh.com', 'Paseo de la Reforma 505, Ciudad H'),
('Proveedor I', '9012345678', 'contacto@proveedori.com', 'Carretera Central 606, Ciudad I'),
('Proveedor J', '0123456789', 'contacto@proveedorj.com', 'Calle Estrella 707, Ciudad J');

INSERT INTO pedidos (id_proveedor, fecha_pedido, estado, total)
VALUES
(9, '2024-12-01 10:00:00', 'pendiente', 1500.00),
(2, '2024-12-02 11:15:00', 'completado', 2000.00),
(3, '2024-12-03 09:30:00', 'pendiente', 1200.50),
(4, '2024-12-04 14:45:00', 'cancelado', 800.00),
(5, '2024-12-05 13:20:00', 'pendiente', 2500.75),
(6, '2024-12-06 15:00:00', 'completado', 1300.30),
(7, '2024-12-07 08:10:00', 'pendiente', 1750.60),
(8, '2024-12-08 16:25:00', 'cancelado', 900.00),
(9, '2024-12-09 12:40:00', 'completado', 3000.45),
(10, '2024-12-10 10:55:00', 'pendiente', 1100.10);



