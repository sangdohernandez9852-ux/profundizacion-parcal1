using System;
using System.Collections.Generic;

class Program
{
    // Método para leer números enteros
    static int LeerEntero(string mensaje, int min, int max)
    {
        int numero;

        while (true)
        {
            Console.Write(mensaje);

            if (int.TryParse(Console.ReadLine(), out numero))
            {
                if (numero >= min && numero <= max)
                {
                    return numero;
                }
            }

            Console.WriteLine("[ERROR] Ingrese un número válido.");
        }
    }

    // Método para leer números decimales
    static decimal LeerDecimal(string mensaje, decimal min)
    {
        decimal numero;

        while (true)
        {
            Console.Write(mensaje);

            if (decimal.TryParse(Console.ReadLine(), out numero))
            {
                if (numero >= min)
                {
                    return numero;
                }
            }

            Console.WriteLine("[ERROR] Ingrese un valor válido.");
        }
    }

    // Método para calcular la factura
    static decimal CalcularFactura(
        decimal precio,
        int cantidad,
        bool tieneDescuento,
        out decimal montoIva,
        out decimal montoDescuento)
    {
        decimal subtotal = precio * cantidad;

        if (tieneDescuento)
        {
            montoDescuento = subtotal * 0.10m;
        }
        else
        {
            montoDescuento = 0;
        }

        montoIva = (subtotal - montoDescuento) * 0.19m;

        decimal total = subtotal - montoDescuento + montoIva;

        return total;
    }

    // Método para imprimir encabezados
    static void ImprimirEncabezado(string titulo)
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("              " + titulo);
        Console.WriteLine("==============================================");
    }

    static void Main()
    {
        // Listas para guardar los productos
        List<string> productos = new List<string>();
        List<decimal> precios = new List<decimal>();
        List<int> stock = new List<int>();
        List<int> unidadesVendidas = new List<int>();

        // Variables para las ventas
        int ventasRealizadas = 0;
        decimal totalCaja = 0;

        int opcion;

        do
        {
            ImprimirEncabezado("SISTEMA GESTOR DE VENTAS E INVENTARIO");

            Console.WriteLine("1. Registrar nuevo producto");
            Console.WriteLine("2. Consultar inventario");
            Console.WriteLine("3. Registrar una venta");
            Console.WriteLine("4. Ver reporte de caja");
            Console.WriteLine("5. Salir");
            Console.WriteLine();

            opcion = LeerEntero("Seleccione una opción (1-5): ", 1, 5);

            switch (opcion)
            {
                case 1:
                    ImprimirEncabezado("REGISTRAR NUEVO PRODUCTO");

                    string nombre;

                    while (true)
                    {
                        Console.Write("Ingrese el nombre del producto: ");
                        nombre = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(nombre))
                        {
                            Console.WriteLine("[ERROR] El nombre no puede estar vacío.");
                            continue;
                        }

                        bool existe = false;

                        for (int i = 0; i < productos.Count; i++)
                        {
                            if (productos[i].ToLower() == nombre.ToLower())
                            {
                                existe = true;
                                break;
                            }
                        }

                        if (existe)
                        {
                            Console.WriteLine("[ERROR] Ya existe un producto con ese nombre.");
                        }
                        else
                        {
                            break;
                        }
                    }

                    decimal precio = LeerDecimal(
                        "Ingrese el precio unitario: $", 0.01m);

                    int cantidadStock = LeerEntero(
                        "Ingrese el stock inicial: ", 0, int.MaxValue);

                    productos.Add(nombre);
                    precios.Add(precio);
                    stock.Add(cantidadStock);
                    unidadesVendidas.Add(0);

                    Console.WriteLine();
                    Console.WriteLine("[OK] Producto registrado correctamente.");
                    break;

                case 2:
                    ImprimirEncabezado("INVENTARIO COMPLETO");

                    if (productos.Count == 0)
                    {
                        Console.WriteLine("No hay productos registrados en el inventario.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "{0,-5} {1,-25} {2,15} {3,10}",
                            "ID", "Producto", "Precio", "Stock");

                        Console.WriteLine(
                            "----------------------------------------------------------");

                        for (int i = 0; i < productos.Count; i++)
                        {
                            string alerta = "";

                            if (stock[i] < 5)
                            {
                                alerta = " [ALERTA: BAJO STOCK]";
                            }

                            Console.WriteLine(
                                "{0,-5} {1,-25} {2,15:C2} {3,10}{4}",
                                i + 1,
                                productos[i],
                                precios[i],
                                stock[i],
                                alerta);
                        }
                    }

                    break;

                case 3:
                    ImprimirEncabezado("REGISTRAR VENTA");

                    if (productos.Count == 0)
                    {
                        Console.WriteLine("No hay productos registrados.");
                        break;
                    }

                    Console.WriteLine("Productos disponibles:");
                    Console.WriteLine();

                    for (int i = 0; i < productos.Count; i++)
                    {
                        string alerta = "";

                        if (stock[i] < 5)
                        {
                            alerta = " [ALERTA: BAJO STOCK]";
                        }

                        Console.WriteLine(
                            "{0}. {1} | Precio: {2:C2} | Stock: {3}{4}",
                            i + 1,
                            productos[i],
                            precios[i],
                            stock[i],
                            alerta);
                    }

                    Console.WriteLine();

                    int productoSeleccionado = LeerEntero(
                        "Seleccione el número del producto: ",
                        1,
                        productos.Count);

                    int posicion = productoSeleccionado - 1;

                    int cantidadVenta = LeerEntero(
                        "Ingrese la cantidad a comprar: ",
                        1,
                        int.MaxValue);

                    while (cantidadVenta > stock[posicion])
                    {
                        Console.WriteLine();
                        Console.WriteLine(
                            "[ERROR] Stock insuficiente. Solo quedan {0} unidades.",
                            stock[posicion]);

                        cantidadVenta = LeerEntero(
                            "Ingrese la cantidad a comprar: ",
                            1,
                            int.MaxValue);
                    }

                    bool tieneDescuento = false;

                    while (true)
                    {
                        Console.Write(
                            "¿Aplica descuento de cliente frecuente (10%)? (S/N): ");

                        string respuesta = Console.ReadLine().ToLower();

                        if (respuesta == "s")
                        {
                            tieneDescuento = true;
                            break;
                        }
                        else if (respuesta == "n")
                        {
                            tieneDescuento = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("[ERROR] Responda solamente S o N.");
                        }
                    }

                    decimal montoIva;
                    decimal montoDescuento;

                    decimal total = CalcularFactura(
                        precios[posicion],
                        cantidadVenta,
                        tieneDescuento,
                        out montoIva,
                        out montoDescuento);

                    decimal subtotal = precios[posicion] * cantidadVenta;

                    // Actualizar el stock
                    stock[posicion] = stock[posicion] - cantidadVenta;

                    // Actualizar las unidades vendidas
                    unidadesVendidas[posicion] =
                        unidadesVendidas[posicion] + cantidadVenta;

                    // Actualizar las ventas y la caja
                    ventasRealizadas++;
                    totalCaja = totalCaja + total;

                    Console.WriteLine();
                    ImprimirEncabezado("TICKET DE VENTA");

                    Console.WriteLine(
                        "Producto:        {0} (x{1})",
                        productos[posicion],
                        cantidadVenta);

                    Console.WriteLine(
                        "Subtotal:        {0:C2}",
                        subtotal);

                    Console.WriteLine(
                        "Descuento:      -{0:C2}",
                        montoDescuento);

                    Console.WriteLine(
                        "IVA (19%):       +{0:C2}",
                        montoIva);

                    Console.WriteLine("----------------------------------------------");

                    Console.WriteLine(
                        "TOTAL A PAGAR:   {0:C2}",
                        total);

                    Console.WriteLine("==============================================");

                    Console.WriteLine();
                    Console.WriteLine(
                        "[OK] Venta efectuada con éxito. Stock actualizado: {0} unidades.",
                        stock[posicion]);

                    break;

                case 4:
                    ImprimirEncabezado("REPORTE DE CAJA Y ESTADÍSTICAS");

                    Console.WriteLine(
                        "Total de ventas realizadas: " + ventasRealizadas);

                    Console.WriteLine(
                        "Total ingresado a caja:     {0:C2}",
                        totalCaja);

                    if (ventasRealizadas > 0)
                    {
                        decimal promedioVenta = totalCaja / ventasRealizadas;

                        Console.WriteLine(
                            "Promedio por venta:         {0:C2}",
                            promedioVenta);
                    }
                    else
                    {
                        Console.WriteLine(
                            "Promedio por venta:         $0,00");
                    }

                    if (productos.Count == 0 || ventasRealizadas == 0)
                    {
                        Console.WriteLine(
                            "Producto con más unidades vendidas: No hay ventas.");
                    }
                    else
                    {
                        int posicionMayor = 0;

                        for (int i = 1; i < unidadesVendidas.Count; i++)
                        {
                            if (unidadesVendidas[i] >
                                unidadesVendidas[posicionMayor])
                            {
                                posicionMayor = i;
                            }
                        }

                        Console.WriteLine(
                            "Producto con más unidades vendidas: {0} ({1} unidades)",
                            productos[posicionMayor],
                            unidadesVendidas[posicionMayor]);
                    }

                    break;

                case 5:
                    Console.WriteLine();
                    Console.WriteLine("Gracias por utilizar el sistema.");
                    Console.WriteLine("Programa finalizado correctamente.");
                    break;
            }

            if (opcion != 5)
            {
                Console.WriteLine();
                Console.WriteLine("Presione una tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }

        } while (opcion != 5);
    }
}