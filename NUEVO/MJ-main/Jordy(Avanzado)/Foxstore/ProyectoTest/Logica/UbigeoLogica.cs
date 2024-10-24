using Foxstore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Foxstore.Logica
{
    public class UbigeoLogica
    {

        private static UbigeoLogica _instancia = null;

        public UbigeoLogica()
        {

        }

        public static UbigeoLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new UbigeoLogica();
                }
                return _instancia;
            }
        }

        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> lst = new List<Departamento>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    SqlCommand cmd = new SqlCommand("select * from departamento", oConexion);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lst.Add(new Departamento()
                            {
                                IdDepartamento = dr["IdDepartamento"].ToString(),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lst = new List<Departamento>();
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return lst;
        }

        public List<Municipio> ObtenerMunicipio(string _iddepartamento)
        {
            List<Municipio> lst = new List<Municipio>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                try
                {
                    SqlCommand cmd = new SqlCommand("select * from MUNICIPIO where IdDepartamento = @iddepartamento", oConexion);
                    cmd.Parameters.AddWithValue("@iddepartamento", _iddepartamento);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lst.Add(new Municipio()
                            {
                                IdMunicipio = dr["IdMunicipio"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                IdDepartamento = dr["IdDepartamento"].ToString()
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lst = new List<Municipio>();
                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return lst;
        }

    }
}