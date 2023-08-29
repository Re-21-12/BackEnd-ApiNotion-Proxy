using System;
using System.Runtime.Serialization;

public class NotionDataDto
{
    public string titulo { get; set; }
    public string descripcion { get; set; } 
    public string autor { get; set; }
    public string fecha_de_realizacion
    {
        get; set;
    }



    // Cambiado a string para representar la fecha formateada
    public DateTime subida_o_modificada
    {
        get; set;
    }
    public bool hecha
    {
        get; set;
    }
    // Agrega aquí las propiedades adicionales de acuerdo con la estructura de datos de Notion
    // Por ejemplo:
    // public string OtroCampo { get; set; }
}

