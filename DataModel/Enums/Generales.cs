using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Enums
{
    public enum TipoOrden
    {
        Compra,
        Venta,
        Inventario
    }
    public enum EstadoOrdenCompra
    {
        Pendiente,
        Ingresado,
        Recibido,
        Cancelado
    }
    public enum EstadoOrdenVenta
    {
        Pendiente,
        Ingresado,        
        Cancelado,
        Entregado,
        Facturado
    }
    public enum EstadoOrdenVentaCliente
    {
        Pendiente,
        Ingresado,
        CanceladoVerificar,
        Cancelado,
        Entregado,
        Facturado
    }
    public enum EstadoOrdenVentaCredito
    {
        Pendiente,
        Ingresado,        
        Entregado,
        Cancelado,
        Facturado
    }
    public enum EstadoCuotaPago
    {
        Pendiente,
        Abonada,
        Cancelada
    }
    public enum EstadoOrdenInventario
    {
        Pendiente,
        Ingresado,
        Distribuido
    }
    public enum TipoInventario
    {
        Inicial,
        Concilia,
        Transferencia,
        DarBaja
    }
}
