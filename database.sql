CREATE TABLE usuarios (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(255) NOT NULL,
    apellido NVARCHAR(255) NOT NULL,
    correo NVARCHAR(255) UNIQUE NOT NULL,
    contrasena NVARCHAR(255) NOT NULL,
    rol NVARCHAR(50) NOT NULL, -- Almacenar 'admin' o 'usuario'
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE categorias (
    id_categoria INT PRIMARY KEY IDENTITY(1,1),
    nombre_categoria NVARCHAR(255) NOT NULL,
    descripcion NVARCHAR(500),
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE productos (
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    nombre_producto NVARCHAR(255) NOT NULL,
    descripcion NVARCHAR(500),
    precio DECIMAL(18,2) NOT NULL,
    stock INT DEFAULT 0,
    id_categoria INT NOT NULL,
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_categoria) REFERENCES categorias(id_categoria)
);

CREATE TABLE ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_usuario INT NOT NULL,
    fecha_venta DATETIME DEFAULT CURRENT_TIMESTAMP,
    total DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

CREATE TABLE detalle_ventas (
    id_detalle INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (id_venta) REFERENCES ventas(id_venta),
    FOREIGN KEY (id_producto) REFERENCES productos(id_producto)
);

CREATE TABLE proveedores (
    id_proveedor INT PRIMARY KEY IDENTITY(1,1),
    nombre_proveedor NVARCHAR(255) NOT NULL,
    telefono NVARCHAR(50),
    correo NVARCHAR(255),
    direccion NVARCHAR(500),
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE pedidos (
    id_pedido INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT NOT NULL,
    fecha_pedido DATETIME DEFAULT CURRENT_TIMESTAMP,
    estado NVARCHAR(50) NOT NULL DEFAULT 'pendiente', -- 'pendiente', 'completado', 'cancelado'
    total DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (id_proveedor) REFERENCES proveedores(id_proveedor)
);

CREATE TABLE detalle_pedidos (
    id_detalle INT PRIMARY KEY IDENTITY(1,1),
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (id_pedido) REFERENCES pedidos(id_pedido),
    FOREIGN KEY (id_producto) REFERENCES productos(id_producto)
);

CREATE TABLE historial_stock (
    id_historial INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    tipo_movimiento NVARCHAR(50) NOT NULL, -- 'entrada', 'salida'
    descripcion NVARCHAR(500),
    fecha_movimiento DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_producto) REFERENCES productos(id_producto)
);

CREATE TABLE configuracion (
    id_config INT PRIMARY KEY IDENTITY(1,1),
    clave NVARCHAR(255) NOT NULL UNIQUE,
    valor NVARCHAR(500) NOT NULL,
    fecha_actualizacion DATETIME DEFAULT CURRENT_TIMESTAMP
);
