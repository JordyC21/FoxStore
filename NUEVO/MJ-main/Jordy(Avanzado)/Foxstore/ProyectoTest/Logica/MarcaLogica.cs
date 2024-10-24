using Foxstore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Foxstore.Logica
{
    public class MarcaLogica
    {
        private static MarcaLogica _instancia = null;

        public MarcaLogica()
        {

        }

        public static MarcaLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new MarcaLogica();
                }

                return _instancia;
            }
        }

        public List<Marca> Listar()
        {

            List<Marca> rptListaMarca = new List<Marca>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_obtenerMarca", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaMarca.Add(new Marca()
                        {
                            IdMarca = Convert.ToInt32(dr["IdMarca"].ToString()),
                            Descripcion = dr["Descripcion"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaMarca;

                }
                catch (Exception ex)
                {
                    rptListaMarca = null;
                    return rptListaMarca;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
        }


        public bool Registrar(Marca oMarca)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", oConexion);
                    cmd.Parameters.AddWithValue("Descripcion", oMarca.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return respuesta;
        }

        public bool Modificar(Marca oMarca)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarMarca", oConexion);
                    cmd.Parameters.AddWithValue("IdMarca", oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("Descripcion", oMarca.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", oMarca.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa

            }

            return respuesta;

        }

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from Marca where idMarca = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa

            }

            return respuesta;

        }
    }
}