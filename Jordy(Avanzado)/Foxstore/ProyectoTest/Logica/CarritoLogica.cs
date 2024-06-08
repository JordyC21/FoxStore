using Foxstore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Foxstore.Logica
{
    public class CarritoLogica
    {
        private static CarritoLogica _instancia = null;

        public CarritoLogica()
        {

        }

        public static CarritoLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CarritoLogica();
                }

                return _instancia;
            }
        }

        public int Registrar(Carrito oCarrito)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", oCarrito.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdProducto", oCarrito.oProducto.IdProducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return respuesta;
        }


        public int Cantidad(int idusuario)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from carrito where idusuario = @idusuario", oConexion);
                    cmd.Parameters.AddWithValue("@idusuario", idusuario);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    respuesta = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return respuesta;
        }

        public List<Carrito> Obtener(int _idusuario)
        {
            List<Carrito> lst = new List<Carrito>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ObtenerCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", _idusuario);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lst.Add(new Carrito()
                            {
                                IdCarrito = Convert.ToInt32(dr["IdCarrito"].ToString()),
                                oProducto = new Producto()
                                {
                                    IdProducto = Convert.ToInt32(dr["IdProducto"].ToString()),
                                    Nombre = dr["Nombre"].ToString(),
                                    oMarca = new Marca() { Descripcion = dr["Descripcion"].ToString() },
                                    Precio = Convert.ToDecimal(dr["Precio"].ToString(), CultureInfo.CurrentCulture), // Utilizar la configuración regional del servidor
                                    RutaImagen = dr["RutaImagen"].ToString()
                                },
                                // Otros campos de Carrito...
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lst = new List<Carrito>();
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return lst;
        }

        public bool Eliminar(string IdCarrito, string IdProducto)
        {

            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("delete from carrito where idcarrito = @idcarrito");
                    query.AppendLine("update PRODUCTO set Stock = Stock + 1 where IdProducto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@idcarrito", IdCarrito);
                    cmd.Parameters.AddWithValue("@idproducto", IdProducto);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return respuesta;
        }

        public List<Compra> ObtenerCompra(int IdUsuario)
        {
            List<Compra> rptDetalleCompra = new List<Compra>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerCompra", oConexion);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                cmd.CommandType = CommandType.StoredProcedure;

#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    oConexion.Open();
                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            if (doc.Element("DATA") != null)
                            {
                                rptDetalleCompra = (from c in doc.Element("DATA").Elements("COMPRA")
                                                    select new Compra()
                                                    {
                                                        Total = (c.Element("Total") != null) ? Convert.ToDecimal(c.Element("Total").Value, new CultureInfo("es-US")) : 0,
                                                        FechaTexto = c.Element("Fecha") != null ? c.Element("Fecha").Value : string.Empty,
                                                        oDetalleCompra = (from d in c.Element("DETALLE_PRODUCTO")?.Elements("PRODUCTO") ?? Enumerable.Empty<XElement>()
                                                                          select new DetalleCompra()
                                                                          {
                                                                              oProducto = new Producto()
                                                                              {
                                                                                  oMarca = new Marca() { Descripcion = d.Element("Descripcion") != null ? d.Element("Descripcion").Value : string.Empty },
                                                                                  Nombre = d.Element("Nombre") != null ? d.Element("Nombre").Value : string.Empty,
                                                                                  RutaImagen = d.Element("RutaImagen") != null ? d.Element("RutaImagen").Value : string.Empty
                                                                              },
                                                                              Total = (d.Element("Total") != null) ? Convert.ToDecimal(d.Element("Total").Value, new CultureInfo("es-US")) : 0,
                                                                              Cantidad = (d.Element("Cantidad") != null) ? Convert.ToInt32(d.Element("Cantidad").Value) : 0
                                                                          }).ToList()
                                                    }).ToList();
                            }
                            else
                            {
                                rptDetalleCompra = new List<Compra>();
                            }
                        }

                        dr.Close();
                    }

                    return rptDetalleCompra;
                }
                catch (Exception ex)
                {
                    rptDetalleCompra = null;
                    return rptDetalleCompra;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
        }



    }
}