using Foxstore.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Foxstore.Logica
{
    public class CompraLogica
    {
        private static CompraLogica _instancia = null;

        public CompraLogica()
        {

        }

        public static CompraLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CompraLogica();
                }

                return _instancia;
            }
        }

        public bool Registrar(Compra oCompra)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    StringBuilder query = new StringBuilder();
                    foreach (DetalleCompra dc in oCompra.oDetalleCompra)
                    {
                        query.AppendLine("insert into detalle_compra(IdCompra,IdProducto,Cantidad,Total) values (¡idcompra!," + dc.IdProducto + "," + dc.Cantidad + "," + dc.Total + ")");
                    }

                    SqlCommand cmd = new SqlCommand("sp_registrarCompra", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", oCompra.IdUsuario);
                    cmd.Parameters.AddWithValue("TotalProducto", oCompra.TotalProducto);
                    cmd.Parameters.AddWithValue("Total", oCompra.Total);
                    cmd.Parameters.AddWithValue("Contacto", oCompra.Contacto);
                    cmd.Parameters.AddWithValue("Telefono", oCompra.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", oCompra.Direccion);
                    cmd.Parameters.AddWithValue("IdMunicipio", oCompra.IdMunicipio);
                    cmd.Parameters.AddWithValue("QueryDetalleCompra", query.ToString());
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                    // Restar la cantidad comprada del stock del producto
                    if (respuesta)
                    {
                        foreach (DetalleCompra dc in oCompra.oDetalleCompra)
                        {
                            RestarCantidadDelStock(dc.IdProducto, dc.Cantidad, oConexion);
                        }
                    }
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return respuesta;
        }

        private void RestarCantidadDelStock(int idProducto, int cantidad, SqlConnection conexion)
        {
            SqlCommand cmdVerificarStock = new SqlCommand("SELECT Stock FROM PRODUCTO WHERE IdProducto = @IdProducto", conexion);
            cmdVerificarStock.Parameters.AddWithValue("@IdProducto", idProducto);
            int stock = Convert.ToInt32(cmdVerificarStock.ExecuteScalar());

            if (cantidad > stock)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible para este producto.");
            }

            SqlCommand cmd = new SqlCommand("UPDATE PRODUCTO SET Stock = Stock - @Cantidad WHERE IdProducto = @IdProducto", conexion);
            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            cmd.ExecuteNonQuery();

            // Verificar si el stock del producto es igual a 0
            stock -= cantidad; // Actualizar el stock restante después de la compra
            if (stock == 0)
            {
                // Actualizar el estado del producto a no activo
                SqlCommand cmdActualizarEstado = new SqlCommand("UPDATE PRODUCTO SET Activo = 0 WHERE IdProducto = @IdProducto", conexion);
                cmdActualizarEstado.Parameters.AddWithValue("@IdProducto", idProducto);
                cmdActualizarEstado.ExecuteNonQuery();
            }
        }

    }
}